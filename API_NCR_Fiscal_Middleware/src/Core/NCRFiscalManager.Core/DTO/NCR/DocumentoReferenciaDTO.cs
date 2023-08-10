namespace NCRFiscalManager.Core.DTO.NCR
{    public class DocumentoReferenciaDTO
    {
        public string TipoDocumnetoRef { get; set; }
        public string NumeroSerie { get; set; }
        public string NumeroResolucion { get; set; }
        public string CorrelativoFiscal { get; set; }
        public string ConceptoNotaCredito { get; set; }
        public DateTime FechaEmision { get; set; }
        public string CufeReference { get; set; }
    }
}

