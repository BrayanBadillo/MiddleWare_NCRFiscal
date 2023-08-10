using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.GoSocket;


public class DTE
{
    public DocumentDteDTO Documento { get; set; }

    [XmlElement("Personalizados")]
    public Personalizados Personalizados { get; set; }

    [XmlAttributeAttribute()]
    public string ID { get; set; }
}

[Serializable]
public class DocumentDteDTO
{
    [XmlElement("Encabezado")]
    public EncabezadoDTO Encabezado { get; set; }

    [XmlElement("DscRcgGlobal")]
    public RecargosGlobalesDto RecargosGlobalesDto { get; set; }

    [XmlElement("Detalle")]
    public List<DetalleGoSocketDTO> Detalle { get; set; }

    [XmlElement("Referencia")]
    public List<ReferenciaDTO> Referencia { get; set; }

    [XmlElement("CAE")]
    public CaeDTO CAE { get; set; }
     
}


[Serializable]
public class Personalizados
{
    [XmlElement("DocPersonalizado")]
    public DocPersonalizado DocPersonalizado { get; set; }
}


[Serializable]
public class DocPersonalizado
{
    [XmlElement("campoString")]
    public string campoString { get; set; }
}


[Serializable]
public class EncabezadoDTO
{
    [XmlElement("IdDoc")]
    public IdDocDTO IdDoc { get; set; }

    [XmlElement("Emisor")]
    public EmisorDTO Emisor { get; set; }

    [XmlElement("Receptor")]
    public ReceptorDTO Receptor { get; set; }

    [XmlElement("Totales")]
    public TotalesDTO Totales { get; set; }

    [XmlElement("Impuestos")]
    public List<ImpuestosDTO> Impuestos { get; set; }

}


public class RecargosGlobalesDto
{
    public string TpoMov { get; set; }
    public string GlosaDR { get; set; }

    public string PctDR { get; set; }
    public decimal ValorDR { get; set; }

}

public class IdDocDTO
{
    public string Ambiente { get; set; }
    public string TipoServicio { get; set; }
    public string Tipo { get; set; }
    public string Serie { get; set; }
    public long Numero { get; set; }
    public string NumeroInterno { get; set; }
    public DateTime FechaEmis { get; set; }
    public string Establecimiento { get; set; }
    public string PtoEmis { get; set; }
    //public DateTime FechaVenc { get; set; }
    public string IndServicio { get; set; }
    public string MedioPago { get; set; }
    public string IDPago { get; set; }
    //public DateTime FechaCancel { get; set; }
    //public DateTime PeriodoDesde { get; set; }
    //public DateTime PeriodoHasta { get; set; }
    public string TermPagoCdg { get; set; }
    public string TipoNegociacion { get; set; }
    public string Plazo { get; set; }
}

public class EmisorDTO
{
    public string TipoContribuyente { get; set; }
    public string RegimenContable { get; set; }
    public string IDEmisor { get; set; }
    public string NmbEmisor { get; set; }
    public NombreEmisorDTO NombreEmisor { get; set; }
    public CodigoEmisorDTO CodigoEmisor { get; set; }
    public string CdgSucursal { get; set; }
    public PlaceDTO DomFiscal { get; set; }
    public PlaceDTO LugarExped { get; set; }
    public ContactoEmisorDTO ContactoEmisor { get; set; }
}

public class ReceptorDTO
{
    public string RegimenContableR { get; set; }
    public string TipoContribuyenteR { get; set; }
    public string CdgSucursal { get; set; }
    public string CdgGLNRecep { get; set; }
    public DocRecepDTO DocRecep { get; set; }
    public string NmbRecep { get; set; }
    public NombreEmisorDTO NombreRecep { get; set; }
    public CodigoReceptorDTO CodigoReceptor { get; set; }
    public PlaceDTO DomFiscalRcp { get; set; }
    public PlaceDTO LugarRecep { get; set; }
    public ContactoEmisorDTO ContactoReceptor { get; set; }
}
public class NombreEmisorDTO
{
    public string PrimerNombre { get; set; }
}
public class CodigoEmisorDTO
{
    public string TpoCdgIntEmisor { get; set; }
    public string CdgIntEmisor { get; set; }
}
public class PlaceDTO
{
    public string Calle { get; set; }
    public string Departamento { get; set; }
    public string Ciudad { get; set; }
    public string Pais { get; set; }
    public string CodigoPostal { get; set; }
}
public class ContactoEmisorDTO
{
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string eMail { get; set; }
    public string Telefono { get; set; }
    public string Fax { get; set; }
}

public class DocRecepDTO
{
    public string TipoDocRecep { get; set; }
    public string NroDocRecep { get; set; }
}
public class CodigoReceptorDTO
{
    public string TpoCdgIntRecep { get; set; }
    public string CdgIntRecep { get; set; }
}

public class TotalesDTO
{
    public string Moneda { get; set; }
    //public float FctConv { get; set; }
    public decimal SubTotal { get; set; }
    public decimal MntBase { get; set; }
    public decimal MntImp { get; set; }
    public decimal VlrPagar { get; set; }
    public decimal MntRcgo { get; set; }
}

public class ImpuestosDTO
{
    public string TipoImp { get; set; }
    public decimal TasaImp { get; set; }
    public decimal MontoBAseImp { get; set; }
    public decimal MontoImp { get; set; }
}

public class ImpuestosDetalleDTO
{
    public string TipoImp { get; set; }
    public decimal TasaImp { get; set; }
    public decimal MontoBaseImp { get; set; }
    public decimal MontoImp { get; set; }
    public string? DescripcioTax { get; set; }
}

public class DetalleGoSocketDTO
{
    public int NroLinDet { get; set; }
    public CdgItemDTO CdgItem { get; set; }
    public string DscItem { get; set; }
    public float QtyItem { get; set; }
    public string UnmdItem { get; set; }
    public float PrcBrutoItem { get; set; }
    public float PrcNetoItem { get; set; }
    public SubDsctoDTO SubDscto { get; set; }
    public ImpuestosDetalleDTO ImpuestosDet { get; set; }
    public List<RetencionesDetDTO> RetencionesDet { get; set; }
    public float MontoTotalItem { get; set; }
    //public string NumeroRef { get; set; }
    public LocalItemDTO LocalItem { get; set; }
    public SubMontoDTO SubMonto { get; set; }
}
public class CdgItemDTO
{
    public string TpoCodigo { get; set; }
    public string VlrCodigo { get; set; }
}
public class SubDsctoDTO
{
    public string TipoDscto { get; set; }
    public string GlosaDscto { get; set; }
    public double PctDscto { get; set; }
    public double MntDscto { get; set; }
}
public class RetencionesDetDTO
{
    public string TipoRet { get; set; }
    public float TasaRet { get; set; }
    public float MontoBaseRet { get; set; }
    public float MontoRet { get; set; }
}
public class LocalItemDTO
{
    public string TipoLoc { get; set; }
    public string CodigoLoc { get; set; }
}

public class ReferenciaDTO
{
    public int NroLinRef { get; set; }
    public string TpoDocRef { get; set; }
    public string NumeroRef { get; set; }
    public string SerieRef { get; set; }
    public string FechaRef { get; set; }
    public int CodRef { get; set; }
    public string RazonRef { get; set; }

    [XmlElement("ECB01")]
    public string Ecb01 { get; set; }
}

public class SubMontoDTO
{
    public string Tipo { get; set; }
    public string CodTipoMonto { get; set; }
    public float MontoConcepto { get; set; }
}

public class CaeDTO
{
    public int Tipo { get; set; }
    public string Serie { get; set; }
    public long NumeroInicial { get; set; }
    public long NumeroFinal { get; set; }
    public long NroResolucion { get; set; }
    public string FechaResolucion { get; set; }
    public string ClaveTC { get; set; }
    public string Plazo { get; set; }
}