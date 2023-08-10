namespace NCRFiscalManager.Core.DTO.NCR
{
    public class PaymentDTO
    {
        public string IdentificadorPago { get; set; }
        public string PagoId { get; set; }
        public string PagoDescripcion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public float MontoPagado { get; set; }
        public float Propina { get; set; }
    }

}
