using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.Entities;

public class BasicAuthUser : BaseEntity
{
    [MaxLength(50)]
    public string Username { get; set; }
    [MaxLength(200)]
    public string Password { get; set; }
}