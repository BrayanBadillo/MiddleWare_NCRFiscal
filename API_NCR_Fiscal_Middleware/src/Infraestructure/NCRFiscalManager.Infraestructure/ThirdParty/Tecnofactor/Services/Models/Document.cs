using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRFiscalManager.Infraestructure.ThirdParty.Tecnofactor.Services.Models
{
    public class Document
    {
        public Pago pago { get; set; }
        public string moneda { get; set; }
        public Detalle[] detalles { get; set; }
        public string emisorId { get; set; }
        public string dsPrefijo { get; set; }
        public string fechaEmision { get; set; }
        public string primerApellido { get; set; }
        public string dsNumeroFactura { get; set; }
        public string dsObservaciones { get; set; }
        public string dsResolucionDian { get; set; }
        public string emailAdquiriente { get; set; }
        public Ciudadadquiriente ciudadAdquiriente { get; set; }
        public string regimenAdquirente { get; set; }
        public Facturasreferencia[] facturasReferencia { get; set; }
        public string nombresAdquiriente { get; set; }
        public string telefonoAdquiriente { get; set; }
        public string direccionAdquiriente { get; set; }
        public bool adquirenteResponsable { get; set; }
        public string tipoPersonaAdquiriente { get; set; }
        public string snConsecutivoAutomatico { get; set; }
        public string tipoDocumentoElectronico { get; set; }
        public string identificacionAdquiriente { get; set; }
        public string responsabilidadesFiscales { get; set; }
        public string tipoIdentificacionAdquiriente { get; set; }
    }

    public class Pago
    {
        public string formaPago { get; set; }
        public string medioPago { get; set; }
    }

    public class Ciudadadquiriente
    {
        public string cdDane { get; set; }
    }

    public class Detalle
    {
        public float cantidad { get; set; }
        public bool esSinIva { get; set; }
        public Obsequio obsequio { get; set; }
        public string unidadMedida { get; set; }
        public string codigoArticulo { get; set; }
        public float precioUnitario { get; set; }
        public object[] cargosDescuentos { get; set; }
        public string descripcionArticulo { get; set; }
        public Impuestosretencione[] impuestosRetenciones { get; set; }
    }

    public class Obsequio
    {
        public int precioReferencia { get; set; }
        public string tipoPrecioReferencia { get; set; }
    }

    public class Impuestosretencione
    {
        public string tributo { get; set; }
        public float porcentaje { get; set; }
    }

    public class Facturasreferencia
    {
        public string numero { get; set; }
        public string prefijo { get; set; }
        public string conceptoNotaCredito { get; set; }
    }

}
