namespace InstagramDMs.API.Services
{
    public class MediaSecurityService
    {
        private readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
{
    // PNG
    {
        ".png", new List<byte[]>
        {
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
        }
    },
    // JPEG
    {
        ".jpeg", new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },
    {
        ".jpg", new List<byte[]>
        {
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
        }
    },
    // GIF
    {
        ".gif", new List<byte[]>
        {
            new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 },
            new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }
        }
    },
    // MP4
    {
        ".mp4", new List<byte[]>
        {
            new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 },
            new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 },
            new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }
        }
    },
    // OGG
    {
        ".ogg", new List<byte[]>
        {
            new byte[] { 0x4F, 0x67, 0x67, 0x53 }
        }
    },
    // AVI
    {
        ".avi", new List<byte[]>
        {
            new byte[] { 0x52, 0x49, 0x46, 0x46, 0x00, 0x00, 0x00, 0x00, 0x41, 0x56, 0x49, 0x20 }
        }
    },
    // MOV
    {
        ".mov", new List<byte[]>
        {
            new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 },
            new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }
        }
    },
    // WEBM
    {
        ".webm", new List<byte[]>
        {
            new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }
        }
    }
};


        public bool IsMediaValid(IFormFile media)
        {
            try
            {
                using (var reader = new BinaryReader(media.OpenReadStream()))
                {

                    var signatures = _fileSignature[Path.GetExtension(media.FileName)];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    var result = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                    return result;
                }

            }
            catch (Exception ec)
            {
                return false;
            }
        }
        public string GetMediaType(IFormFile media)
        {
            if (media.ContentType.StartsWith("image/"))
            {
                return "image";
            }

            if (media.ContentType.StartsWith("video/"))
            {
                return "video";
            }

            return null;
        }
    }
}