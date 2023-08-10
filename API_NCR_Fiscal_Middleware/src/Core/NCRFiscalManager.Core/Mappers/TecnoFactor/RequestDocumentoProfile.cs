using AutoMapper;
using NCRFiscalManager.Core.DTO;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.TecnoFactor;

namespace NCRFiscalManager.Core.Mappers.TecnoFactor
{
    public class RequestDocumentoProfile : Profile
    {
        public RequestDocumentoProfile()
        {
            try
            {
                CreateMap<DocumentDTO, RequestDocumentDTO>()
                    .ForMember(dest => dest.Pago,
                        opt => opt.MapFrom(src => new PagoDTO() { MedioPago = "EFECTIVO", FormaPago = "CONTADO" }))
                    .ForMember(dest => dest.Moneda, opt => opt.MapFrom(src => src.Moneda.Replace("$", "COP")))
                    .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Articulos))
                    .ForMember(dest => dest.EmisorId, opt => opt.MapFrom(src => src.EmisorId))
                    .ForMember(dest => dest.DsPrefijo, opt => opt.MapFrom(src => src.NumeroSerie))
                    .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => src.FechaEmision))
                    //.ForMember(dest => dest.FechaVencimiento, opt => opt.MapFrom(src => getFechaVencimiento(DateTime.Parse(src.FechaEmision))))
                    .ForMember(dest => dest.NombresAdquiriente,
                        opt => opt.MapFrom(src => getNameClient(src.Cliente.Nombre))) //Por validar.
                    .ForMember(dest => dest.SegundoNombre, opt => opt.MapFrom(src => ""))
                    .ForMember(dest => dest.PrimerApellido,
                        opt => opt.MapFrom(src => getLastNameClient(src.Cliente.Nombre))) //Por validar.
                    .ForMember(dest => dest.SegundoApellido, opt => opt.MapFrom(src => ""))

                    .ForMember(dest => dest.DsNumeroFactura, opt => opt.MapFrom(src => src.CorrelativoFiscal))
                    .ForMember(dest => dest.DsObservaciones, opt => opt.MapFrom(src => "Observacion"))
                    .ForMember(dest => dest.DsResolucionDian, opt => opt.MapFrom(src => src.NumeroResolucion))
                    .ForMember(dest => dest.EmailAdquiriente,
                        opt => opt.MapFrom(src => src.Cliente.Emails.FirstOrDefault()))
                    .ForMember(dest => dest.CiudadAdquiriente,
                        opt => opt.MapFrom(src => new CiudadAdquirienteDTO() { CdDane = src.CodigoDane }))
                    .ForMember(dest => dest.RegimenAdquiriente, opt => opt.MapFrom(src => "NO_RESPONSABLE_IVA"))
                    .ForMember(dest => dest.TelefonoAdquiriente, opt => opt.MapFrom(src => src.Cliente.Telefono))
                    .ForMember(dest => dest.DireccionAdquiriente, opt => opt.MapFrom(src => src.Cliente.Direccion))
                    .ForMember(dest => dest.TipoPersonaAdquiriente,
                        opt => opt.MapFrom(src => src.Cliente.TipoIdentificacion))
                    .ForMember(dest => dest.AdquirienteResponsable,
                        opt => opt.MapFrom(src => src.Cliente.Regimen == "RESPONSABLE_IVA"))
                    .ForMember(dest => dest.SnConsecutivoAutomatico, opt => opt.MapFrom(src => false))
                    .ForMember(dest => dest.TipoDocumentoElectronico, opt => opt.MapFrom(src => "VENTA"))
                    .ForMember(dest => dest.IdentificacionAdquiriente, opt => opt.MapFrom(src => src.Cliente.ClienteId))
                    .ForMember(dest => dest.ResponsabilidadesFiscales,
                        opt => opt.MapFrom(src => src.Cliente.ResponsabilidadFiscal.Replace("0","O")))
                    .ForMember(dest => dest.TipoIdentificacionAdquiriente,
                        opt => opt.MapFrom(src => src.Cliente.TipoPersona));
                //.ForMember(dest => dest.FacturasReferencia, opt => opt.MapFrom(src => new ArrayList() { new FacturasReferenciaDTO() { Prefijo = src.NumeroSerie, Numero = src.CorrelativoFiscal } }));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        /// <summary>
        /// Método para obtener el primar nombre del cliente.
        /// </summary>
        /// <param name="strNameClient">Nombre completo del cliente</param>
        /// <returns>Retorna string con el primer nombre del cliente</returns>
        private string getNameClient(string strNameClient)
        {
            if (strNameClient == null) return null;
            string[] arrName = strNameClient.Split(" ");
            if (arrName.Length > 0)
            {
                return arrName[0];
            }

            return strNameClient;
        }

        /// <summary>
        /// Método para obtener el primer apellido del cliente.
        /// </summary>
        /// <param name="strNameClient">Nombre completo del cliente</param>
        /// <returns>Retorna string con el primero apellido del cliente</returns>
        private string getLastNameClient(string strNameClient)
        {
            if (strNameClient == null) return null;
            string[] arrName = strNameClient.Split(" ");
            if (arrName.Length > 2)
            {
                return arrName[2];
            }
            else if (arrName.Length == 2)
            {
                return arrName[1];
            }

            return null;
        }
    }
}