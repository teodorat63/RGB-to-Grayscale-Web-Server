using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace Projekat
{
    internal class ImageService
    {
        private MemoryCache cache;

        public ImageService()
        {
            cache = MemoryCache.Default;
        }

        public void ServeImage(string request, NetworkStream stream)
        {
            try
            {
                string[] parts = request.Split(' ');
                string filename = parts[1].Substring(1); // Remove the leading slash

                // Check if the grayscale image is cached
                byte[] cachedGrayscaleImage = cache.Get(filename) as byte[];
                if (cachedGrayscaleImage != null)
                {
                    // If cached, send the cached grayscale image
                    HttpResponseHandler responseHandler = new HttpResponseHandler();
                    responseHandler.SendImageResponse(cachedGrayscaleImage, stream);
                    return;
                }

                // If not cached, read the original image from file
                byte[] imageData = System.IO.File.ReadAllBytes(filename);

                // Load the image
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);

                    // Convert the image to grayscale
                    Bitmap grayscaleImage = ConvertToGrayscale(image);

                    // Save the grayscale image to a memory stream
                    using (MemoryStream outputMs = new MemoryStream())
                    {
                        grayscaleImage.Save(outputMs, ImageFormat.Jpeg);
                        byte[] outputData = outputMs.ToArray();

                        // Cache the grayscale image
                        cache.Set(filename, outputData, DateTimeOffset.Now.AddMinutes(10)); // Cache for 10 minutes

                        HttpResponseHandler responseHandler = new HttpResponseHandler();
                        responseHandler.SendImageResponse(outputData, stream);
                    }

                    image.Dispose();
                    grayscaleImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serving image: {ex.Message}");
                string notFoundResponse = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n404 Not Found";
                byte[] notFoundData = Encoding.UTF8.GetBytes(notFoundResponse);
                stream.Write(notFoundData, 0, notFoundData.Length);
            }
        }

        private Bitmap ConvertToGrayscale(Image image)
        {
            Bitmap grayscaleImage = new Bitmap(image.Width, image.Height);

            int numThreads = Environment.ProcessorCount;
            int stripHeight = image.Height / numThreads;

            Thread[] threads = new Thread[numThreads];

            for (int i = 0; i < numThreads; i++)
            {
                int startY = i * stripHeight;
                int endY = (i == numThreads - 1) ? image.Height : (i + 1) * stripHeight;

                Image imageCopy = new Bitmap(image); // Create a copy of the original image for each thread

                threads[i] = new Thread(() =>
                {
                    ConvertStripToGrayscale(imageCopy, grayscaleImage, startY, endY);
                });

                threads[i].Start();
            }

            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return grayscaleImage;
        }

        private void ConvertStripToGrayscale(Image image, Bitmap grayscaleImage, int startY, int endY)
        {
            for (int y = startY; y < endY; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = ((Bitmap)image).GetPixel(x, y);
                    int grayscaleValue = (int)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);
                    Color grayscaleColor = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);

                    lock (grayscaleImage)
                    {
                        grayscaleImage.SetPixel(x, y, grayscaleColor);
                    }
                }
            }
        }

    }
}
