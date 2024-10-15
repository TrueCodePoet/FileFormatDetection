# FileTypeValidator

`FileTypeValidator` is a simple and efficient C# utility designed to validate file types based on their "magic numbers" (signature bytes) and file extensions. Magic numbers are unique byte sequences that indicate the file format, and this tool allows you to check files for integrity, prevent format spoofing, and ensure that the actual file content matches its extension.

## Features

- **Magic Number Validation**: Validate files by checking their magic number (signature bytes) against known file formats.
- **Extension-Based Validation**: Ensure that the file extension matches the file content, adding another layer of verification.
- **Supported Formats**: 
  - **Images**: PNG, BMP, GIF, TIFF, JPG
  - **Videos**: MKV, MP4, AVI, ASF, WEBM
  - **Compressed Files**: ZIP, TAR, GZ
  - **Music**: MP3, OGG
- **Extensible**: Easily add support for additional file types by extending the `MagicKeys` list.

## Getting Started

### Prerequisites

- .NET SDK (>= .NET Framework 4.5 or .NET Core)
- Basic understanding of C# for extending and modifying the file types

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/FileTypeValidator.git
    ```

2. Open the project in Visual Studio or your preferred IDE.

3. Build the project to restore dependencies and compile the utility.

### Usage

The main functionality is exposed via the `FileTypeValidator` class, which contains methods for validating file formats based on magic numbers and extensions.

#### Example: Validating a File by Path

You can validate a file by its path using the `IsValidFormat` method:

```csharp
string filePath = "path_to_your_file.ext";

if (FileTypeValidator.IsValidFormat(filePath))
{
    Console.WriteLine("The file format is valid.");
}
else
{
    Console.WriteLine("The file format is not recognized.");
}
```

#### Example: Validating a File with an Expected Extension

You can also validate that the file's content matches the expected extension:

```csharp
string filePath = "path_to_your_file.ext";
string expectedExtension = ".PNG";

if (FileTypeValidator.IsValidFormat(filePath, expectedExtension))
{
    Console.WriteLine("The file is a valid PNG file.");
}
else
{
    Console.WriteLine("The file is not a valid PNG file.");
}
```
