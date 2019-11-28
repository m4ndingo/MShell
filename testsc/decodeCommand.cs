using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace testsc
{
    class decodeCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Decode input. Available decoders: b64 zlib gzip";
        }
        public override void Run()
        {
            string decode_results = args;
            byte[] decoded_bytes = null;
            using (MemoryStream decompressedFileStream = new MemoryStream())
            {
                byte[] tmp = last_message.Select(c => (byte)c).ToArray<byte>();
                Stream decompressionStream = null;
                try
                {
                    if (decode_results.Equals("b64"))
                        decoded_bytes = System.Convert.FromBase64String(last_message);
                    if (decode_results.Equals("zlib"))
                        decompressionStream = new DeflateStream(new MemoryStream(tmp), CompressionMode.Decompress);
                    if (decode_results.Equals("gzip"))
                        decompressionStream = new GZipStream(new MemoryStream(tmp), CompressionMode.Decompress);
                    if (decompressionStream != null)
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        decoded_bytes = decompressedFileStream.ToArray();
                    }
                }catch(Exception ex)
                {
                    ConsoleWrite("decodeCommand: Run: {0}", ex.Message);
                }
            }
            if (decoded_bytes != null)
                ConsoleWrite(string.Join("", decoded_bytes.Select(b => (char)b).ToArray()));
        }
    }
}