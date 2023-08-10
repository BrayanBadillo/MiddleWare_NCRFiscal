using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class ContactoDTO
{
    [XmlAttribute("Nombre")]
    public string Nombre { get; set; }

    [XmlAttribute("Telefono")]
    public string Telefono { get; set; }


    [XmlAttribute("Telfax")]
    public int Telfax { get; set; }

    [XmlAttribute("Email")]
    public string Email { get; set; }

    [XmlAttribute("Notas")]
    public string Notas { get; set; }
}





