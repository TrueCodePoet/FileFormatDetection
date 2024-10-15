using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFormatDetection
{
    public static class FileSignatureChecker
    {
        public class FileType
        {
            public int Offset { get; set; }
            public List<string> EXT { get; set; }
            public byte[] MagicNumbers { get; set; }
        }


        public static readonly List<FileType> MagicKeys = new List<FileType> {

                    // Images
                    new FileType() { Offset = 0, MagicNumbers = Encoding.ASCII.GetBytes("BM")    , EXT = new List<string>() {".BMP", ".DIB" } },
                    new FileType() { Offset = 0, MagicNumbers = Encoding.ASCII.GetBytes("GIF")   , EXT = new List<string>() {".GIF" } },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("89 50 4E 47 0D 0A 1A 0A".Replace(" ",""))   , EXT = new List<string>() {".PNG" } },
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 137, 80, 78, 71 }   , EXT = new List<string>() {".PNG" } },
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 73, 73, 42 }        , EXT = new List<string>() {".TIF" } },          // TIFF
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 77, 77, 42 }        , EXT = new List<string>() {".TIF" } },          // TIFF
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 255, 216, 255, 219 }, EXT = new List<string>() {".JPG" } },  // jpeg
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 255, 216, 255, 224 }, EXT = new List<string>() {".JPG" } },  // jpeg
                    new FileType() { Offset = 0, MagicNumbers = new byte[] { 255, 216, 255, 225 }, EXT = new List<string>() {".JPG" } },
                    // 	ff d8 ff e0
                    
                    // Video
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("1A 45 DF A3".Replace(" ","")), EXT = new List<string>()                  {".MKV",".MKA",".MKS",".MK3D",".WEBM" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("30 26 B2 75 8E 66 CF 11".Replace(" ","")), EXT = new List<string>()      {".ASF",".WMA",".WMV"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("A6 D9 00 AA 00 62 CE 6C".Replace(" ","")), EXT = new List<string>()      {".ASF",".WMA",".WMV"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("52 49 46 46".Replace(" ","")), EXT = new List<string>()      {".AVI"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("41 56 49 20".Replace(" ","")), EXT = new List<string>()      {".AVI"}  },

                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("41 56 49 20".Replace(" ","")), EXT = new List<string>()      {".MP4"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 00 14 66 74 79 70 71 74 20 20".Replace(" ","")), EXT = new List<string>()      {".MP4"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 00 18 66 74 79 70 33 67 70 35".Replace(" ","")), EXT = new List<string>()      {".MP4"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 00 18 66 74 79 70 6D 70 34 32".Replace(" ","")), EXT = new List<string>()      {".MP4"}  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 00 20 66 74 79 70 33 67 70".Replace(" ","")), EXT = new List<string>()      {".MP4"}  },

                    // Compressed
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("1F 8B".Replace(" ","")),                   EXT = new List<string>()            {".GZ" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("75 73 74 61 72 00 30 30".Replace(" ","")), EXT = new List<string>()            {".TAR" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("75 73 74 61 72 20 20 00".Replace(" ","")), EXT = new List<string>()            {".TAR" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("504B0304"),                                EXT = new List<string>()            { ".ZIP", ".JAR", ".ODT", ".ODS", ".ODP", ".DOCX", ".XLSX", ".PPTX", ".VSDX", ".APK" }  },



                    // MUSIC
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF FB".Replace(" ","")), EXT = new List<string>()            {".MP3" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("49 44 33".Replace(" ","")), EXT = new List<string>()         {".MP3" }  },
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4F 67 67 53".Replace(" ","")), EXT = new List<string>()      {".OGG", ".OGA", ".OGV" }  },

                    //  Testing These

                    // Images
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF D8 FF E0".Replace(" ","")), EXT = new List<string>() {".JPG", ".JPEG" } },  // JPEG
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF D8 FF E1".Replace(" ","")), EXT = new List<string>() {".JPG", ".JPEG" } },  // JPEG (Exif)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF D8 FF E8".Replace(" ","")), EXT = new List<string>() {".JPG", ".JPEG" } },  // Still Picture Interchange File Format (SPIFF)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("38 42 50 53".Replace(" ","")), EXT = new List<string>() {".PSD" } },           // Photoshop Document
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 01 00".Replace(" ","")), EXT = new List<string>() {".ICO" } },           // Icon File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 02 00".Replace(" ","")), EXT = new List<string>() {".CUR" } },           // Cursor File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 00 0C 6A 50 20 20 0D 0A".Replace(" ","")), EXT = new List<string>() {".JP2" } }, // JPEG 2000
                    new FileType() { Offset = 8, MagicNumbers = StringToByteArray("57 45 42 50".Replace(" ","")), EXT = new List<string>() {".WEBP" } },          // WebP
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("25 50 44 46 2D".Replace(" ","")), EXT = new List<string>() {".PDF" } },        // PDF
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("3C 3F 78 6D 6C 20".Replace(" ","")), EXT = new List<string>() {".XML", ".SVG" } }, // XML/SVG
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("25 21 50 53".Replace(" ","")), EXT = new List<string>() {".EPS" } },           // Encapsulated PostScript
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("76 2F 31 01".Replace(" ","")), EXT = new List<string>() {".EXR" } },           // OpenEXR Image
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("67 6C 54 46".Replace(" ","")), EXT = new List<string>() {".GLB" } },           // GL Transmission Format (Binary)


                    // Documents
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("D0 CF 11 E0 A1 B1 1A E1".Replace(" ","")), EXT = new List<string>() {".DOC", ".XLS", ".PPT", ".MSI" } }, // Microsoft Office 97–2003 Documents
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("7B 5C 72 74 66 31".Replace(" ","")), EXT = new List<string>() {".RTF" } },     // Rich Text Format
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("21 42 44 4E".Replace(" ","")), EXT = new List<string>() {".PST" } },           // Outlook Personal Folder File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("49 54 53 46 03 00 00 00".Replace(" ","")), EXT = new List<string>() {".CHM" } }, // Compiled HTML Help File

                    // Archives/Compressed Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("37 7A BC AF 27 1C".Replace(" ","")), EXT = new List<string>() {".7Z" } },      // 7-Zip
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("52 61 72 21 1A 07 00".Replace(" ","")), EXT = new List<string>() {".RAR" } }, // RAR Archive (Version 1.5 to 4.0)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("52 61 72 21 1A 07 01 00".Replace(" ","")), EXT = new List<string>() {".RAR" } }, // RAR Archive (Version 5.0+)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4D 53 43 46".Replace(" ","")), EXT = new List<string>() {".CAB" } },           // Microsoft Cabinet File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("42 5A 68".Replace(" ","")), EXT = new List<string>() {".BZ2" } },              // BZIP2
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("1F A0".Replace(" ","")), EXT = new List<string>() {".LZH" } },                 // LZH Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("60 EA".Replace(" ","")), EXT = new List<string>() {".ARJ" } },                 // ARJ Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FD 37 7A 58 5A 00".Replace(" ","")), EXT = new List<string>() {".XZ" } },     // XZ Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("5D 00 00 80 00".Replace(" ","")), EXT = new List<string>() {".LZMA" } },      // LZMA Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("1F 9D".Replace(" ","")), EXT = new List<string>() {".Z" } },                   // Z Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("30 37 30 37 30".Replace(" ","")), EXT = new List<string>() {".CPIO" } },       // CPIO Archive
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("21 3C 61 72 63 68 3E".Replace(" ","")), EXT = new List<string>() {".DEB" } }, // Debian Software Package
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("ED AB EE DB".Replace(" ","")), EXT = new List<string>() {".RPM" } },           // Red Hat Package Manager
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4B 44 4D".Replace(" ","")), EXT = new List<string>() {".VMDK" } },            // VMware Virtual Disk
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4D 53 57 49 4D 00 00 00".Replace(" ","")), EXT = new List<string>() {".WIM" } }, // Windows Imaging Format
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("7A 50 41 51 66 69 6C 65".Replace(" ","")), EXT = new List<string>() {".ZPAQ" } }, // ZPAQ Compressed File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("78 61 72 21".Replace(" ","")), EXT = new List<string>() {".XAR" } },           // Extensible Archive

                    // Audio Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("66 4C 61 43".Replace(" ","")), EXT = new List<string>() {".FLAC" } },          // Free Lossless Audio Codec
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4D 54 68 64".Replace(" ","")), EXT = new List<string>() {".MID", ".MIDI" } }, // MIDI Audio File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("77 76 70 6B".Replace(" ","")), EXT = new List<string>() {".WV" } },            // WavPack Audio
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("2E 73 6E 64".Replace(" ","")), EXT = new List<string>() {".AU" } },            // AU Audio File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF F1".Replace(" ","")), EXT = new List<string>() {".AAC" } },                 // Advanced Audio Coding
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF F9".Replace(" ","")), EXT = new List<string>() {".AAC" } },                 // Advanced Audio Coding
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4D 41 43 20".Replace(" ","")), EXT = new List<string>() {".APE" } },           // Monkey's Audio
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("66 72 65 65".Replace(" ","")), EXT = new List<string>() {".M4A", ".ALAC" } }, // Apple Lossless Audio Codec
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("52 49 46 46".Replace(" ","")), EXT = new List<string>() {".WAV" } },           // WAV Audio File
                    new FileType() { Offset = 8, MagicNumbers = StringToByteArray("57 41 56 45".Replace(" ","")), EXT = new List<string>() {".WAV" } },           // WAV Audio File

                    // Video Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 01 BA".Replace(" ","")), EXT = new List<string>() {".MPG", ".MPEG", ".VOB", ".DAT" } }, // MPEG Video
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("47".Replace(" ","")), EXT = new List<string>() {".MTS", ".M2TS", ".TS" } },    // MPEG Transport Stream
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("46 4C 56".Replace(" ","")), EXT = new List<string>() {".FLV" } },              // Flash Video
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("2E 52 4D 46".Replace(" ","")), EXT = new List<string>() {".RM", ".RMVB" } },   // RealMedia File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("43 57 53".Replace(" ","")), EXT = new List<string>() {".SWF" } },              // Shockwave Flash
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("46 57 53".Replace(" ","")), EXT = new List<string>() {".SWF" } },              // Shockwave Flash
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("67 6C 54 46".Replace(" ","")), EXT = new List<string>() {".GLB" } },           // GL Transmission Format (Binary)

                    // Executable Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4D 5A".Replace(" ","")), EXT = new List<string>() {".EXE", ".DLL", ".SYS", ".DRV" } }, // Windows Executable
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("7F 45 4C 46".Replace(" ","")), EXT = new List<string>() {".ELF" } },           // Executable and Linkable Format
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FE ED FA CE".Replace(" ","")), EXT = new List<string>() {".MACHO" } },         // Mach-O 32-bit
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FE ED FA CF".Replace(" ","")), EXT = new List<string>() {".MACHO" } },         // Mach-O 64-bit
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("CA FE BA BE".Replace(" ","")), EXT = new List<string>() {".CLASS" } },         // Java Class File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4C 00 00 00".Replace(" ","")), EXT = new List<string>() {".LNK" } },           // Windows Shortcut
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("64 65 78 0A 30 33 35 00".Replace(" ","")), EXT = new List<string>() {".DEX" } }, // Dalvik Executable (Android)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("62 70 6C 69 73 74 30 30".Replace(" ","")), EXT = new List<string>() {".PLIST" } }, // Binary Property List (Apple)

                    // Database Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("53 51 4C 69 74 65 20 66".Replace(" ","")), EXT = new List<string>() {".SQLITE", ".SQLITEDB", ".DB" } }, // SQLite Database

                    // Font Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 01 00 00 00".Replace(" ","")), EXT = new List<string>() {".TTF" } },        // TrueType Font
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4F 54 54 4F 00".Replace(" ","")), EXT = new List<string>() {".OTF" } },        // OpenType Font
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("77 4F 46 46".Replace(" ","")), EXT = new List<string>() {".WOFF" } },          // Web Open Font Format
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("77 4F 46 32".Replace(" ","")), EXT = new List<string>() {".WOFF2" } },         // Web Open Font Format 2

                    // Virtual Disk Images
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("76 68 64 78 66 69 6C 65".Replace(" ","")), EXT = new List<string>() {".VHDX" } }, // Hyper-V Virtual Hard Disk
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4B 44 4D".Replace(" ","")), EXT = new List<string>() {".VMDK" } },            // VMware Virtual Disk

                    // Disk Images
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("78 01 73 0D 62 62 60".Replace(" ","")), EXT = new List<string>() {".DMG" } }, // Apple Disk Image

                    // Certificate Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("30 82".Replace(" ","")), EXT = new List<string>() {".DER" } },                 // DER Encoded X.509 Certificate

                    // Network Capture Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("D4 C3 B2 A1".Replace(" ","")), EXT = new List<string>() {".PCAP" } },         // PCAP File (Little Endian)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("A1 B2 C3 D4".Replace(" ","")), EXT = new List<string>() {".PCAP" } },         // PCAP File (Big Endian)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("0A 0D 0D 0A".Replace(" ","")), EXT = new List<string>() {".PCAPNG" } },       // PCAP Next Generation

                    // Miscellaneous Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("B1 68 DE 3A".Replace(" ","")), EXT = new List<string>() {".DCX" } },          // ZSoft IBM PC Multi-Page Paintbrush
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("43 72 32 34".Replace(" ","")), EXT = new List<string>() {".CRX" } },          // Chrome Extension
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("01 00 00 00 20 45 4D 46".Replace(" ","")), EXT = new List<string>() {".EMF" } }, // Enhanced Metafile
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FF 4F FF 51".Replace(" ","")), EXT = new List<string>() {".J2C", ".J2K" } },  // JPEG 2000 Codestream
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("30 26 B2 75 8E 66 CF 11".Replace(" ","")), EXT = new List<string>() {".WMV", ".WMA", ".ASF" } }, // Windows Media
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("00 00 01 B3".Replace(" ","")), EXT = new List<string>() {".MPG", ".MPEG" } }, // MPEG Video
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4F 67 67 53".Replace(" ","")), EXT = new List<string>() {".OGG", ".OGA", ".OGV", ".OGX" } }, // OGG Container
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("23 21 41 4D 52".Replace(" ","")), EXT = new List<string>() {".AMR" } },       // Adaptive Multi-Rate Audio
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("64 73 65 73".Replace(" ","")), EXT = new List<string>() {".DSF" } },          // Sony DSD Stream File
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("46 4F 52 4D".Replace(" ","")), EXT = new List<string>() {".AIFF", ".AIF" } }, // Audio Interchange File Format
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("7B 0D 0A 6F 20 66 66 73".Replace(" ","")), EXT = new List<string>() {".EOT" } }, // Embedded OpenType Font
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("3C 3F 78 6D 6C 20 76 65".Replace(" ","")), EXT = new List<string>() {".KML" } }, // Keyhole Markup Language

                    // Files with Offset Signatures
                    new FileType() { Offset = 8, MagicNumbers = StringToByteArray("41 49 46 46".Replace(" ","")), EXT = new List<string>() {".AIFF", ".AIF" } }, // AIFF Audio
                    new FileType() { Offset = 4, MagicNumbers = StringToByteArray("66 74 79 70 4D 53 4E 56".Replace(" ","")), EXT = new List<string>() {".M4V", ".MP4" } }, // MPEG-4 Video
                    new FileType() { Offset = 4, MagicNumbers = StringToByteArray("66 74 79 70 4D 34 41".Replace(" ","")), EXT = new List<string>() {".M4A", ".MP4" } }, // MPEG-4 Audio

                    // ISO Image
                    new FileType() { Offset = 32769, MagicNumbers = StringToByteArray("43 44 30 30 31".Replace(" ","")), EXT = new List<string>() {".ISO" } },    // ISO9660 CD/DVD Image

                    // Additional Audio Formats
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("2E 52 4D 46".Replace(" ","")), EXT = new List<string>() {".RA", ".RAM" } },    // Real Audio
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("4F 67 67 53".Replace(" ","")), EXT = new List<string>() {".SPX" } },           // Speex Audio

                    // Additional Executable Formats
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("50 4B 03 04".Replace(" ","")), EXT = new List<string>() {".APK" } },           // Android Package (ZIP)
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("FE ED FA CE".Replace(" ","")), EXT = new List<string>() {".DYLIB" } },         // macOS Dynamic Library

                    // Additional Image Formats
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("42 4D".Replace(" ","")), EXT = new List<string>() {".BMP", ".DIB" } },         // Bitmap Image

                    // Additional Web Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("3C 21 44 4F 43 54 59 50 45".Replace(" ","")), EXT = new List<string>() {".HTML", ".HTM" } }, // HTML Document

                    // Flash Video
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("46 57 53".Replace(" ","")), EXT = new List<string>() {".SWF" } },              // Flash

                    // Additional Font Files
                    new FileType() { Offset = 34, MagicNumbers = StringToByteArray("4C 50".Replace(" ","")), EXT = new List<string>() {".EOT" } },                // Embedded OpenType Font

                    // Additional Music Formats
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("49 44 33".Replace(" ","")), EXT = new List<string>() {".MP3" } },              // MP3 Audio

                    // Additional Executable Files
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("EF BB BF".Replace(" ","")), EXT = new List<string>() {".PS1", ".BAT", ".CMD" } }, // UTF-8 with BOM Scripts

                    // Additional Archives
                    new FileType() { Offset = 0, MagicNumbers = StringToByteArray("1F 8B 08".Replace(" ","")), EXT = new List<string>() {".GZ" } },               // GZIP Compressed File


        };
        public static bool IsValidFormat(String path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                byte[] header = new byte[1000];
                fs.Read(header, 0, 1000);
                if (IsValidFormat(header)) return true;
            }
            return false;
        }

        public static bool IsValidFormat(byte[] header)
        {

            foreach (var pattern in MagicKeys)
            {
                if (pattern.MagicNumbers.SequenceEqual(header.Skip(pattern.Offset).Take(pattern.MagicNumbers.Length)))
                    return true;
            }
            return false;
            //52 49 46 46 nn nn nn nn
            //41 56 49 20
        }

        public static bool IsValidFormat(byte[] header, string extension)
        {
            string ext = extension.ToUpperInvariant();
            var patterns = MagicKeys.Where(x => x.EXT.Contains(ext));
            foreach (var pattern in patterns)
            {
                if (pattern.MagicNumbers.SequenceEqual(header.Skip(pattern.Offset).Take(pattern.MagicNumbers.Length)))
                    return true;
            }
            return false;
        }

        public static bool IsValidFormat(string path, string extension)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                byte[] header = new byte[1000];
                fs.Read(header, 0, header.Length);
                return IsValidFormat(header, extension);
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static bool CanValidateFile(String path)
        {
            return (MagicKeys.Where(x => x.EXT.Contains(System.IO.Path.GetExtension(path).ToUpperInvariant())).Any());
        }

    }
}
