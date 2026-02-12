using _3DHISTECHhomework.Server.Domain.Enum;

namespace _3DHISTECHhomework.Server.Domain.DTO;

public class ImageProcessingRequest
{
    /// <summary>Base64 encoded image</summary>
    public string ImageBase64 { get; set; } = string.Empty;

    public EncodingType OutputEncoding { get; set; }
}

