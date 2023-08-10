using NCRFiscalManager.Core.DTO.TecnoFactor;

namespace NCRFiscalManager.Core.Interfaces
{
    public interface ISendDocumentTecnoFactorService
    {
        /// <summary>
        /// Servicio de autenticación para TecnoFactor.
        /// </summary>
        /// <returns>Retorna objeto ResponseTokenDTO con los datos del token</returns>
        public ResponseTokenDTO GenerateAuthenticationToken();
        
        /// <summary>
        /// Servicio para generar la factura electrónica en TecnoFactor.
        /// </summary>
        /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <param name="authToken">String con el token autorizado para consumir el servicio</param>
        /// <returns>Retorna objeto ResponseDocumentDTO con el resultado de la transacción</returns>
        public ResponseDocumentDTO SendDocumentToTecnofactor(RequestDocumentDTO document, string authToken);

        /// <summary>
        /// Servicio para consultar el estado del documento en TecnoFactor.
        /// </summary>
        /// <param name="requestConsultarDocumento">Objeto de tipo RequestConsultarDocumentoDTO con los datos a consultar</param>
        /// <param name="authToken">String con el token autorizado para consumir el servicio</param>
        /// <returns>Retorna objeto ResponseConsultarDocumentoDTO con los datos de respuesta del servicio</returns>
        public ResponseConsultarDocumentoDTO ConsultDocumentTecnofactor(RequestConsultarDocumentoDTO requestConsultarDocumento,
            string authToken);
    }
}
