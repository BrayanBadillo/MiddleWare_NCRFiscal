using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class TotalesDTO
{
    [XmlAttribute("Bruto")]
    public string Bruto { get; set; }

    [XmlAttribute("BaseImponible")]
    public string BaseImponible { get; set; }

    [XmlAttribute("BrutoMasImpuestos")]
    public string BrutoMasImpuestos { get; set; }

    [XmlAttribute("Cargos")]
    public string Cargos { get; set; }

    [XmlAttribute("Descuentos")]
    public string Descuentos { get; set; }

    [XmlAttribute("Impuestos")]
    public string Impuestos { get; set; }

    [XmlAttribute("Retenciones")]
    public float Retenciones { get; set; }

    [XmlAttribute("General")]
    public string General { get; set; }

    [XmlAttribute("Anticipo")]
    public string Anticipo { get; set; }

    [XmlAttribute("Redondeo")]
    public decimal Redondeo { get; set; }

    [XmlAttribute("TotalDescuentosLineas")]
    public string TotalDescuentosLineas { get; set; }

    [XmlAttribute("TotalCargosLineas")]
    public int TotalCargosLineas { get; set; }

    [XmlAttribute("TotalOtros1")]
    public int TotalOtros1 { get; set; }
    
    [XmlAttribute("TotalOtros2")]
    public int TotalOtros2 { get; set; }

    [XmlAttribute("TotalReteFuente")]
    public int TotalReteFuente { get; set; }

    [XmlAttribute("TotalReteIva")]
    public int TotalReteIva { get; set; }
   
    [XmlAttribute("TotalReteIca")]
    public int TotalReteIca { get; set; }
}














