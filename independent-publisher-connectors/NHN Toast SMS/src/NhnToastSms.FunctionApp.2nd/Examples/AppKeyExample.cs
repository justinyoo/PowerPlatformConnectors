using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace NhnToastSms.FunctionApp.Examples
{
    public class AppKeyExample : OpenApiExample<string>
    {
        public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("demoapp", "RoQggV7WSoo15e9", namingStrategy));

            return this;
        }
    }
}