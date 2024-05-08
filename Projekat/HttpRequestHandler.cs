using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Projekat
{
    internal class HttpRequestHandler
    {
        public string ReadRequest(NetworkStream stream)
        {
            // 1. Create a buffer to store the data read from the network stream.
            byte[] buffer = new byte[1024];

            // 2. Read data from the network stream and store the number of bytes read in bytesRead.
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // 3. Convert the bytes read from the buffer to a string using UTF-8 encoding.
            //    Only the actual bytes read (up to bytesRead) are converted to a string.
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);

        }

        public bool isIndexPageRequest (string request)
        {
            string[] parts = request.Split(' ');
            string filename = parts[1].Substring(1); // Remove the leading slash

            if (filename == "")
            {
                return true;
            }
            return false;


        }

        public bool IsValidImageRequest(string request)
        {
            string[] parts = request.Split(' ');
            string filename = parts[1].Substring(1); // Remove the leading slash

            if (!File.Exists(filename))
            {
                throw new ArgumentException("File " + filename + " does not exist");
            }

            string extension = Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                extension == ".gif" || extension == ".bmp")
            {
                Console.WriteLine("Valid image file.");
                return true;
            }
            else
            {
                throw new ArgumentException("File " + filename + " is not a valid image file");

            }

        }
    }
}
