using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json.Converters;

namespace NhnToastSms.FunctionApp.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthorizationStatusType
    {
        [Display("SRS01")]
        AuthorizationRequested = 1,

        [Display("SRS02")]
        Reviewing = 2,

        [Display("SRS03")]
        AuthorizationCompleted = 3,

        [Display("SRS04")]
        AuthorizationRejected = 4,

        [Display("SRS05")]
        PhoneBeingCertified = 5,

        [Display("SRS06")]
        PhoneCertifyingFailed = 6,

        [Display("SRS07")]
        ManualAuthorizationCompleted = 7,
    }
}