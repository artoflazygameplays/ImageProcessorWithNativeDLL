#include "pch.h"
#include <opencv2/opencv.hpp>
#include <vector>
#include <objbase.h>
#include <thread>
#include <algorithm>

#define EXPORT_METHOD extern "C" __declspec(dllexport)

EXPORT_METHOD
bool GaussianBlurImage(
    unsigned char* input,
    int inputSize,
    unsigned char** output,
    int* outputSize,
    int kernelSize) 
{
    if (!input || inputSize <= 0 || !output || !outputSize || kernelSize <= 0)
        return false;

    try {
        std::vector<unsigned char> buffer(input, input + inputSize);
        cv::Mat img = cv::imdecode(buffer, cv::IMREAD_UNCHANGED);
        if (img.empty()) return false;

        cv::Mat result = img.clone();

        unsigned int numThreads = std::thread::hardware_concurrency();
        if (numThreads == 0) numThreads = 2; // fallback

        std::vector<std::thread> threads;
        int rowsPerThread = img.rows / numThreads;
        int remainder = img.rows % numThreads;

        int startRow = 0;
        for (unsigned int t = 0; t < numThreads; t++) {
            int endRow = startRow + rowsPerThread;
            if (t == numThreads - 1) endRow += remainder;

            threads.emplace_back([&, startRow, endRow]() {
                for (int r = startRow; r < endRow; r++) {
                    cv::Mat srcRow = img.row(r);
                    cv::Mat dstRow = result.row(r);
                    cv::GaussianBlur(srcRow, dstRow, cv::Size(kernelSize, kernelSize), 0);
                }
                });

            startRow = endRow;
        }

        for (auto& th : threads) {
            if (th.joinable()) th.join();
        }

        std::vector<unsigned char> outBuf;
        cv::imencode(".png", result, outBuf);

        *outputSize = static_cast<int>(outBuf.size());
        *output = (unsigned char*)CoTaskMemAlloc(*outputSize);
        if (!*output) return false;

        memcpy(*output, outBuf.data(), *outputSize);

        return true;
    }
    catch (...) {
        return false;
    }
}

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
