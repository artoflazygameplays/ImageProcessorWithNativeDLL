using _3DHISTECHhomework.Server.Domain.Enum;

namespace _3DHISTECHhomework.Server.Domain.Interface;

public interface IImageProcessor
{
    Task<byte[]> ProcessAsync(byte[] imageBytes, EncodingType outputEncoding, CancellationToken cancellationToken, int kernelSize = 25);
}
