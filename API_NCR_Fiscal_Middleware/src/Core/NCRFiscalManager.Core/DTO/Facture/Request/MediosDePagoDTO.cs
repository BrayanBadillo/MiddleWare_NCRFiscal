using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class MediosDePagoDTO
{
    [XmlAttribute("CodigoMedioPago")]
    public string CodigoMedioPago { get; set; }

    [XmlAttribute("FormaDePago")]
    public int FormaDePago { get; set; }

    [XmlAttribute("Identificador")]
    public string Identificador { get; set; }
}

public class ReferenciaDePagoDTO
{
    public string ReferenciaDePago { get; set; }
}







