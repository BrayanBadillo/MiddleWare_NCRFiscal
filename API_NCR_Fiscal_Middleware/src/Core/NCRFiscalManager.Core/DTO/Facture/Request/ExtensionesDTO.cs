using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class ExtensionesDTO
{
    [XmlElement("POS")]
    public POSDTO POS { get; set; }
   
    //public InteroperabilidadDTO Interoperabilidad { get; set; }
}

public class POSDTO
{
    [XmlElement("InformacionBeneficiosComprador")]
    public InformacionBeneficiosCompradorDTO InformacionBeneficiosComprador { get; set; }

    [XmlElement("InformacionCajaVenta")]
    public InformacionCajaVentaDTO InformacionCajaVenta { get; set; }
}

public class InformacionBeneficiosCompradorDTO
{
    [XmlAttribute("Cabecera")]
    public string Codigo { get; set; }

    [XmlAttribute("NombresApellidos")]
    public string NombresApellidos { get; set; }

    [XmlAttribute("Puntos")]
    public string Puntos { get; set; }
}

public class InformacionCajaVentaDTO
{
    [XmlAttribute("PlacaCaja")]
    public string PlacaCaja { get; set; }

    [XmlAttribute("UbicacionCaja")]
    public string UbicacionCaja { get; set; }

    [XmlAttribute("Cajero")]
    public string Cajero { get; set; }

    [XmlAttribute("TipoCaja")]
    public string TipoCaja { get; set; }

    [XmlAttribute("CodigoVenta")]
    public string CodigoVenta { get; set; }

    [XmlAttribute("Subtotal")]
    public string Subtotal { get; set; }
}

public class InteroperabilidadDTO
{
    public GrupoDTO Grupo { get; set; }


    public string URLAdjunto { get; set; }
}

public class GrupoDTO
{
    public CategoriaDTO Categoria { get; set; }


    public string Nombre { get; set; }
}

public class CategoriaDTO
{
    public List<CategoriaCampoAdicionalDTO> CampoAdicional { get; set; }


    public string Nombre { get; set; }
}

public partial class CategoriaCampoAdicionalDTO
{

    public string Nombre { get; set; }


    public string Valor { get; set; }


    public string NombreEsquema { get; set; }


    public string IDEsquema { get; set; }

}
















