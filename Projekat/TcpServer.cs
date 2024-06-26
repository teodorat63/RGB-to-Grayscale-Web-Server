﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Projekat
{
    internal class TcpServer
    {
        private TcpListener tcpListener;
        private List<RequestInfo> receivedRequests;
        private readonly object requestLock = new object();


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
                    Stopwatch stopwatch = new Stopwatch();

                    try
                    {
                        string request = requestHandler.ReadRequest(stream);

                        lock (requestLock)
                        {
                            newRequest.request = request;
                            receivedRequests.Add(newRequest);
                        }

                        if (requestHandler.isIndexPageRequest(request))
                        {
                            responseHandler.SendResponse("Dobrodosli na server!", stream);
                            newRequest.details = "Successfully accessed main page";

                        }
                        else if (requestHandler.IsValidImageRequest(request))
                        {
                            imageService.ServeImage(request, stream);
                            newRequest.details = "Image successfully served";

                        }

                    }
                    catch (ArgumentException ex)
                    {
                        responseHandler.SendResponse(ex.Message, stream);
                        newRequest.details = "Unsuccessful " + ex.Message;


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        newRequest.details = "Unsuccessful " + ex.Message;


                    }
                    finally
                    {
                         Console.WriteLine(newRequest.ToString()); 
                    }
                }
            }
        }

       
    }
}
