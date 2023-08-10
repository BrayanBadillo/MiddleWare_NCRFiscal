using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;

namespace NCRFiscalManager.Core.Services;

public class BlackListedItemsService : IBlackListedItemsService
{
    private readonly IBlackListItemsRepository _blackListItemsRepository;
    private readonly IEmitterInVoiceRepository _emitterInVoiceRepository;
    private readonly IMapper _mapper;

    public BlackListedItemsService(IBlackListItemsRepository blackListItemsRepository, IEmitterInVoiceRepository emitterInVoiceRepository, IMapper mapper)
    {
        _blackListItemsRepository = blackListItemsRepository ?? throw new ArgumentNullException(nameof(blackListItemsRepository));
        _emitterInVoiceRepository = emitterInVoiceRepository ?? throw new ArgumentNullException(nameof(emitterInVoiceRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public IEnumerable<BlackListItemsDTO> GetBlacklistedItemsByEmmiterInvoiceIdentification(string emitterInvoiceIdentification)
    {
        var emitterInvoice = _emitterInVoiceRepository.GetEmitterInvoiceByIdentificationNumber(emitterInvoiceIdentification);

        var blackListedItems = _blackListItemsRepository.GetAllBlackListedItemsByEmitterInvoiceId(emitterInvoice.Id);

        return _mapper.Map<List<BlackListItemsDTO>>(blackListedItems);
    }

    public IEnumerable<string> GetBlackListedItemsCodesByEmitterInvoice(string emitterInvoiceIdentification)
    {
        var emitterInvoice = _emitterInVoiceRepository.GetEmitterInvoiceByIdentificationNumber(emitterInvoiceIdentification);
        var blackListedItems = _blackListItemsRepository.GetAllBlackListedItemsByEmitterInvoiceId(emitterInvoice.Id);
        var mappedBackListedItems = _mapper.Map<List<BlackListItemsDTO>>(blackListedItems);

        List<string> blackListedItemsCodes = new();

        foreach (BlackListItemsDTO blackListItems in mappedBackListedItems)
        {
            blackListedItemsCodes.Add(blackListItems.AlohaItemCode);
        }

        return blackListedItemsCodes;
    }

    public async Task<BlackListItemsDTO> GetBlackListedItemByEmitterInvoiceIdentificationAndAlohaItemCode(string emitterInvoiceIdentification, string alohaItemCode)
    {
        var emitterInvoice = _emitterInVoiceRepository.GetEmitterInvoiceByIdentificationNumber(emitterInvoiceIdentification);
        var blackListedItem = await _blackListItemsRepository.GetByFilter(x => x.EmitterInVoiceId == emitterInvoice.Id && x.AlohaItemCode == alohaItemCode);


        return _mapper.Map<BlackListItemsDTO>(blackListedItem.FirstOrDefault());

    }

    public async Task<BlackListItemsDTO> GetByIdAsync(long id)
    {
        var blackListedItem = await _blackListItemsRepository.GetByIdAsync(id);

        return _mapper.Map<BlackListItemsDTO>(blackListedItem);
    }

    public async Task<IEnumerable<BlackListItemsDTO>> GetAllAsync()
    {
        var blackListedItems = await _blackListItemsRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<BlackListItemsDTO>>(blackListedItems);
    }

    public async Task RemoveAsync(long id)
    {
        var blackListedItem = await _blackListItemsRepository.GetByIdAsync(id);
        if(blackListedItem is null) throw new NullReferenceException();

        await _blackListItemsRepository.DeleteAsync(blackListedItem);
    }

    public async Task<BlackListItemsDTO> UpdateAsync(long id, UpsertBlackListItemDTO storeDTO)
    {
        var blackListedItem = await _blackListItemsRepository.GetByIdAsync(id);
        if (blackListedItem is null) throw new NullReferenceException($"La tienda con id {id} no existe.");

        blackListedItem = _mapper.Map(storeDTO, blackListedItem);
        var updatedBlackListItem = await _blackListItemsRepository.UpdateAsync(blackListedItem);

        return _mapper.Map<BlackListItemsDTO>(updatedBlackListItem);
    }

    public async Task<BlackListItemsDTO> CreateAsync(UpsertBlackListItemDTO blackListedItem)
    {
        var blackListedItemMapped = _mapper.Map<BlackListItems>(blackListedItem);
        var newBlackListedItem = await _blackListItemsRepository.CreateAsync(blackListedItemMapped);

        return _mapper.Map<BlackListItemsDTO>(newBlackListedItem);
    }
}
