using System.Xml.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;

public class LineaDTO
{
    [XmlElement("Detalle")]
    public DetalleDTO Detalle { get; set; }

    [XmlElement ("Impuestos")]
    public ImpuestosLineaDTO Impuestos { get; set; }

    [XmlElement("DescuentosOCargos")]
    public DescuentosOCargosDTO DescuentosOCargos { get;  set; }
    
    [XmlElement ("CodificacionesEstandar")]
    public CodificacionesEstandarDTO CodificacionesEstandar { get; set; }
    
    [XmlElement ("PrecioReferencia")]
    public PrecioReferenciaDTO PrecioReferencia { get; set; }
}

public class DetalleDTO
{
    [XmlAttribute("NumeroLinea")]
    public int NumeroLinea { get; set; }

    [XmlAttribute("Nota")]
    public string Nota { get; set; }

    [XmlAttribute("Cantidad")]
    public float Cantidad { get; set; }

    [XmlAttribute("UnidadMedida")]
    public string UnidadMedida { get; set; }

    [XmlAttribute("SubTotalLinea")]
    public float SubTotalLinea { get; set; }

    [XmlAttribute("Descripcion")]
    public string Descripcion { get; set; }

    [XmlAttribute("CantidadBase")]
    public float CantidadBase { get; set; }

    [XmlAttribute("UnidadCantidadBase")]
    public string UnidadCantidadBase { get; set; }

    [XmlAttribute("PrecioUnitario")]
    public float PrecioUnitario { get; set; }

    [XmlAttribute("ValorTotalItem")]
    public float ValorTotalItem { get; set; }

    [XmlAttribute("IdentificadorUnico")]
    public string IdentificadorUnico { get; set; }
}

public class PrecioReferenciaDTO
{
    [XmlAttribute("ValorArticulo")]
    public string ValorArticulo { get; set; }

    [XmlAttribute("CodigoTipoPrecio")]
    public string CodigoTipoPrecio { get; set; }

    [XmlAttribute("MonedaPrecioReferencia")]
    public string MonedaPrecioReferencia { get; set; }
}


[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class RetencionDTO
{
    public RetencionSubtotalDTO Subtotal { get; set; }

    public decimal Valor { get; set; }

    public byte Tipo { get; set; }

    public string Nombre { get; set; }

}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class RetencionSubtotalDTO
{
    public int ValorBase { get; set; }

    public decimal Valor { get; set; }

    public decimal Porcentaje { get; set; }
}


[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class CodificacionVendedorDTO
{
    public string CodigoArticulo { get; set; }

    public string CodigoExtendido { get; set; }
}

public class CodificacionesEstandarDTO
{
    public CodificacionEstandarDTO CodificacionEstandar { get; set; }
}

public class CodificacionEstandarDTO
{
    [XmlAttribute("CodigoArticulo")]
    public string CodigoArticulo { get; set; }

    [XmlAttribute("CodigoEstandar")]
    public string CodigoEstandar { get; set; }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class CodificacionCompradorDTO
{
    public string ID { get; set; }

    public string Codigo { get; set; }
}


[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class CampoAdicionalDTO
{
    public string Nombre { get; set; }

    public string Valor { get; set; }

    public int Cantidad { get; set; }

    public string Unidad { get; set; }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class MandanteDTO
{
    public int Nit { get; set; }

    public int DV { get; set; }

    public int TipoIdentificacion { get; set; }

}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class InformacionAdicionalDTO
{
    public InformaciónAdicionalDocumentoReferenciaDTO DocumentoReferencia { get; set; }
}


[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class InformaciónAdicionalDocumentoReferenciaDTO
{

    public int Codigo { get; set; }

    public int TipoDocumento { get; set; }

    public DateTime Fecha { get; set; }

    public string UrlAdjunto { get; set; }

    public string CUFEDocumento { get; set; }

}

//[XmlRoot("Impuestos")]
public class ImpuestosLineaDTO
{
    public ImpuestoLineaDTO Impuesto { get; set; }
}

public class ImpuestoLineaDTO
{
    public SubtotalLineaDTO Subtotal { get; set; }

    [XmlAttribute("Valor")]
    public string Valor { get; set; }

    [XmlAttribute("Tipo")]
    public string Tipo { get; set; }

    [XmlAttribute("Nombre")]
    public string Nombre { get; set; }

    [XmlAttribute("Redondeo")]
    public float Redondeo { get; set; }
}

public class SubtotalLineaDTO
{
    [XmlAttribute("ValorBase")]
    public string ValorBase { get; set; }

    [XmlAttribute("Valor")]
    public string Valor { get; set; }

    [XmlAttribute("CodigoUnidadMedidaBase")]
    public string CodigoUnidMedidaBase { get; set; }

    [XmlAttribute("Porcentaje")]
    public string Porcentaje { get; set; }
}













