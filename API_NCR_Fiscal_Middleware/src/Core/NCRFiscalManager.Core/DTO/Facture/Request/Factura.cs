using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

[SerializableAttribute()]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot("Factura", IsNullable = false)]
public class Factura
{
    [XmlElement("Cabecera")]
    public CabeceraDTO Cabecera { get; set; }

    [XmlElement("NumeracionDIAN")]
    public NumeracionDianDTO NumeracionDIAN { get; set; }

    [XmlElement("Notificacion")]
    public NotificacionDTO Notificacion { get; set; }

    [XmlElement("Emisor")]
    public EmisorDTO Emisor { get; set; }

    [XmlElement("Cliente")]
    public ClienteDTO Cliente { get; set; }

    [XmlElement("MediosDePago")]
    public List<MediosDePagoDTO> MediosDePago { get; set; }

    [XmlElement("Anticipos")]
    public AnticiposDTO Anticipos { get; set; }

    [XmlElement("DescuentosOCargos")]
    public DescuentosOCargosDTO DescuentosOCargos { get; set; }

    [XmlElement("Impuestos")]
    public ImpuestosDTO Impuestos { get; set; }

    [XmlElement("Totales")]
    public TotalesDTO Totales { get; set; }

    [XmlElement("Linea")]
    public List<LineaDTO> Linea { get; set; }

    [XmlElement("Extensiones")]
    public ExtensionesDTO Extensiones { get; set; }

    /// <summary>
    /// Método para asignar un número al campo NumeroLinea del objeto DetalleDTO
    /// </summary>
    public void SetLineNumbers()
    {
        if(Linea != null)
        {
            int lineCounter = 1;

            Linea.ForEach(linea => linea.Detalle.NumeroLinea = lineCounter++);
        }
    }
}


















