using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;




namespace Projekat
{
    internal class ImageService
    {
        private MemoryCache cache;

        public ImageService()
        {
            cache = MemoryCache.Default;
        }


        public async Task ServeImageAsync(string request, NetworkStream stream)
        {
            try
            {
                string[] parts = request.Split(' ');
                string filename = parts[1].Substring(1); // Remove the leading slash
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

                        // Send the grayscale image response
                        HttpResponseHandler responseHandler = new HttpResponseHandler();
                        responseHandler.SendImageResponse(outputData, stream);
                    }

                    // Dispose resources
                    image.Dispose();
                    grayscaleImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serving image: {ex.Message}");
                string notFoundResponse = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n404 Not Found";
                byte[] notFoundData = Encoding.UTF8.GetBytes(notFoundResponse);
                await stream.WriteAsync(notFoundData, 0, notFoundData.Length);
            }
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

                        // Send the grayscale image response
                        HttpResponseHandler responseHandler = new HttpResponseHandler();
                        responseHandler.SendImageResponse(outputData, stream);
                    }

                    // Dispose resources
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

            using (Graphics g = Graphics.FromImage(grayscaleImage))
            {
                // Create the grayscale color matrix
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                        new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                        new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });

                // Create the ImageAttributes object and set the color matrix
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);

                    // Draw the image with the grayscale color matrix
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                        0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }

            return grayscaleImage;
        }
    }
}
