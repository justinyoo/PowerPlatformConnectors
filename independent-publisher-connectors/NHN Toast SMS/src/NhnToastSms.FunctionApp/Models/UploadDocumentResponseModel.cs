namespace NhnToastSms.FunctionApp.Models
{
    public class UploadDocumentResponseModel
    {
        public virtual ResponseHeader Header { get; set; }
        public virtual ResponseBody Body { get; set; }
    }

    public class ResponseHeader
    {
        public virtual bool IsSuccessful { get; set; }
        public virtual int Resultcode { get; set; }
        public virtual string ResultMessage { get; set; }
    }

    public class ResponseBody
    {
        public virtual FileResponse Data { get; set; }
    }

    public class FileResponse
    {
        public virtual int FileId { get; set; }
        public virtual string FileName { get; set; }
        public virtual string FilePath { get; set; }
    }
}