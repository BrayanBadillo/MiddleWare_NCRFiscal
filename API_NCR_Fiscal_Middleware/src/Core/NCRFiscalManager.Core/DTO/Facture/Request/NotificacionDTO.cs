using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class NotificacionDTO
{
    [XmlElement("Para")]
    public List<ParaDTO> Para { get; set; }

    [XmlAttribute("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute("De")]
    public string De { get; set; }

}

public class ParaDTO
{
    [XmlText()]
    public string Para { get; set; }
}



