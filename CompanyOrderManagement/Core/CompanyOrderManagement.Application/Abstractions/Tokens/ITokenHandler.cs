using CompanyOrderManagement.Application.DTOs;


namespace CompanyOrderManagement.Application.Abstractions.Tokens
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(int minute);

    }
}
