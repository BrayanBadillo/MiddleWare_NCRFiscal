using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class FacturasRelacionadasDTO
{
    [XmlElement("FacturaRelacionada")]
    public FacturaRelacionadaDTO FacturaRelacionada { get; set; }
}

public class FacturaRelacionadaDTO
{
    [XmlAttribute("Numero")]
    public string Numero { get; set; }
    
    [XmlAttribute("CUFE")]
    public string Cufe { get; set; }
    
    [XmlAttribute("FechaEmisionFA")]
    public string FechaEmisionFA { get; set; }
}