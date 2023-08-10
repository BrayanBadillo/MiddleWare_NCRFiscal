using NCRFiscalManager.Core.DTO.NCR;
namespace NCRFiscalManager.Core.Interfaces;

public interface IDocumentManagerTecnoFactorService
{
    /// <summary>
    /// Método para reportar el documento al operador tecnológico TecnoFactor.
    /// </summary>
    /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
    /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
    public ResponseDTO ProcessDocumentTecnofactor(DocumentDTO document);
}