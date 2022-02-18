using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

// using Aliencube.AzureFunctions.Extensions.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using NhnToastSms.FunctionApp.Examples;
using NhnToastSms.FunctionApp.Models;

namespace NhnToastSms.FunctionApp
{
    public static class SenderHttpTrigger
    {
        private const string UploadDocumentForAuthorizationUrl = "appKeys/{appKey}/requests/attachFiles/authDocuments";

        // private readonly ILogger _logger;
        // private readonly HttpClient _http;

        // public SenderHttpTrigger(ILogger<SenderHttpTrigger> logger, HttpClient http)
        // {
        //     this._logger = logger.ThrowIfNullOrDefault();
        //     this._http = http.ThrowIfNullOrDefault();
        // }

        [FunctionName(nameof(SenderHttpTrigger.UploadDocumentForAuthorization))]
        [OpenApiOperation(operationId: "Sender.UploadDocumentForAuthorization", tags: new[] { "sender" }, Summary = "Uploads a document", Description = "This uploads a document to get the sender's phone number authorized", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "secretKey", schemeType: SecuritySchemeType.ApiKey, Name = "X-Secret-Key", In = OpenApiSecurityLocationType.Header, Description = "Unique secret key")]
        [OpenApiParameter(name: "appKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Example = typeof(AppKeyExample), Summary = "Unique application key", Description = "This is the unique application key")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UploadDocumentRequestModel), Required = true, Description = "Document for authorization")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UploadDocumentResponseModel), Example = typeof(UploadDocumentResponseExample), Summary = "Represents the document upload result", Description = "This represents the document upload result")]
        public static async Task<IActionResult> UploadDocumentForAuthorization(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = UploadDocumentForAuthorizationUrl)] HttpRequest req,
            string appKey,
            ILogger log)
        {
            var file = await req.ReadAsStringAsync().ConfigureAwait(false);
            //var requestUrl = $"{Environment.GetEnvironmentVariable("OpenApi__HostNames").TrimEnd('/')}/sms/v3.0/{UploadDocumentForAuthorizationUrl}".Replace("{appKey}", appKey);
            //await this._http.PostAsync(requestUrl, )

            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(SenderHttpTrigger.RunMultiPartFormData))]
        [OpenApiOperation(operationId: "run.multipart.formdata", tags: new[] { "multipartformdata" }, Summary = "Transfer image through multipart/formdata", Description = "This transfers an image through multipart/formdata.", Visibility = OpenApiVisibilityType.Advanced)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(MultiPartFormDataModel), Required = true, Description = "Image data")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Summary = "Image data", Description = "This returns the image", Deprecated = false)]
        public static async Task<IActionResult> RunMultiPartFormData(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "form/multipart")] HttpRequest req,
            ILogger log)
        {
            var files = req.Form.Files;
            var file = files[0];

            var content = default(byte[]);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms).ConfigureAwait(false);
                content = ms.ToArray();
            }

            var result = new FileContentResult(content, "image/png");

            return result;
        }
    }
}