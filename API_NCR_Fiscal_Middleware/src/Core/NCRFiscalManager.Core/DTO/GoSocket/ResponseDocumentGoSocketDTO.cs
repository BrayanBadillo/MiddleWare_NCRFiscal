namespace NCRFiscalManager.Core.DTO.GoSocket
{
    public class ResponseDocumentGoSocketDTO
    {
        public bool Success { get; set; }
        public string GlobalDocumentId { get; set; }
        public string CountryDocumentId { get; set; }
        public OtherDataDTO OtherData { get; set; }
        public List<string> Messages { get; set; }
        public string ResponseValue { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    
    public class OtherDataDTO
    {
        public string Country { get; set; }
        public string Certifier { get; set; }
        public string AuthorityTimeStamp { get; set; }
        public string BarCodeText { get; set; }
        public string Folio { get; set; }
        public string NumeroInterno { get; set; }
        public string Serie { get; set; }
    }
}