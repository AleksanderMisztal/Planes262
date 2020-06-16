using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

public class WsClient
{
    // WebSocket
    private readonly ClientWebSocket ws;
    private readonly UTF8Encoding encoder; // For websocket text message encoding.
    private const ulong MAXREADSIZE = 1 * 1024 * 1024;

    // Server address
    private readonly Uri serverUri;

    // Queues
    public ConcurrentQueue<string> ReceiveQueue { get; }
    public BlockingCollection<ArraySegment<byte>> SendQueue { get; }

    // Threads
    private Thread ReceiveThread { get; set; }
    private Thread SendThread { get; set; }

    public WsClient(string serverURL)
    {
        encoder = new UTF8Encoding();
        ws = new ClientWebSocket();

        serverUri = new Uri(serverURL);

        ReceiveQueue = new ConcurrentQueue<string>();
        ReceiveThread = new Thread(RunReceive);
        ReceiveThread.Start();

        SendQueue = new BlockingCollection<ArraySegment<byte>>();
        SendThread = new Thread(RunSend);
        SendThread.Start();
    }

    public async Task Connect()
    {
        Debug.Log("Connecting to: " + serverUri);
        await ws.ConnectAsync(serverUri, CancellationToken.None);
        Debug.Log("Connect status: " + ws.State);
    }

    #region [Send]

    public void Send(string message)
    {
        byte[] buffer = encoder.GetBytes(message);
        var sendBuf = new ArraySegment<byte>(buffer);

        SendQueue.Add(sendBuf);
    }

    private async void RunSend()
    {
        ArraySegment<byte> msg;
        while (true)
        {
            while (!SendQueue.IsCompleted)
            {
                msg = SendQueue.Take();
                await ws.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    #endregion

    #region [Receive]

    private async Task<string> Receive(UInt64 maxSize = MAXREADSIZE)
    {
        byte[] buf = new byte[4 * 1024];
        var memoryStream = new MemoryStream();
        ArraySegment<byte> arrayBuf = new ArraySegment<byte>(buf);
        if (ws.State == WebSocketState.Open)
        {
            WebSocketReceiveResult chunkResult;
            do
            {
                chunkResult = await ws.ReceiveAsync(arrayBuf, CancellationToken.None);
                memoryStream.Write(arrayBuf.Array, arrayBuf.Offset, chunkResult.Count);
                if ((UInt64)(chunkResult.Count) > MAXREADSIZE)
                {
                    Console.Error.WriteLine("Warning: Message is bigger than expected!");
                }
            } while (!chunkResult.EndOfMessage);
            memoryStream.Seek(0, SeekOrigin.Begin);

            if (chunkResult.MessageType == WebSocketMessageType.Text)
            {
                return StreamToString(memoryStream, Encoding.UTF8);
            }
        }
        return "";
    }

    private async void RunReceive()
    {
        string result;
        while (true)
        {
            result = await Receive();
            if (result != null && result.Length > 0)
            {
                ReceiveQueue.Enqueue(result);
            }
            else
            {
                Task.Delay(50).Wait();
            }
        }
    }
    #endregion

    public static string StreamToString(MemoryStream ms, Encoding encoding)
    {
        string readString = "";
        if (encoding == Encoding.UTF8)
        {
            using (var reader = new StreamReader(ms, encoding))
            {
                readString = reader.ReadToEnd();
            }
        }
        return readString;
    }
}