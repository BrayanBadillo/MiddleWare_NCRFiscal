using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class NumeracionDianDTO
{
    [XmlAttribute("NumeroResolucion")]
    public long NumeroResolucion { get; set; }

    [XmlAttribute("FechaInicio")]
    public string FechaInicio { get; set; }

    [XmlAttribute("FechaFin")]
    public string FechaFin { get; set; }

    [XmlAttribute("PrefijoNumeracion")]
    public string PrefijoNumeracion { get; set; }

    [XmlAttribute("ConsecutivoInicial")]
    public string ConsecutivoInicial { get; set; }

    [XmlAttribute("ConsecutivoFinal")]
    public string ConsecutivoFinal { get; set; }
}


