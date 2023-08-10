namespace NCRFiscalManager.Core.Interfaces
{
    public interface IDBFManager
    {
        /// <summary>
        /// Consultar un archivo DBF.
        /// </summary>
        /// <param name="filePath"> Ruta del documento DBF</param>
        /// <param name="key"> Nombre de la columna que será utilizada como llave</param>
        /// <param name="value"> Nombre de la columna que será utilizada como valor</param>
        /// <returns> Retorna diccionario de tipo <int, string> con los datos organizados </returns>
        public Dictionary<int, string> ReadDictionaryFromDBFWithIntKey(
            string filePath,
            string key,
            string value);
    }
}