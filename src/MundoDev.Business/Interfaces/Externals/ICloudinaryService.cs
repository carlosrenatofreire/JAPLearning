using Microsoft.AspNetCore.Http;

namespace MundoDev.Business.Interfaces.Externals
{
    public interface ICloudinaryService
    {
        /// <summary>Faz upload de uma imagem e devolve a URL segura (https).</summary>
        Task<string?> UploadImageAsync(IFormFile file, string folder);

        /// <summary>Elimina uma imagem pelo publicId (ex: "mundodev/users/abc123").</summary>
        Task<bool> DeleteImageAsync(string publicId);

        /// <summary>Extrai o publicId a partir de uma URL Cloudinary.</summary>
        string? ExtractPublicId(string? url);
    }
}
