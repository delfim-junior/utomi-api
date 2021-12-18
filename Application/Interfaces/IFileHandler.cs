using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileHandler
    {
        Task<string> WriteFile(IFormFile file, string appSettingsDirectoryProperty);
    }
}