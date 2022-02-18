using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using NhnToastSms.FunctionApp.Converters;
using NhnToastSms.FunctionApp.Enums;

namespace NhnToastSms.FunctionApp.Models
{
    public class GetAuthorizationStatusResponseModel : BaseResponseModel<AuthorizationStatusResponseBody>
    {
    }

    public class AuthorizationStatusResponseBody
    {
        [JsonProperty("pageNum")]
        public virtual int PageNumber { get; set; }

        public virtual int PageSize { get; set; }

        public virtual int TotalCount { get; set; }

        public virtual List<AuthorizationStatusResponse> Data { get; set; } = new List<AuthorizationStatusResponse>();
    }

    public class AuthorizationStatusResponse
    {
        public virtual AuthorizationRequestType AuthType { get; set; }

        [JsonProperty("sendNos")]
        public virtual List<string> SenderNumbers { get; set; } = new List<string>();

        public virtual string Comment { get; set; }

        public virtual List<int> FileIds { get; set; }

        public virtual AuthorizationRequestStatusType Status { get; set; }

        [JsonConverter(typeof(ToastDateTimeConverter))]
        public virtual DateTimeOffset CreateDate { get; set; }

        [JsonConverter(typeof(ToastDateTimeConverter))]
        public virtual DateTimeOffset UpdateDate { get; set; }

        [JsonConverter(typeof(ToastDateTimeConverter))]
        public virtual DateTimeOffset ConfirmDate { get; set; }
    }
}