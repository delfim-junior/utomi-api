using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services
{
    public class FileHandlerService : IFileHandler
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public FileHandlerService(ITokenGenerator tokenGenerator, IConfiguration configuration,
            IHostEnvironment environment)
        {
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<string> WriteFile(IFormFile file, string appSettingsDirectoryProperty)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileToken = $"{_tokenGenerator.GenerateSecureString(20)}{fileExtension}";

            //Generate Final File path
            var finalFilePath = GenerateFinalFilePath(fileToken, appSettingsDirectoryProperty);

            //Save File
            await using var fs = new FileStream(finalFilePath, FileMode.Create);

            await file.CopyToAsync(fs);

            return finalFilePath;
        }

        private string GenerateFinalFilePath(string fileToken, string appConfigProperty)
        {
            var uploadDir = _configuration[appConfigProperty];
            var root = "/";
            if (uploadDir[0] != '/')
            {
                root = _environment.ContentRootPath;
            }

            return Path.Combine(root, uploadDir);

        }

        private static bool ByteArrayToFile(string filePath, byte[] byteArray)
        {
            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.WriteAsync(byteArray, 0, byteArray.Length);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}