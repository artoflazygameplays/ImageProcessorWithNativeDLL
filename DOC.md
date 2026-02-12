# 3DHISTECH Image Processing Demo – One Page Overview

## Overview
This application is a full-stack image processing system that applies a Gaussian blur to uploaded images using a native C++ OpenCV library.

**Technology Stack**
- Frontend: Angular (Standalone API)
- Backend: ASP.NET Core Web API
- Native Layer: C++ DLL (OpenCV)
- Communication: HTTP (Base64 upload, Blob download)

---

## Architecture Flow
Angular Client → ASP.NET Core API → C# ImageProcessor (P/Invoke) → C++ DLL (OpenCV + Multithreading) → Binary Image Response → Angular Download

---

## Frontend (Angular)

**Responsibilities**
- Select image file
- Convert image to Base64
- Send processing request to API
- Receive processed image as Blob
- Trigger browser download

**Upload Process**
1. User selects file.
2. FileReader converts file to Base64.
3. Base64 string is sent to:
   ```
   POST /api/image/process
   ```
4. Backend returns processed image as binary Blob.
5. Blob is converted to object URL.
6. Download is triggered via dynamically created `<a>` element.

---

## Backend (ASP.NET Core)
**Endpoint**
```
POST /api/image/process
```

**Request Body**
```json
{
  "imageBase64": "string",
  "outputEncoding": "Png | Jpeg"
}
```

**Processing Steps**
1. Convert Base64 string to byte array.
2. Call `ImageProcessor.ProcessAsync`.
3. Return processed image using `File()` with correct MIME type:
   - `image/png`
   - `image/jpeg`

Swagger documentation:
```
/swagger
/swagger/v1/swagger.json
```

---

## ImageProcessor Service (C#)

- Uses P/Invoke to call native method:
  ```
  GaussianBlurImage(...)
  ```
- Executes CPU-heavy work via `Task.Run`.
- Copies unmanaged memory into managed byte array.
- Frees native memory using `Marshal.FreeCoTaskMem`.

---

## Native C++ Layer (OpenCV)

**Processing Pipeline**
1. Decode input bytes using `cv::imdecode`.
2. Clone image matrix.
3. Determine thread count using `hardware_concurrency()`.
4. Split rows among threads.
5. Apply `cv::GaussianBlur`.
6. Encode result to PNG in memory.
7. Allocate output buffer with `CoTaskMemAlloc`.

**Threading**
- Uses `std::thread`
- Row-based parallel processing

---

## Key Characteristics
- Fully in-memory processing (no disk I/O)
- Multithreaded native execution
- Proper managed/unmanaged memory handling
- Efficient binary transfer using Blob
- Clear API contract via Swagger

---

## End-to-End Summary
1. User uploads image.
2. Angular converts image to Base64.
3. API decodes Base64 and calls native DLL.
4. OpenCV applies Gaussian blur.
5. Processed image returned as binary.
6. User downloads processed result.

---

## Requirements

**Backend**
- .NET 7/8
- OpenCV installed
- ImageProcessor.dll available

**Frontend**
- Node.js
- Angular CLI

This project demonstrates integration between Angular, ASP.NET Core, and native C++ image processing with multithreaded execution and safe memory interoperability.
