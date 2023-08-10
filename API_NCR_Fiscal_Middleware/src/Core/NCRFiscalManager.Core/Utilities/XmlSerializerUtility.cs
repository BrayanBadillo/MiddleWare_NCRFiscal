using System.Xml;
using System.Xml.Serialization;

namespace NCRFiscalManager.Core.Utilities;

public class XmlSerializerUtility
{
    /// <summary>
    /// Método para convertir un DTO a XML
    /// </summary>
    /// <param name="objDTO">Objeto a convertir en XML</param>
    /// <typeparam name="T">Clase genérica</typeparam>
    /// <returns>Retorna string en formato XML</returns>
    public static string GetXml<T>(T objDTO)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(objDTO.GetType());
        var memoryStream = new MemoryStream();
        var xmlWriter = XmlWriter.Create(memoryStream);
        xmlSerializer.Serialize(xmlWriter, objDTO);
        xmlWriter.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);
        var sr = new StreamReader(memoryStream, System.Text.Encoding.UTF8);
        return sr.ReadToEnd();
    }
}