using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WebApplicationApi.Endpoints;

public static class UploadFileEndpoints
{
    public static void MapUploadFileEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("upload-image", UploadImage)
            .DisableAntiforgery();
    }

    private static async Task<IResult> UploadImage(IFormFile file, IConfiguration configuration)
    {
        if (file.Length == 0)
        {
            return Results.BadRequest("Файл не выбран");
        }

        if (!ValidateFileType(file.FileName))
        {
            return Results.BadRequest("Неверный тип файла");
        }

        var uploadImageFolderPath = configuration["UploadImageFolderPath"]!;

        var imageSize = await ProcessingImage(file, uploadImageFolderPath);

        return Results.Ok(imageSize);
    }

    private static async Task<long> ProcessingImage(IFormFile file, string uploadImageFolderPath)
    {
        var fileFullPath = Path.Combine(uploadImageFolderPath, file.FileName);
        using var watermark = await Image.LoadAsync("Resources/watermark.png");
        using var image = await Image.LoadAsync(file.OpenReadStream());
        
        var scale = CalculateScale(image.Width, image.Height, watermark.Width, watermark.Height);
        var size = new Size((int)(watermark.Width * scale), (int)(watermark.Height * scale));
        
        watermark.Mutate(x => x.Resize(size));
        
        var location = new Point((image.Width - watermark.Width) / 2, (image.Height - watermark.Height) / 2);
        
        image.Mutate(x => x.DrawImage(watermark, location, PixelColorBlendingMode.Add, 0.5f));
        
        using var outputStream = new MemoryStream();
        
        await image.SaveAsync(outputStream, new JpegEncoder() { Quality = 90 });
        
        var imageSizeInBytes = outputStream.Length;
        
        await using var fileStream = File.Create(fileFullPath);
        
        outputStream.Seek(0, SeekOrigin.Begin);
        
        await outputStream.CopyToAsync(fileStream);
    
        return imageSizeInBytes;
    }

    private static bool ValidateFileType(string fileName)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        return allowedExtensions.Contains(Path.GetExtension(fileName).ToLower());
    }

    private static float CalculateScale(int imageWidth, int imageHeight, int watermarkWidth, int watermarkHeight)
    {
        const float k = 0.8f;
        var widthScale = (float)imageWidth / watermarkWidth * k;
        var heightScale = (float)imageHeight / watermarkHeight * k;
        return Math.Min(widthScale, heightScale);
    }
}