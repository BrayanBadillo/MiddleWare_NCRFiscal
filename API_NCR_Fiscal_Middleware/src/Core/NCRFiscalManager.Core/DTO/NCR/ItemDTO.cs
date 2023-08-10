namespace NCRFiscalManager.Core.DTO.NCR
{
    public class ItemDTO
    {
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public int Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public float PrecioUnitario { get; set; }
        public float Descuento { get; set; }
        public string DescripcionDescuento { get; set; }
        public float MontoIVA { get; set; }
        public float TasaIva { get; set; }
        public string DescripcionIVA { get; set; }
        public float MontoGravado { get; set; }
        public float MontoExento { get; set; }
        public float MontoExonerado { get; set; }
        public float Total { get; set; }
    }

}
