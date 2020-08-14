using Scripts.Networking;

namespace GameServer.GameLogic.ServerEvents
{
    public interface IGameEvent
    {
        Packet GetPacket();
        string GetString();
    }
}
