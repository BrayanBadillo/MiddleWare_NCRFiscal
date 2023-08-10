
namespace NCRFiscalManager.Core.Constants
{
    public class Constant
    {
        /// <summary>
        /// Mensaje de error en el log al consumir un servicio.
        /// </summary>
        public const string ErrorMessageServiceLog = "Se ha producido un error";

        /// <summary>
        /// Mensaje exitoso en el log al consoumir un servicio.
        /// </summary>
        public const string SuccessMessageServiceLog = "Respuesta recibida";

        /// <summary>
        /// Content-Type del cliente al consumir un servicio.
        /// </summary>
        public const string ContentTypeHeader = "Content-Type";

        /// <summary>
        /// Formato del content type enviado en el header al consumir un servicio.
        /// </summary>
        public const string ApplicationJsonHeader = "application/json;charset=utf-8";

        /// <summary>
        /// Key para decodificar datos sensibles
        /// </summary>
        public const string Key = "";

        //public const string PosSettings = "PosSettings";

        public const string Unidad = "UNIDAD";
        public const string MonedaCop = "COP";
        public const string PersonaJuridica = "1";
        public const string RegimenJuridico = "48";
        public const string NoResponsableFiscal = "R-99-PN";
        public const string CufeNoGenerado = "NO_GENERADO";

        // Forma de pago de contado.
        public const string PayMethod = "10";

        public const int ProductionEnvironment = 1;
        public const int TestEnvironment = 2;

        // Unidad de medida 94 de acuerdo a catálogos
        public const string UnitOfMeasurement = "94";

        // Código de método del pago de 'Contado' de Facture.
        public const int PaymentMethodFacture = 1;

        // Formato de redondeo para los valores de Facture
        public const string DefaultRound = "0.00";

        // Nombre del método de pago con el que se hacen los anticipos en Chuck E' Cheese.
        public const string AdvancePaymentMethodName = "Anticipo";

        public const string EstandarCodificacionArticulo = "999";

        public enum ProveedorTecnologico
        {
            TECNOFACTOR = 1,
            GOSOCKET = 2,
            FACTURE = 3
        }

        // Parametrización de acuerdo al catálogo de servicios de GoSocket, Facture para obtener el código de tipo de identificación
        public static Dictionary<string, string> GetKindDocument()
        {
            return new Dictionary<string, string>()
            {
                { "CEDULA_CIUDADANIA", "13" },
                { "CEDULA_EXTRANJERIA", "22" },
                { "NIT", "31" },
                { "PASAPORTE", "41" },
                { "TARJETA_EXTRANJERIA", "21" },
                { "NIUP", "91" },
                { "NIT_OTRO_PAIS", "50" }
            };
        }
        public static Dictionary<string, string> GetTaxType()
        {
            return new Dictionary<string, string>
            {
                {"IVA","01"},
                {"IMPOCONSUMO","04"},
                {"NO", "ZY"},
                {"IMP. INCLUIDO","04"},
                {"IVA 19%","01"},
                {"IVA 16%","01"},
                {"IVA 19","01"},
                {"I-CONSUMO","04"},
                {"INC","04"},
                {"ICO 8%","04"},
                {"ICO 5%","04"},
                {"Sin impuesto","ZY"},
                {"No Tax","ZY"},
                {"IVA 5%", "01"},
                {"Sin Imp","ZY"}
            };
        }
        public static Dictionary<float, float> GetTaxValue()
        {
            return new Dictionary<float, float>()
            {
                { 0.19f, 19.00f },
                { 0.05f, 5.00f },
                { 0.08f, 8.00f },
                { 0.16f, 16.00f },
                { 0f, 0f }
            };
        }

        public static Dictionary<string, string> GetKindPerson()
        {
            return new Dictionary<string, string>()
            {
                {"JURIDICA", "1"},
                {"NATURAL", "2"}
            };
        }
        public static Dictionary<string, string> GetTaxResponsability()
        {
            return new Dictionary<string, string>()
            {
                {"NO_RESPONSABLE_IVA", "49"},
                {"RESPONSABLE_IVA", "48"}
            };
        }


        public static List<string> GetExcludeItemTax()
        {
            return new List<string>()
            {
                {"998"}, // <VlrCodigo>998</VlrCodigo>
            };
        }

    }
}
