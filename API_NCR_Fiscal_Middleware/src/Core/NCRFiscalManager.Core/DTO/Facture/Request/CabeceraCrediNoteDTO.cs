using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class CabeceraCrediNoteDTO
{
    
    [XmlAttribute("Numero")]
    public string Numero { get; set; }
    
    [XmlAttribute("Prefijo")]
    public string Prefijo { get; set; }

    [XmlAttribute("OrdenCompra")]
    public string OrdenCompra { get; set; }

    [XmlAttribute("FechaEmision")]
    public string FechaEmision { get; set; }

    [XmlAttribute("HoraEmision")]
    public string HoraEmision { get; set; }

    [XmlAttribute("Observaciones")]
    public string Observaciones { get; set; }

    [XmlAttribute("MonedaNota")]
    public string MonedaNota { get; set; }

    [XmlAttribute("FormaDePago")]
    public int FormaDePago { get; set; }

    [XmlAttribute("LineasDeNota")]
    public long LineasDeNota { get; set; }

    [XmlAttribute("TipoOperacion")]
    public string TipoOperacion { get; set; }

    [XmlAttribute("Ambiente")]
    public int Ambiente { get; set; }
}