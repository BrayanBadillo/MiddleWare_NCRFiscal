using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCRFiscalManager.Core.DTO;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Interfaces;
using System.Net;

namespace NCRFiscalManager.API.Controllers;

public class BlackListedItemsController : ApiController
{
    private readonly IBlackListedItemsService _blackListedItemsService;

    public BlackListedItemsController(IBlackListedItemsService blackListedItemsService)
    {
        _blackListedItemsService = blackListedItemsService ?? throw new ArgumentNullException(nameof(blackListedItemsService));
    }

    [HttpGet("{emitterInvoiceIdentification}")]
    public ActionResult<IEnumerable<BlackListItemsDTO>> GetBlackListedItemsByEmitterNumberIdentification(string emitterInvoiceIdentification)
    {
        return Ok(_blackListedItemsService.GetBlacklistedItemsByEmmiterInvoiceIdentification(emitterInvoiceIdentification));
    }

    [HttpGet("AllBlackListedItems")]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> GetAllBlackListedItems()
    {
        try
        {
            var blackListItems = await _blackListedItemsService.GetAllAsync();
            if (blackListItems == null) return NotFound(ResponseDTO<IEnumerable<BlackListItemsDTO>>.NotFound("No se han encontrado items en la lista negra."));
            var response = ResponseDTO<IEnumerable<BlackListItemsDTO>>.Success("¡Consulta realizada con éxito!", blackListItems);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<BlackListItemsDTO>.BadRequest("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> GetByBlackListItembById(long id)
    {
        try
        {
            var blackListedItem = await _blackListedItemsService.GetByIdAsync(id);
            if (blackListedItem == null) return NotFound(ResponseDTO<BlackListItemsDTO>.NotFound($"El item con id {id} no existe."));

            var response = ResponseDTO<BlackListItemsDTO>.Success("¡Consulta realizada con éxito!", blackListedItem);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<BlackListItemsDTO>.BadRequest("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }

    [HttpGet("GetBlackListedItemByAlohaCode")]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> GetByBlackListItembByAlohaItemCode(string emiterInvoiceIdentification, string alohaItemCode)
    {
        try
        {
            var blackListedItem = await _blackListedItemsService.GetBlackListedItemByEmitterInvoiceIdentificationAndAlohaItemCode(emiterInvoiceIdentification, alohaItemCode);
            if (blackListedItem == null) return NotFound(ResponseDTO<BlackListItemsDTO>.NotFound($"El item con el código de aloha {alohaItemCode} no existe en la lista negra del emisor indicado."));

            var response = ResponseDTO<BlackListItemsDTO>.Success("¡Consulta realizada con éxito!", blackListedItem);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<BlackListItemsDTO>.BadRequest("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> CreateStore([FromBody] UpsertBlackListItemDTO blackListedItemdDTO)
    {
        try
        {
            if (!ModelState.IsValid) return ResponseDTO<BlackListItemsDTO>.BadRequest("Se ha producido un error al crear el item, revise la data que está enviendo.");
            var newBlackListedItem = await _blackListedItemsService.CreateAsync(blackListedItemdDTO);
            var response = ResponseDTO<BlackListItemsDTO>.Success("¡Item añadido a la lista negra con éxito!", newBlackListedItem, (int)HttpStatusCode.Created);

            return StatusCode(response.StatusCode, response);

        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ResponseDTO<BlackListItemsDTO>.InternalServerError("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> UpdateStore(int id, [FromBody] UpsertBlackListItemDTO blackListDTO)
    {
        try
        {
            if (!ModelState.IsValid) return ResponseDTO<BlackListItemsDTO>.BadRequest("Se ha producido un error al editar el item, revise la data que está enviendo.");
            var updatedBlackListItem = await _blackListedItemsService.UpdateAsync(id, blackListDTO);
            var response = ResponseDTO<BlackListItemsDTO>.Success("¡Tienda editada con éxito!", updatedBlackListItem);

            return StatusCode(response.StatusCode, response);

        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ResponseDTO<BlackListItemsDTO>.InternalServerError("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResponseDTO<BlackListItemsDTO>>> DeleteStore(int id)
    {
        try
        {
            var storeToDelete = await _blackListedItemsService.GetByIdAsync(id);
            if (storeToDelete == null) return NotFound(ResponseDTO<BlackListItemsDTO>.NotFound($"El item con id {id} no existe."));
            await _blackListedItemsService.RemoveAsync(id);
            var response = ResponseDTO<BlackListItemsDTO>.Success("¡Item eliminado de la lista negra exitósamente!");

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ResponseDTO<BlackListItemsDTO>.InternalServerError("Ha ocurrido un error en la solicitud", ex.Message));
        }
    }


}
