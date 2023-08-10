using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.DTO.NCR
{
    public class CustomerDTO
    {
        [Required]
        public string ClienteId { get; set; }
        [Required]
        public string TipoPersona { get; set; }
        public string Nombre { get; set; }

        [Required]
        public List<string> Emails { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        [Required]
        public string TipoIdentificacion { get; set; }
        [Required]
        public string Regimen { get; set; }
        [Required]
        public string ResponsabilidadFiscal { get; set; }
    }

}
