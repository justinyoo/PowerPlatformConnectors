namespace NhnToastSms.FunctionApp.Models
{
    public class UploadDocumentResponseModel
    {
        public ResponseHeader Header { get; set; }
        public FileResponse File { get; set; }
    }

    public class ResponseHeader
    {
        public bool IsSuccessful { get; set; }
        public int Resultcode { get; set; }
        public string ResultMessage { get; set; }
    }

    public class FileResponse
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}