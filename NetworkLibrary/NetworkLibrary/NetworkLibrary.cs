using System.Net;

public class NetworkLibrary
{
    /// <summary>
    /// Create listener -> "http://{adress}/{endpoint}/"
    /// </summary>
    /// <param name="adress"></param>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static HttpListener ServerCreateListenerWithEndpoint(string adress, string endpoint)
    {
        HttpListener listener = new();
        listener.Prefixes.Add($"http://{adress}/{endpoint}/");
        listener.Start();
        return listener;
    }

    /// <summary>
    /// Get context from listener
    /// </summary>
    /// <param name="httpListener"></param>
    /// <returns></returns>
    public static HttpListenerContext ServerGetContextFromListener(HttpListener httpListener)
    {
        HttpListenerContext context = httpListener.GetContext();
        return context;
    }

    /// <summary>
    /// Send a message to client
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    public static void ServerSendMessageBodyToClient(HttpListenerContext context, string message)
    {
        using HttpListenerResponse response = context.Response;
        response.ContentType = "text/plain";

        using Stream output = response.OutputStream;

        using StreamWriter writer = new(output);
        writer.Write(message);

        writer.Flush();

        // Close the writer and output stream to release resources
        writer.Close();
        output.Close();
    }

    /// <summary>
    /// Receive a message from client with index
    /// </summary>
    /// <param name="context"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string ServerReceiveMessageFromQueryStringWithIndex(HttpListenerContext context, int index)
    {
        var qry = context.Request.QueryString;
        string message = qry[index];
        return message;
    }



    /// <summary>
    /// Create an new http client with baseAddress EXAMPLE: "http://{baseAddress}/"
    /// </summary>
    /// <param name="baseAddress"></param>
    /// <returns></returns>
    public static HttpClient ClientCreateHttpClient(string baseAddress)
    {
        HttpClient client = new()
        {
            Timeout = TimeSpan.FromSeconds(5),
            BaseAddress = new Uri($"http://{baseAddress}/")
        };

        return client;
    }

    /// <summary>
    /// Receive a message from client request EXAMPLE: "example/getdate/?apikey=testkey";
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    public static string ClientReceiveMessage(HttpClient httpClient, string endpoint)
    {
        // ENDPOINT EXAMPLE: "example/getdate/?apikey=testkey";

        try
        {
            // Syncronous variation on .Result
            HttpResponseMessage wordResponse = httpClient.GetAsync(endpoint).Result;
            string reveivedMessage = wordResponse.Content.ReadAsStringAsync().Result;
            return reveivedMessage;
        }

        catch { return string.Empty; }
    }
}