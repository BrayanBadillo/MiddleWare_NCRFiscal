using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Interfaces.Services;
using Newtonsoft.Json;

namespace NCRFiscalManager.API.Controllers
{
    public class DocumentController : ApiController
    {
        private readonly IDocumentManagerServices _documentManagerServices;

        public DocumentController(IDocumentManagerServices documentManagerServices)
        {
            _documentManagerServices = documentManagerServices;
        }

        /// <summary>
        /// Servicio para recibir los datos de AFM Fiscal
        /// </summary>
        /// <param name="document">Objeto DocumentDTO con los datos enviados por AFM Fiscal</param>
        /// <returns>Retorna estado de la generación del servicio</returns>
        [HttpPost("{idOperadorTecnologico}")]
        public IActionResult Post([BindRequired] int idOperadorTecnologico, [FromBody] DocumentDTO document)
        {
            return Ok(_documentManagerServices.ProcessDocument(document, idOperadorTecnologico));
        }


        [HttpPost("/resendinvoice/{storeKey}")]
        public IActionResult ResendFactureInovice( [BindRequired] string storeKey, [FromBody] Factura factura )
        {
            //Deserializar el string al objeto Factura
            //var factura = JsonConvert.DeserializeObject<Factura>(request);

            return Ok(_documentManagerServices.ProcessResendDocumentFacture(factura, storeKey));
        }


    }
}