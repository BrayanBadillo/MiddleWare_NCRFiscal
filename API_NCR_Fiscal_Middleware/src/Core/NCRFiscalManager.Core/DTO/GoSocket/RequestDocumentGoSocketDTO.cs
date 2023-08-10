namespace NCRFiscalManager.Core.DTO.GoSocket
{
    public class RequestDocumentGoSocketDTO
    {
        public string FileContent { get; set; }
        public bool Async { get; set; }
        public string Mapping { get; set; }
        public bool Sign { get; set; }
        public bool DefaultCertification { get; set; }
        public bool IsTxt { get; set; }
    }
}