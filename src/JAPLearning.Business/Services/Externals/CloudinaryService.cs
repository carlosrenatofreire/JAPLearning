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
            _cloudinary = new Cloudinary(acc) { Api = { Secure = true } };
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
