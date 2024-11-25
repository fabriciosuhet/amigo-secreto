using System.Text.Json.Serialization;
using Presentes.Models.Enums;

namespace Presentes.Entities;

public class Usuario
{
    public Guid Id { get; init; }
    
    public string Name { get; private set; }
    [JsonIgnore]
    public string PasswordSalt { get; private set; }
    [JsonIgnore]
    public string Email { get; private set; }
    [JsonIgnore]
    public string PassWordHash { get; private set; }

    public Role Role { get; private set; }
    
    private Usuario(){}

    public Usuario(string name, string email, string passWordHash, string passwordSalt, Role role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PassWordHash = passWordHash;
        PasswordSalt = passwordSalt;
        Role = role;
    }

    public void AlterarNome(string nome)
    {
        Name = nome;
    }

    public void AlterarEmail(string email)
    {
        Email = email;
    }

    public void AlterarPassWord(string passordHash, string passwordSalt)
    {
        PassWordHash = passordHash;
        PasswordSalt = passwordSalt;
    }

    public void AlterarRole(Role role)
    {
        Role = role;
    }
}