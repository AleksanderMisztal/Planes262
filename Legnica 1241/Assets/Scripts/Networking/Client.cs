using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Scripts.Utils;
using System.Net;

namespace Scripts.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        private static int dataBufferSize = 4096;

        //IPAddress ip = IPAddress.Parse("52.157.211.20");
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 26950;
        public int myId = 0;
        public TCP tcp;

        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Debug.Log("Client = this...");
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying this...");
                Destroy(this);
            }
        }

        private void Start()
        {
            Debug.Log("Client started...");
            tcp = new TCP();
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            InitializePacketHandlers();
            tcp.Connect();
        }

        private void InitializePacketHandlers()
        {
            packetHandlers = new Dictionary<int, PacketHandler>
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome },
                {(int)ServerPackets.GameJoined, ClientHandle.GameJoined },
                {(int)ServerPackets.TroopSpawned, ClientHandle.TroopSpawned },
                {(int)ServerPackets.TroopMoved, ClientHandle.TroopMoved },
            };
            Debug.Log("Initialized client packet handlers.");
        }


        public class TCP
        {
            public TcpClient socket;
            private NetworkStream stream;
            private Packet receivedData;

            private byte[] receiveBuffer;


            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };
                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();

                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int byteLength = stream.EndRead(_result);
                    if (byteLength <= 0)
                    {
                        //TODO: disconnect
                        return;
                    }
                    byte[] _data = new byte[byteLength];
                    Array.Copy(receiveBuffer, _data, byteLength);

                    receivedData.Reset(HandleData(_data));

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    //TODO: disconnect
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            packetHandlers[_packetId](_packet);
                        }
                    });
                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }
                return _packetLength <= 1;
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"Error while sending data to server via TCP: {ex}");
                }
            }
        }
    }
}
