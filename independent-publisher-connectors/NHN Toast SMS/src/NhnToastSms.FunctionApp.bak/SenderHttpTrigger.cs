using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using NhnToastSms.FunctionApp.Examples;
using NhnToastSms.FunctionApp.Models;

namespace NhnToastSms.FunctionApp
{
    public class SenderHttpTrigger
    {
        private const string UploadDocumentForAuthorizationUrl = "appKeys/{appKey}/requests/attachFiles/authDocuments";

        private readonly ILogger _logger;
        private readonly HttpClient _http;

        public SenderHttpTrigger(ILogger<SenderHttpTrigger> logger, HttpClient http)
        {
            this._logger = logger.ThrowIfNullOrDefault();
            this._http = http.ThrowIfNullOrDefault();
        }

        [Function(nameof(SenderHttpTrigger.UploadDocumentForAuthorization))]
        [OpenApiOperation(operationId: "Sender.UploadDocumentForAuthorization", tags: new[] { "sender" }, Summary = "Uploads a document", Description = "This uploads a document to get the sender's phone number authorized", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "secretKey", schemeType: SecuritySchemeType.ApiKey, Name = "X-Secret-Key", In = OpenApiSecurityLocationType.Header, Description = "Unique secret key")]
        [OpenApiParameter(name: "appKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Example = typeof(AppKeyExample), Summary = "Unique application key", Description = "This is the unique application key")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadDocumentRequestModel), Required = true, Description = "Document for authorization")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(UploadDocumentResponseModel), Example = typeof(UploadDocumentResponseExample), Summary = "Represents the document upload result", Description = "This represents the document upload result")]
        public async Task<HttpResponseData> UploadDocumentForAuthorization(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = UploadDocumentForAuthorizationUrl)] HttpRequestData req, string appKey)
        {
            var payload = default(string);
            using (var reader = new StreamReader(req.Body))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var requestUrl = $"{Environment.GetEnvironmentVariable("OpenApi__HostNames").TrimEnd('/')}/{UploadDocumentForAuthorizationUrl}".Replace("{appKey}", appKey);
            var content = new StreamContent(req.Body);
            var boundary = req.Headers
                              .Single(p => p.Key == "Content-Type").Value.First()
                                  .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                      .Last().Trim()
                                      .Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries)
                                          .Last().Trim();
            content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));

            var cnt = new MultipartFormDataContent(boundary);

            //this._http.DefaultRequestHeaders.Add("Content-Type", contentType);
            this._http.DefaultRequestHeaders.Add("X-Secret-Key", "002ojfhy");
            var res = default(UploadDocumentResponseModel);
            using (var msg = await this._http.PostAsync(requestUrl, content).ConfigureAwait(false))
            {
                res = await msg.Content.ReadAsAsync<UploadDocumentResponseModel>().ConfigureAwait(false);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(res).ConfigureAwait(false);

            return response;
        }
    }
}