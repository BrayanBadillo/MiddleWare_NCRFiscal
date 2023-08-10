namespace NCRFiscalManager.Core.DTO.NCR;

public class DescuentosDTO
{
    public long Identificador { get; set; }
    public int DescuentoId { get; set; }
    public string NombreDescuento { get; set; }
    public string ReferenciaDescuento { get; set; }
    public float PorcentajeDescuento { get; set; }
    public float MontoDescuento { get; set; }
    public int CajeroId { get; set; }
    public int GerenteId { get; set; }
}
