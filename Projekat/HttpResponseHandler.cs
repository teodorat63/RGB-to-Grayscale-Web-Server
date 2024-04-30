using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Projekat
{
    internal class HttpResponseHandler
    {
        public string GenerateResponse(string request)
        {
            return "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<h1>Zdravo, uspešno si se povezao na server!</h1>";
        }

        public async Task SendResponseAsync(string response, NetworkStream stream)
        {
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseData, 0, responseData.Length);
        }

        //improve
        public async Task SendImageResponse(byte[] imageData, NetworkStream stream)
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 200 OK");
            responseBuilder.AppendLine("Content-Type: image/jpeg"); // Change content type according to your image type
            responseBuilder.AppendLine($"Content-Length: {imageData.Length}");
            responseBuilder.AppendLine();
            byte[] responseHeader = Encoding.UTF8.GetBytes(responseBuilder.ToString());

            await stream.WriteAsync(responseHeader, 0, responseHeader.Length);
            await stream.WriteAsync(imageData, 0, imageData.Length);
        }

        public void SendResponse(string response, NetworkStream stream)
        {
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);
        }
    }
}
