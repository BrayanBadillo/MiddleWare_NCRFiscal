using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class DescuentosOCargosDTO
{
    [XmlElement("DescuentoOCargo")]
    public List<DescuentoOCargoDTO> DescuentoOCargo { get; set; }
}

public class DescuentoOCargoDTO
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("Indicador")]
    public bool Indicador { get; set; }

    [XmlAttribute("CodigoDescuento")]
    public int CodigoDescuento { get; set; }

    [XmlAttribute("Justificacion")]
    public string Justificacion { get; set; }

    [XmlAttribute("Porcentaje")]
    public string Porcentaje { get; set; }

    [XmlAttribute("Valor")]
    public string Valor { get; set; }

    [XmlAttribute("ValorBase")]
    public string ValorBase { get; set; }
}







