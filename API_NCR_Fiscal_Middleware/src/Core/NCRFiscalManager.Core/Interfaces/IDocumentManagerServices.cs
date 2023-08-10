using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Interfaces.Services
{
    public interface IDocumentManagerServices
    {
        /// <summary>
        /// Servicio encargado de recibir un documento, procesarlo y enviarlo al tercero (Tecnofactor).
        /// </summary>
        /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <param name="idOperadorTecnologico">Indica el operador tecnológico</param>
        /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
        public ResponseDTO ProcessDocument(DocumentDTO document, int idOperadorTecnologico);

        public ResponseDTO ProcessResendDocumentFacture( Factura factura, string storeKey );
    }
}
