using System.Text;
using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Core.Interfaces;
using SocialExplorer.IO.FastDBF;

namespace NCRFiscalManager.Infraestructure.ThirdParty.DBF.Implementation
{
    public class DBFManager : IDBFManager
    {
        private readonly DbfFile _dbfFile;
        private readonly IConfiguration _configuration;

        public DBFManager()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _dbfFile = new DbfFile(Encoding.GetEncoding(1252));
        }

        /// <summary>
        /// Consultar un archivo DBF.
        /// </summary>
        /// <param name="filePath"> Ruta del documento DBF</param>
        /// <param name="key"> Nombre de la columna que será utilizada como llave</param>
        /// <param name="value"> Nombre de la columna que será utilizada como valor</param>
        /// <returns> Retorna diccionario de tipo <int, string> con los datos organizados </returns>
        public Dictionary<int, string> ReadDictionaryFromDBFWithIntKey(string filePath, string key, string value)
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            Dictionary<int, string> result = new();
            try
            {
                _dbfFile.Open(filePath, FileMode.Open, FileAccess.Read);
                var dbfRecord = new DbfRecord(_dbfFile.Header);
                for (var index = 0; index < _dbfFile.Header.RecordCount; index++)
                {
                    if (!_dbfFile.Read(index, dbfRecord)) break;

                    bool isAValidKey = int.TryParse(dbfRecord[key], out int keyValue);
                    if (isAValidKey)
                        result.Add(keyValue, dbfRecord[value]);
                }
                _dbfFile.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}