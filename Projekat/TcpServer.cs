using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Projekat
{
    internal class TcpServer
    {
        private TcpListener tcpListener;
        private List<RequestInfo> receivedRequests;

        public TcpServer(IPAddress ipAddress, int port)
        {
            tcpListener = new TcpListener(ipAddress, port);
            receivedRequests = new List<RequestInfo>();
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

                    HttpRequestHandler requestHandler = new HttpRequestHandler();
                    HttpResponseHandler responseHandler = new HttpResponseHandler();
                    ImageService imageService = new ImageService();
                    RequestInfo newRequest = new RequestInfo();


                    try
                    {
                        string request = requestHandler.ReadRequest(stream);

                        newRequest.request= request;
                        receivedRequests.Add(newRequest);

                        if (requestHandler.IsValidImageRequest(request))
                        {
                            imageService.ServeImage(request, stream);
                            newRequest.successful = "Image successfully served";
                        }

                    }
                    catch (ArgumentNullException)
                    {
                        responseHandler.SendResponse("Dobrodosli na server!", stream);
                        newRequest.successful = "Successfully accessed main page";

                    }
                    catch (ArgumentException ex)
                    {
                        responseHandler.SendResponse(ex.Message, stream);
                        newRequest.successful = "Unsuccessful" + ex.Message;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        newRequest.successful = "Unsuccessful" + ex.Message;

                    }
                    finally
                    {
                        foreach (var request in receivedRequests) { Console.WriteLine(request.ToString()); }
                    }
                }
            }
        }

       
    }
}
