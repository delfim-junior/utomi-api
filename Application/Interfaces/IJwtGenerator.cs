using Domain;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        string Generate(AppUser appUser);
    }
}