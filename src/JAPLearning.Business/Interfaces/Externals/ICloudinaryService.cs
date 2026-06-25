using Microsoft.AspNetCore.Http;

namespace JAPLearning.Business.Interfaces.Externals
{
    public interface ICloudinaryService
    {
        /// <summary>Faz upload de uma imagem e devolve a URL segura (https).</summary>
        Task<string?> UploadImageAsync(IFormFile file, string folder);

        /// <summary>Faz upload de um ficheiro raw (PDF, ZIP, etc.) e devolve a URL segura.</summary>
        Task<string?> UploadRawFileAsync(IFormFile file, string folder);

        /// <summary>Elimina uma imagem pelo publicId (ex: "mundodev/users/abc123").</summary>
        Task<bool> DeleteImageAsync(string publicId);

        /// <summary>Elimina um ficheiro raw pelo publicId.</summary>
        Task<bool> DeleteRawFileAsync(string publicId);

        /// <summary>Extrai o publicId a partir de uma URL Cloudinary.</summary>
        string? ExtractPublicId(string? url);
    }
}
