using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace Projekat
{
    internal class TcpServer
    {
        private TcpListener tcpListener;
        private List<string> receivedRequests;

        public TcpServer(IPAddress ipAddress, int port)
        {
            tcpListener = new TcpListener(ipAddress, port);
            receivedRequests = new List<string>();
        }
        public void StartServer()
        {
            tcpListener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected...");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();

            }
        }

        private void HandleClient(TcpClient client)
        {

            using (client)
            {

                using (NetworkStream stream = client.GetStream())
                {

                    Console.WriteLine(stream.ToString());

                    HttpRequestHandler requestHandler = new HttpRequestHandler();
                    HttpResponseHandler responseHandler = new HttpResponseHandler();
                    ImageService imageService = new ImageService();

                    try
                    {
                        string request = requestHandler.ReadRequest(stream);
                        receivedRequests.Add(request);

                        if (requestHandler.IsValidImageRequest(request))
                        {
                            imageService.ServeImage(request, stream);
                        }
                        else
                        {
                            responseHandler.SendResponse("Dobrodosli na server!", stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
            }
        }

        //private async Task HandleClientAsync(TcpClient client)
        //{
        //    // The 'using' block ensures that the 'client' object is disposed of properly
        //    // when it's no longer needed.
        //    using (client)
        //    {
        //        // Inside the 'using' block for 'client', a 'NetworkStream' object is created.
        //        // The 'using' block ensures that the 'stream' object is disposed of properly
        //        // when it's no longer needed.
        //        using (NetworkStream stream = client.GetStream())
        //        {

        //            Console.WriteLine(stream.ToString());

        //            HttpRequestHandler requestHandler = new HttpRequestHandler();
        //            HttpResponseHandler responseHandler = new HttpResponseHandler();
        //            ImageService imageService = new ImageService();

        //            try 
        //            {
        //                string request = await requestHandler.ReadRequestAsync(stream);
        //                receivedRequests.Add(request);

        //                if (requestHandler.IsValidImageRequest(request))
        //                {
        //                    await imageService.ServeImageAsync(request, stream);
        //                }
        //                else
        //                {
        //                    string response = responseHandler.GenerateResponse(request);
        //                    await responseHandler.SendResponseAsync(response, stream);
        //                }
        //            }
        //            catch(Exception ex)
        //            {

        //            }
        //        } // The 'stream' object is disposed of automatically when it goes out of scope.
        //    } // The 'client' object is disposed of automatically when it goes out of scope.
        //}
    }
}
