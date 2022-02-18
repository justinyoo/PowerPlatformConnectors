using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using NhnToastSms.FunctionApp.Models;

namespace NhnToastSms.FunctionApp.Examples
{
    public class UploadDocumentResponseExample : OpenApiExample<UploadDocumentResponseModel>
    {
        public override IOpenApiExample<UploadDocumentResponseModel> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve("result",
                new UploadDocumentResponseModel()
                {
                    Header = new ResponseHeader() { IsSuccessful = true, Resultcode = 100, ResultMessage = "Success" },
                    File = new FileResponse() { FileId = 123, FileName = "document.pdf", FilePath = "/uploaded/document.pdf" },
                },
                namingStrategy));

            return this;
        }
    }
}