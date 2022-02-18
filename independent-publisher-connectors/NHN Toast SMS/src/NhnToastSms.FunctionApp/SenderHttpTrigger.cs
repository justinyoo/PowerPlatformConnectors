using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using NhnToastSms.FunctionApp.ActionResults;
using NhnToastSms.FunctionApp.Builders;
using NhnToastSms.FunctionApp.Configurations;
using NhnToastSms.FunctionApp.Examples;
using NhnToastSms.FunctionApp.Models;

namespace NhnToastSms.FunctionApp
{
    public class SenderHttpTrigger
    {
        private readonly ToastSettings _settings;
        private readonly HttpClient _http;
        private readonly ILogger _logger;

        public SenderHttpTrigger(ToastSettings settings, IHttpClientFactory factory, ILogger<SenderHttpTrigger> logger)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("sender");
            this._logger = logger.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(SenderHttpTrigger.UploadDocumentForAuthorization))]
        [OpenApiOperation(operationId: "Sender.Authorization.UploadDocument", tags: new[] { "sender" }, Summary = "Uploads a document", Description = "This uploads a document to get the sender's phone number authorized", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "app_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Unique application key")]
        [OpenApiSecurity(schemeName: "secret_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Unique secret key")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadDocumentRequestModel), Required = true, Description = "Document for authorization")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(UploadDocumentResponseModel), Example = typeof(UploadDocumentResponseExample), Summary = "Represents the successful operation", Description = "This represents the successful operation")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: ContentTypes.ApplicationJson, bodyType: typeof(UploadDocumentResponseModel), Example = typeof(UploadDocumentResponseExample), Summary = "Represents the document upload failure", Description = "This represents the document upload failure")]
        public async Task<IActionResult> UploadDocumentForAuthorization(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.POST, Route = "sender/authorization/upload-document")] HttpRequest req)
        {
            var headers = await req.To<RequestHeaderModel>(SourceFrom.Header).ConfigureAwait(false);
            var content = await req.ToMultipartFormDataContent().ConfigureAwait(false);

            var requestUrl = new RequestUrlBuilder(this._settings)
                                 .WithApiUrl(ApiUrls.UploadDocumentForAuthorizationUrl)
                                 .Build(new Dictionary<string, object>() { { "{appKey}", headers.AppKey } });

            var res = default(UploadDocumentResponseModel);
            this._http.DefaultRequestHeaders.Add(ApiHeaders.SecretHeaderKey, headers.SecretKey);
            using (var msg = await this._http.PostAsync(requestUrl, content).ConfigureAwait(false))
            {
                res = await msg.Content.ReadAsAsync<UploadDocumentResponseModel>().ConfigureAwait(false);
            }

            var result = default(IActionResult);
            if (res.Header.IsSuccessful)
            {
                result = new OkObjectResult(res);
            }
            else
            {
                result = new InternalServerErrorObjectResult(res);
            }

            return result;
        }
    }
}