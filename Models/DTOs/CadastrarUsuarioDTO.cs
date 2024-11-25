using Presentes.Models.Enums;

namespace Presentes.Models.DTOs;

public class CadastrarUsuarioDTO
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}