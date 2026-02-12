using _3DHISTECHhomework.Server.Domain.DTO;
using _3DHISTECHhomework.Server.Domain.Enum;
using _3DHISTECHhomework.Server.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/image")]
public class ImageProcessingController : ControllerBase
{
    public IImageProcessor ImageProcessor { get; }


    public ImageProcessingController(IImageProcessor imageProcessor)
    {
        ImageProcessor = imageProcessor;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessImage(
        [FromBody] ImageProcessingRequest request,
        CancellationToken cancellationToken)
    {
        byte[] imageBytes;

        try
        {
            imageBytes = Convert.FromBase64String(request.ImageBase64);
        }
        catch
        {
            return BadRequest("Invalid base64 image.");
        }

        var processedImage = await ImageProcessor.ProcessAsync(
            imageBytes,
            request.OutputEncoding,
            cancellationToken);

        var mime = request.OutputEncoding == EncodingType.Png
            ? "image/png"
            : "image/jpeg";

        var fileName = request.OutputEncoding == EncodingType.Png
            ? "processed.png"
            : "processed.jpg";

        return File(processedImage, mime, fileName);
    }
}
