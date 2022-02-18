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
                OpenApiExampleResolver.Resolve("success",
                new UploadDocumentResponseModel()
                {
                    Header = new ResponseHeader()
                    {
                        IsSuccessful = true,
                        Resultcode = 0,
                        ResultMessage = "SUCCESS"
                    },
                    Body = new ResponseBody()
                    {
                        Data = new FileResponse()
                        {
                            FileId = 12345678,
                            FileName = "document.pdf",
                            FilePath = "/temporary/9876/sendno-2014-04-16/5432/12345678"
                        }
                    },
                },
                namingStrategy));

            this.Examples.Add(
                OpenApiExampleResolver.Resolve("failure",
                new UploadDocumentResponseModel()
                {
                    Header = new ResponseHeader()
                    {
                        IsSuccessful = false,
                        Resultcode = -9998,
                        ResultMessage = "File upload error"
                    },
                    Body = null,
                },
                namingStrategy)); ;

            return this;
        }
    }
}