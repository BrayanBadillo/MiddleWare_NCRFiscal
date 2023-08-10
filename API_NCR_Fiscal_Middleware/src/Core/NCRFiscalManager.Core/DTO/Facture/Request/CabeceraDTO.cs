using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class CabeceraDTO
{
    [XmlAttribute("Numero")]
    public string Numero { get; set; }

    [XmlAttribute("OrdenCompra")]
    public string OrdenCompra { get; set; }

    [XmlAttribute("FechaEmision")]
    public string FechaEmision { get; set; }

    [XmlAttribute("HoraEmision")]
    public string HoraEmision { get; set; }

    [XmlAttribute("MonedaFactura")]
    public string MonedaFactura { get; set; }

    [XmlAttribute("Observaciones")]
    public string Observaciones { get; set; }

    [XmlAttribute("TipoFactura")]
    public string TipoFactura { get; set; }

    [XmlAttribute("FormaDePago")]
    public int FormaDePago { get; set; }

    [XmlAttribute("LineasDeFactura")]
    public long LineasDeFactura { get; set; }

    [XmlAttribute("TipoOperacion")]
    public string TipoOperacion { get; set; }

    [XmlAttribute("Ambiente")]
    public int Ambiente { get; set; }
}


