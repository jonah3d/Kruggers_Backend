using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Kruggers_Backend.Service;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<(string? Url, string? PublicId)> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return (null, null);

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Kruggers_BE"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return (uploadResult.SecureUrl?.ToString(), uploadResult.PublicId?.ToString());
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
        return deletionResult.Result == "ok";
    }
}