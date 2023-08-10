using System.Net;

namespace NCRFiscalManager.Core.DTO;

public class ResponseDTO<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
    public string Errors { get; set; }


    /// <summary>
    /// Obtener la respuesta que será enviada por los endpoints.
    /// </summary>
    /// <param name="statusCode">Código http.</param>
    /// <param name="message">Mensaje a enviar en la respuesta.</param>
    /// <param name="data">Información a mostrar en la respuesta.</param>
    /// <param name="errors">Errores a mostrar.</param>
    /// <returns>Un objeto ResponseDTO con la información necesaria a responder dependiendo del request.</returns>
    private static ResponseDTO<T> NewResponse(int statusCode, string message, object data = null, string errors = null)
    {

        return new ResponseDTO<T>
        {
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = errors
        };
    }

    /// <summary>
    /// Sobrecarga del método NewResponse para obtener la respuesta que será enviada por los endpoints.
    /// </summary>
    /// <param name="statusCode">Código http.</param>
    /// <param name="message">Mensaje a enviar en la respuesta.</param>
    /// <param name="data">Información a mostrar en la respuesta.</param>
    /// <param name="errors">Errores a mostrar.</param>
    /// <returns>Un objeto ResponseDTO con la información necesaria a responder dependiendo del request.</returns>
    private static ResponseDTO<T> NewResponse(int statusCode, string message, IEnumerable<object> data = null, string errors = null)
    {

        return new ResponseDTO<T>
        {
            StatusCode = statusCode,
            Message = message,
            Data = data == null ? null : data.ToList(),
            Errors = errors
        };
    }

    /// <summary>
    /// Obtener una objeto ResponseDTO con la información de la consulta o request.
    /// </summary>
    /// <param name="message">Mensaje a mostrar en la respuesta</param>
    /// <param name="data">Información consultada</param>
    /// <param name="statusCode">Código http (por defecto 200)</param>
    /// <returns>Objeto ResponseDTO</returns>
    public static ResponseDTO<T> Success(string message, object data = null, int statusCode = (int)HttpStatusCode.OK)
    {
        return NewResponse(statusCode, message, data);
    }

    /// <summary>
    /// Obtener una objeto ResponseDTO indicando que la consulta no se hizo correctamente.
    /// </summary>
    /// <param name="message">Mensaje a mostrar en la respuesta.</param>
    /// <param name="error">Errores por mostrar.</param>
    /// <returns>Objeto ResponseDTO</returns>
    public static ResponseDTO<T> BadRequest(string message, string error = null)
    {
        return NewResponse((int)HttpStatusCode.BadRequest, message, default, error);
    }

    /// <summary>
    /// Obtener un objeto ResponseDTO indicando que no se encontró lo que se está consultando.
    /// </summary>
    /// <param name="message">Mensaje a mostrar en la respuesta.</param>
    /// <returns>Objeto ResponseDTO</returns>
    public static ResponseDTO<T> NotFound(string message)
    {
        return NewResponse((int)HttpStatusCode.NotFound, message, default);
    }

    public static ResponseDTO<T> InternalServerError(string message, string error = null)
    {
        return NewResponse((int)HttpStatusCode.InternalServerError, message, default, error);
    }
}
