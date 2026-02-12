using _3DHISTECHhomework.Server.Domain.Enum;
using _3DHISTECHhomework.Server.Domain.Interface;
using System.Runtime.InteropServices;


namespace _3DHISTECHhomework.Server.Service.ImageProcess;

public class ImageProcessor : IImageProcessor
{
    private const string dllPath = @"..\..\x64\Debug\ImageProcessor.dll";

    public Task<byte[]> ProcessAsync(byte[] imageBytes, EncodingType outputEncoding, CancellationToken cancellationToken, int kernelSize = 25)
    {
        return Task.Run(() =>
        {
            if (!GaussianBlurImage(imageBytes, imageBytes.Length, out IntPtr ptr, out int size, kernelSize))
                return Array.Empty<byte>();

            if (ptr == IntPtr.Zero || size == 0)
                return Array.Empty<byte>();

            var output = new byte[size];

            Marshal.Copy(ptr, output, 0, size);
            Marshal.FreeCoTaskMem(ptr);

            return output;
        }, cancellationToken);
    }

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool GaussianBlurImage(
        byte[] input,
        int inputSize,
        out IntPtr output,
        out int outputSize,
        int kernelSize);
}
