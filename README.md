# RGB to Grayscale Web Server

## Description:
This repository contains a web server implemented in C# as a console application. The server is capable of converting images from RGB to grayscale format upon receiving requests via HTTP GET method from a web browser.
Users specify the filename of the image as a parameter in the request URL.

## Features:
- Implements a console-based web server using .NET Framework.
- Converts images to grayscale format.
- Logs all incoming requests.
- Utilizes functionality from the System.Threading library, including synchronization mechanisms and ThreadPool.

## Usage:
Once the server is running, it will listen for incoming TCP connections on the specified IP address and port.
Clients can connect to the server using a web browser. 
The server supports HTTP GET requests for both web pages and images. 
When requesting images, users specify the filename in the request URL, and the server converts the image to grayscale before serving it to the client.

## Note
This project is submitted as part of the assignment covering multithreaded programming using .NET Framework.

## Dependencies:
This project relies on the following dependencies:
- .NET Framework: The project is developed using C# and requires the .NET Framework to run.
- System.Drawing: Used for image processing and manipulation.
- System.Net.Sockets: Provides classes for network communication.
- System.Threading: Used for multi-threading to handle multiple client connections simultaneously.
- System.Runtime.Caching: Used for caching grayscale images to improve performance.

## Future Improvements:
Some potential enhancements for this project include:
- Enhancing error handling and logging to provide better feedback to clients.
- Optimizing image processing algorithms for better performance.


