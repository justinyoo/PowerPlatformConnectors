namespace NhnToastSms.FunctionApp.Models
{
    public abstract class BaseResponseModel<T>
    {
        public virtual ResponseHeader Header { get; set; }
        public virtual T Body { get; set; }
    }

    public class ResponseHeader
    {
        public virtual bool IsSuccessful { get; set; }
        public virtual int Resultcode { get; set; }
        public virtual string ResultMessage { get; set; }
    }
}