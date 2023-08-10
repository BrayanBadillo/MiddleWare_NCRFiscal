using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;

namespace NCRFiscalManager.Core.Services;

public class EmitterInvoiceService : IEmitterInvoiceService
{
    private readonly IEmitterInVoiceRepository _emitterInVoiceRepository;
    private readonly IMapper _mapper;

    public EmitterInvoiceService(IEmitterInVoiceRepository emitterInVoiceRepository, IMapper mapper)
    {
        _emitterInVoiceRepository = emitterInVoiceRepository ?? throw new ArgumentNullException(nameof(emitterInVoiceRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    /// <summary>
    /// Obtener la confguración de precios de item zero del emisor.
    /// </summary>
    /// <param name="identificationNumber">Identificación del emisor (NIT).</param>
    /// <returns>Verdadero o falso, dependiendo de la configuración.</returns>
    public EmitterInvoiceDTO GetZeroPriceItemConfigurationOfEmitterInvoice(string identificationNumber)
    {
        var emitterInvoiceConfigurations = _emitterInVoiceRepository.GetEmitterInvoiceByIdentificationNumber(identificationNumber);


        return _mapper.Map<EmitterInvoiceDTO>(emitterInvoiceConfigurations);
    }
}
