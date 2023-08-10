using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class ReferenciasNotasDTO
{
    [XmlElement("ReferenciaNota")]
    public ReferenciaNotaDTO ReferenciaNota { get; set; }
}

public class ReferenciaNotaDTO
{
    [XmlAttribute("FacturaAsociada")]
    public string FacturaAsociada { get; set; }

    [XmlAttribute("CodigoNota")]
    public int CodigoNota { get; set; }

    [XmlAttribute("DescripcionNota")]
    public string DescripcionNota { get; set; }
}
