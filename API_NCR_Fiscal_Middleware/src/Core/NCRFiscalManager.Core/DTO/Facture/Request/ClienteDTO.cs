using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class ClienteDTO
{
    [XmlElement("PersonaNatural")]
    public PersonaNaturalDTO PersonaNatural { get; set; }

    [XmlElement("Direccion")]
    public DireccionDTO Direccion { get; set; }

    [XmlElement("ObligacionesCliente")]
    public List<CodigoObligacionDTO> ObligacionesCliente { get; set; }

    [XmlElement("DirrecionFiscal")]
    public DireccionFiscalDTO DireccionFiscal { get; set; }

    [XmlElement("Contacto")]
    public ContactoDTO Contacto { get; set; }

    [XmlElement("TributoCliente")]
    public TributoClienteDTO TributoCliente { get; set; }

    [XmlElement("ClientesAdicionales")]
    public List<ClienteAdicionalDTO> ClientesAdicionales { get; set; }

    [XmlAttribute("TipoPersona")]
    public string TipoPersona { get; set; }

    [XmlAttribute("TipoRegimen")]
    public string TipoRegimen { get; set; }

    [XmlAttribute("TipoIdentificacion")]
    public string TipoIdentificacion { get; set; }

    [XmlAttribute("NumeroIdentificacion")]
    public string NumeroIdentificacion { get; set; }

    [XmlAttribute("DV")]
    public int DV { get; set; }

    [XmlAttribute("NombreComercial")]
    public string NombreComercial { get; set; }

    [XmlAttribute("RazonSocial")]
    public string RazonSocial { get; set; }

    [XmlAttribute("NumeroMatriculaMercantil")]
    public string NumeroMatriculaMercantil { get; set; }
   
}

public class PersonaNaturalDTO
{
    [XmlElement("DocumentoReferencia")]
    public DocumentoReferenciaFactureDTO DocumentoReferencia { get; set; }

    [XmlElement("Direccion")]
    public PersonaNaturalDireccionDTO Direccion { get; set; }

    [XmlAttribute("DireccionesAdicionales")]
    public List<string> DireccionesAdicionales { get; set; }

    [XmlAttribute("TipoIdentificacion")]
    public int TipoIdentificacion { get; set; }

    [XmlAttribute("NumeroIdentificacion")]
    public string NumeroIdentificacion { get; set; }

    [XmlAttribute("PrimerNombre")]
    public string PrimerNombre { get; set; }

    [XmlAttribute("SegundoNombre")]
    public string SegundoNombre { get; set; }

    [XmlAttribute("Apellido")]
    public string Apellido { get; set; }
}

public class DocumentoReferenciaFactureDTO
{
    [XmlAttribute("DocReferenciaId")]
    public string DocReferenciaID { get; set; }

    [XmlAttribute("DocReferenciaNombre")]
    public string DocReferenciaNombre { get; set; }

    [XmlAttribute("Entidad")]
    public string Entidad { get; set; }

    [XmlAttribute("PaisEntidad")]
    public string PaisEntidad { get; set; }

    [XmlAttribute("CodigoPaisEntidad")]
    public string CodigoPaisEntidad { get; set; }

    [XmlAttribute("DocReferenciaCodigo")]
    public string DocReferenciaCodigo { get; set; }
}


public class PersonaNaturalDireccionDTO
{
    [XmlAttribute("CodigoCiudad")]
    public ushort CodigoCiudad { get; set; }

    [XmlAttribute("NombreCiudad")]
    public string NombreCiudad { get; set; }

    [XmlAttribute("CodigoDane")]
    public string CodigoDANE { get; set; }

    [XmlAttribute("Direccion")]
    public string Direccion { get; set; }

    [XmlAttribute("NombrePais")]
    public string NombrePais { get; set; }

    [XmlAttribute("CodigoPais")]
    public string CodigoPais { get; set; }
}

public class TributoClienteDTO
{
    [XmlAttribute("CodigoTributo")]
    public string CodigoTributo { get; set; }

    [XmlAttribute("NombreTributo")]
    public string NombreTributo { get; set; }
}

public class ClienteAdicionalDTO
{

    [XmlAttribute("RazonSocial")]
    public string RazonSocial { get; set; }

    [XmlAttribute("NumeroIdentificacion")]
    public string NumeroIdentificacion { get; set; }

    [XmlAttribute("DV")]
    public string DV { get; set; }

    [XmlAttribute("TipoIdentificacion")]
    public string TipoIdentificacion { get; set; }

    [XmlAttribute("NumeroMatriculaMercantil")]
    public string NumeroMatriculaMercantil { get; set; }

    [XmlAttribute("Porcentaje")]
    public string Porcentaje { get; set; }
}




