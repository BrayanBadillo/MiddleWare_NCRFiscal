using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.TecnoFactor;

namespace NCRFiscalManager.Core.Interfaces;

public interface IMapperTecnoFactorService
{
    /// <summary>
    /// Servicio para mapear los datos entre AFM Fiscal y TecnoFactor.
    /// </summary>
    /// <param name="documentDto">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
    /// <returns>Retorna objeto RequestDocumentDTO con la estructura a enviar al Operador Tecnológico</returns>
    public RequestDocumentDTO GetTecnoFactorClientDTO(DocumentDTO documentDto);
}