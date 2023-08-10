using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NCRFiscalManager.Core.DTO.Facture.Request
{
    public  class ConnectionDataFactureDTO
    {
        /*Facture*/
        [JsonPropertyName("Tenant")]
        public string Tenant { get; set; }

        [JsonPropertyName("XWho")]
        public string Xwho { get; set; }

        [JsonPropertyName("XKeyControl")]
        public string XKeyControl { get; set; }

        [JsonPropertyName("DocumentTypeInvoice")]
        public string DocumentTypeInvoice { get; set; }

        [JsonPropertyName("DocumentTypeCreditNote")]
        public string DocumentTypeCreditNote { get; set; }
 
    }
}
