namespace Presentes.Services.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(string email, string role);
}