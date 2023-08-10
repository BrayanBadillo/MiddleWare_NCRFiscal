using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class EmisorDTO
{
    [XmlAttribute("TipoPersona")]
    public string TipoPersona { get; set; }

    [XmlAttribute("TipoRegimen")]
    public string TipoRegimen { get; set; }

    [XmlAttribute("TipoIdentificacion")]
    public string TipoIdentificacion { get; set; }

    [XmlAttribute("Numeroidentificacion")]
    public string NumeroIdentificacion { get; set; }

    [XmlAttribute("DV")]
    public string DV { get; set; }

    [XmlAttribute("RazonSocial")]
    public string RazonSocial { get; set; }

    [XmlAttribute("NumeroMatriculaMercantil")]
    public string NumeroMatriculaMercantil { get; set; }

    [XmlAttribute("NombreComercial")]
    public string NombreComercial { get; set; }

    [XmlElement("CodigosCIIU")]
    public List<CIIUDTO> CodigosCIIU { get; set; }

    [XmlElement("Direccion")]
    public DireccionDTO Direccion { get; set; }

    [XmlElement("ObligacionesEmisor")]
    public List<CodigoObligacionDTO> ObligacionesEmisor { get; set; }

    [XmlElement("DireccionFiscal")]
    public DireccionFiscalDTO DireccionFiscal { get; set; }

    [XmlElement("TributoEmisor")]
    public TributoEmisorDTO TributoEmisor { get; set; }

    [XmlElement("Contacto")]
    public ContactoDTO Contacto { get; set; }

    [XmlElement("Consorcio")]
    public List<ParticipanteDTO> Consorcio { get; set; }
}

public class CIIUDTO
{
    public string CIIU { get; set; }
}

public class CodigoObligacionDTO
{
    [XmlElement("CodigoObligacion")]
    public string CodigoObligacion { get; set; }
}

public class TributoEmisorDTO
{
    [XmlAttribute("CodigoTributo")]
    public string CodigoTributo { get; set; }

    [XmlAttribute("NombreTributo")]
    public string NombreTributo { get; set; }
}

public class ParticipanteDTO
{
    [XmlElement("PersonaNatural")]
    public List<CodigoObligacionDTO> Obligaciones { get; set; }

    [XmlAttribute("RazonSocial")]
    public string RazonSocial { get; set; }

    [XmlAttribute("Nit")]
    public int Nit { get; set; }

    [XmlAttribute("DV")]
    public int DV { get; set; }

    [XmlAttribute("Regimen")]
    public string Regimen { get; set; }

    [XmlAttribute("CodigoTributo")]
    public string CodigoTributo { get; set; }

    [XmlAttribute("NombreTributo")]
    public string NombreTributo { get; set; }

    [XmlAttribute("Porcentaje")]
    public int Porcentaje { get; set; }
}





