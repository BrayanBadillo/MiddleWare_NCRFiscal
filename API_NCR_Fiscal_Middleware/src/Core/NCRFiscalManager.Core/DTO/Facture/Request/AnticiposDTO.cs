using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class AnticiposDTO
{
    [XmlElement]
    public List<AnticipoDTO> Anticipo { get; set; }
}

public class AnticipoDTO
{
    [XmlAttribute("IDPago")]
    public string IDPago { get; set; }

    [XmlAttribute("ValorPagoAnticipo")]
    public float ValorPagoAnticipo { get; set; }

    [XmlAttribute("MonedaAnticipo")]
    public string MonedaAnticipo { get; set; }

    [XmlAttribute("FechaRecepcion")]
    public string FechaRecepcion { get; set; }
}
