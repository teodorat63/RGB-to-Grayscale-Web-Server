﻿using System.Net.Sockets;
using System.Text;

namespace Projekat
{
    internal class HttpResponseHandler
    {

        public void SendResponse(string request, NetworkStream stream)
        {
            string send = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<h1>" + request + "</ h1 > ";
            byte[] responseData = Encoding.UTF8.GetBytes(send);
            stream.Write(responseData, 0, responseData.Length);
        }

        public void SendImageResponse(byte[] imageData, NetworkStream stream)
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.AppendLine("HTTP/1.1 200 OK");
            responseBuilder.AppendLine("Content-Type: image/jpeg"); 
            responseBuilder.AppendLine($"Content-Length: {imageData.Length}");
            responseBuilder.AppendLine();
            byte[] responseHeader = Encoding.UTF8.GetBytes(responseBuilder.ToString());

            stream.Write(responseHeader, 0, responseHeader.Length);
            stream.Write(imageData, 0, imageData.Length);
        }
    }
}
