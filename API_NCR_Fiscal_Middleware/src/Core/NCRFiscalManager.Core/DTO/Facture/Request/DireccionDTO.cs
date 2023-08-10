using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class DireccionDTO
{
    [XmlAttribute("CodigoMunicipio")]
    public string CodigoMunicipio { get; set; }

    [XmlAttribute("NombreCiudad")]
    public string NombreCiudad { get; set; }

    [XmlAttribute("CodigoPostal")]
    public string CodigoPostal { get; set; }

    [XmlAttribute("NombreDepartamento")]
    public string NombreDepartamento { get; set; }

    [XmlAttribute("CodigoDepartamento")]
    public string CodigoDepartamento { get; set; }

    [XmlAttribute("Direccion")]
    public string Direccion { get; set; }

    [XmlAttribute("CodigoPais")]
    public string CodigoPais { get; set; }

    [XmlAttribute("NombrePais")]
    public string NombrePais { get; set; }

    [XmlAttribute("IdiomaPais")]
    public string IdiomaPais { get; set; }
}





