# RGB to Grayscale Web Server

This is the README for the web server project that converts images from RGB format to grayscale. The server accepts requests via a web browser using the GET method. The request includes the filename or image name as a parameter. Upon receiving a request, the server searches the root folder for the requested file and performs the conversion. If the requested file does not exist, an error is returned to the user.


## Features:
- Implements a console-based web server using .NET Framework.
- Converts images to grayscale format.
- Log all incoming requests and information about their processing, including any errors and details about successful processing.
- Cache responses to incoming requests in memory so that a prepared response is forwarded in case of a repeated request.
- Utilizes functionality from the System.Threading library, including synchronization mechanisms and ThreadPool.

## Usage:
Once the server is running, it will listen for incoming TCP connections on the specified IP address and port. Clients can connect to the server using a web browser. The server supports HTTP GET requests for both web pages and images. When requesting images, users specify the filename in the request URL, and the server converts the image to grayscale before serving it to the client.

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
- Optimizing thread usage and ensuring thread safety for improved concurrency.


## Note
This project is submitted as part of the assignment covering multithreaded programming using .NET Framework.
