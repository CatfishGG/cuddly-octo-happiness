using System.IO;
using System.Net;
using System.Text;

HttpListener server = new();
server.Prefixes.Add("http://localhost:1337/");
server.Start();
Console.WriteLine("Server is listening...");

while (true)
{
    HttpListenerContext context = server.GetContext(); // Block until a connection comes in
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    string url = request.RawUrl ?? "";
    string filePath = "";

    if (url.EndsWith(".css"))
    {
        response.ContentType = "text/css";
        filePath = "/path/to/your/css" + url;
    }
    else if (url.EndsWith(".js"))
    {
        response.ContentType = "text/javascript";
        filePath = "/path/to/your/js" + url;
    }
    else
    {
        response.ContentType = "text/html";
        filePath = "/path/to/your/html" + url;
    }

    if (File.Exists(filePath))
    {
        string responseString = File.ReadAllText(filePath);
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length); // Send the response
    }
    else
    {
        response.StatusCode = 404; // File not found, send 404 status
    }

    response.Close(); // Close the response to send it to the client
}