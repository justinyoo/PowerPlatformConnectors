namespace NhnToastSms.FunctionApp.Models
{
    public class UploadDocumentResponseModel : BaseResponseModel<FileResponseBody>
    {
    }

    public class FileResponseBody
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