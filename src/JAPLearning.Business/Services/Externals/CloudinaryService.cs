using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Models.Settings;

namespace JAPLearning.Business.Services.Externals
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary        _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IOptions<CloudinarySetting> settings, ILogger<CloudinaryService> logger)
        {
            _logger = logger;
            var s   = settings.Value;
            var acc = new Account(s.CloudName, s.ApiKey, s.ApiSecret);
            _cloudinary = new Cloudinary(acc)
            {
                Api =
                {
                    Secure  = true,
                    Timeout = 600000  // 10 minutos em milissegundos
                }
            };
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            // Only allow image MIME types
            var allowed = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowed.Contains(file.ContentType.ToLower()))
            {
                _logger.LogWarning("Cloudinary upload rejected: unsupported content type {Type}", file.ContentType);
                return null;
            }

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File           = new FileDescription(file.FileName, stream),
                Folder         = $"mundodev/{folder}",
                Transformation = new Transformation()
                                     .Width(1200).Height(800).Crop("limit")
                                     .Quality("auto").FetchFormat("auto"),
                Overwrite      = true
            };

            try
            {
                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    _logger.LogError("Cloudinary upload error: {Msg}", result.Error.Message);
                    return null;
                }

                return result.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cloudinary upload exception");
                return null;
            }
        }

        public async Task<string?> UploadRawFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0) return null;

            // Ler todo o ficheiro para um array de bytes antes de enviar ao Cloudinary.
            // Garante que os dados estão completamente em memória e independentes do
            // stream do request HTTP (que pode ser cancelado durante uploads longos).
            byte[] fileBytes;
            await using (var src = file.OpenReadStream())
            {
                fileBytes = new byte[file.Length];
                var totalRead = 0;
                while (totalRead < fileBytes.Length)
                {
                    var read = await src.ReadAsync(fileBytes, totalRead, fileBytes.Length - totalRead);
                    if (read == 0) break;
                    totalRead += read;
                }
            }

            var safeId   = $"{folder}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var mimeType = file.ContentType;

            _logger.LogInformation(
                "Cloudinary raw upload início: {Name} | {Size} bytes | {Mime} → mundodev/{Folder}",
                file.FileName, fileBytes.Length, mimeType, folder);

            try
            {
                using var ms = new MemoryStream(fileBytes);

                var uploadParams = new RawUploadParams
                {
                    File        = new FileDescription(file.FileName, ms),
                    Folder      = $"mundodev/{folder}",
                    PublicId    = safeId,
                    Overwrite   = true,
                    UseFilename = false
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    var errMsg = $"[CLOUDINARY RAW ERROR] {result.Error.Message}";
                    _logger.LogError(errMsg);
                    Console.Error.WriteLine(errMsg);
                    return null;
                }

                var url = result.SecureUrl?.ToString() ?? result.Url?.ToString();
                _logger.LogInformation("Cloudinary raw upload OK: {Url}", url);
                Console.WriteLine($"[CLOUDINARY RAW OK] {url}");
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cloudinary raw upload excepção: {Name} ({Size} bytes)",
                    file.FileName, file.Length);
                return null;
            }
        }

        public async Task<bool> DeleteRawFileAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return false;
            try
            {
                var result = await _cloudinary.DestroyAsync(
                    new DeletionParams(publicId) { ResourceType = ResourceType.Raw });
                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cloudinary raw delete exception for publicId {Id}", publicId);
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return false;

            try
            {
                var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                return result.Result == "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cloudinary delete exception for publicId {Id}", publicId);
                return false;
            }
        }

        public string? ExtractPublicId(string? url)
        {
            // Cloudinary URL pattern:
            // https://res.cloudinary.com/<cloud>/image/upload/v<version>/<folder>/<filename>.<ext>
            if (string.IsNullOrWhiteSpace(url)) return null;

            try
            {
                var uri     = new Uri(url);
                var path    = uri.AbsolutePath; // /image/upload/v123456/mundodev/users/abc
                var marker  = "/upload/";
                var idx     = path.IndexOf(marker, StringComparison.Ordinal);
                if (idx < 0) return null;

                var afterUpload = path[(idx + marker.Length)..];

                // Strip version segment (v1234567/)
                if (afterUpload.StartsWith("v") && afterUpload.Contains('/'))
                {
                    var slash = afterUpload.IndexOf('/');
                    afterUpload = afterUpload[(slash + 1)..];
                }

                // Strip extension
                var dot = afterUpload.LastIndexOf('.');
                return dot >= 0 ? afterUpload[..dot] : afterUpload;
            }
            catch
            {
                return null;
            }
        }
    }
}
