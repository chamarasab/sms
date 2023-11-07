using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GsmComm.GsmCommunication;
using Newtonsoft.Json;
using GsmComm.PduConverter;
using System.IO.Ports;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using sms;

namespace sms
{
    class Program
    {
        private static async Task HandleWebSocketClientAsync(HttpListenerContext context)
        {
            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                WebSocket webSocket = webSocketContext.WebSocket;
                Console.WriteLine("WebSocket connection established");

                try
                {
                    byte[] buffer = new byte[1024];
                    while (webSocket.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            Console.WriteLine($"Received JSON: {receivedMessage}");

                            // Parse the received JSON
                            JsonData data = JsonConvert.DeserializeObject<JsonData>(receivedMessage);

                            GSMsms sms = new GSMsms();

                            //sms.Search();
                            sms.Connect();
                            //Console.WriteLine(sms.IsConnected);
                            //sms.Disconnect();
                           
                            if (sms.IsConnected)
                            {
                                //sms.Read();
                                sms.Send(data.mobile, data.response); 
                            }

                            Console.Read();
                        }
                    }
                }
                catch (WebSocketException)
                {
                    // Handle WebSocket exceptions here
                }
                finally
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
                    }
                    webSocket.Dispose();
                }
            }
        }

        static async Task Main(string[] args)
        {
            //WebSocket and HTTP listener setup code here
            const string serverAddress = "http://localhost:8080/";
            using HttpListener listener = new HttpListener();
            listener.Prefixes.Add(serverAddress);

            listener.Start();
            Console.WriteLine($"Server listening on {serverAddress}");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    await HandleWebSocketClientAsync(context);
                }
                else
                {
                    // Handle other HTTP requests here if needed
                }
            }
        }
    }
}
