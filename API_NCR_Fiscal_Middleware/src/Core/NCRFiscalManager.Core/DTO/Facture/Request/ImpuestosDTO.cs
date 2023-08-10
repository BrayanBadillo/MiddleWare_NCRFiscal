using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class ImpuestosDTO
{
    [XmlElement("Impuesto")]
    public List<ImpuestoDTO> Impuesto { get; set; }
}


public class ImpuestoDTO
{
    [XmlElement("Subtotal")]
    public SubtotalDTO Subtotal { get; set; }

    [XmlAttribute("Valor")]
    public string Valor { get; set; }

    [XmlAttribute("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute("Nombre")]
    public string Nombre { get; set; }

    [XmlAttribute("Redondeo")]
    public string Redondeo { get; set; }
}

public class SubtotalDTO
{
    [XmlAttribute("ValorBase")]
    public string ValorBase { get; set; }

    [XmlAttribute("Valor")]
    public string Valor { get; set; }

    [XmlAttribute("CodigoUnidadBase")]
    public string CodigoUnidMedidaBase { get; set; }

    [XmlAttribute("ValorTributoXUnidad")]
    public float ValorTributoXUnidad { get; set; }

    [XmlAttribute("Porcentaje")]
    public string Porcentaje { get; set; }

    [XmlAttribute("ValorUnidadMedidaBase")]
    public string ValorUnidadMedidaBase { get; set; }

}











