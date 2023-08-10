using NCRFiscalManager.Core.DTO.Entities;

namespace NCRFiscalManager.Core.Interfaces;

public interface IBlackListedItemsService
{
    IEnumerable<BlackListItemsDTO> GetBlacklistedItemsByEmmiterInvoiceIdentification(string emitterInvoiceIdentification);
    IEnumerable<string> GetBlackListedItemsCodesByEmitterInvoice(string emitterInvoiceIdentification);
    Task<BlackListItemsDTO> GetBlackListedItemByEmitterInvoiceIdentificationAndAlohaItemCode(string emitterInvoiceIdentification, string alohaItemCode);
    Task<IEnumerable<BlackListItemsDTO>> GetAllAsync();
    Task<BlackListItemsDTO> GetByIdAsync(long id);
    Task<BlackListItemsDTO> CreateAsync(UpsertBlackListItemDTO blackListedItem);
    Task<BlackListItemsDTO> UpdateAsync(long id, UpsertBlackListItemDTO blackListedItem);
    Task RemoveAsync(long id);
}
