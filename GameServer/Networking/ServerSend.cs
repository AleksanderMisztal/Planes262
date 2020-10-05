using System;
using System.Threading.Tasks;
using GameDataStructures;
using GameDataStructures.Packets;
using GameJudge.GameEvents;
using GameJudge.Troops;

namespace GameServer.Networking
{
    public class ServerSend
    {
        private readonly Server server;

        public ServerSend(Server server)
        {
            this.server = server;
        }

        public async Task Welcome(int toClient)
        {
            Packet packet = new Packet(ServerPackets.Welcome);
            await server.SendPacket(toClient, packet);
        }

        
        public async Task GameJoined(int toClient, string opponentName, PlayerSide side, Board board, ClockInfo clockInfo)
        {
            Packet packet = new Packet(ServerPackets.GameJoined);

            packet.Write(opponentName);
            packet.Write((int)side);
            packet.Write(board);
            packet.Write(clockInfo);

            await server.SendPacket(toClient, packet);
        }

        public async Task MessageSent(int toClient, string message)
        {
            Packet packet = new Packet(ServerPackets.MessageSent);
            packet.Write(message);
            await server.SendPacket(toClient, packet);
        }

        public async Task OpponentDisconnected(int toClient)
        {
            Packet packet = new Packet(ServerPackets.OpponentDisconnected);
            await server.SendPacket(toClient, packet);
        }

        public async Task LostOnTime(int redId, int blueId, PlayerSide loser)
        {
            Console.WriteLine($"Sending {loser} lost on time");
            Packet packet = new Packet(ServerPackets.LostOnTime);
            packet.Write((int)loser);
            
            await server.SendPacket(redId, packet);
            await server.SendPacket(blueId, packet);
        }

        
        public async Task TroopsSpawned(int redId, int blueId, TroopsSpawnedEventArgs args, TimeInfo timeInfo)
        {
            Packet packet = new Packet(ServerPackets.TroopSpawned);
            
            packet.Write(args.Troops.Count);
            foreach (Troop troop in args.Troops) packet.Write(troop);
            packet.Write(timeInfo);

            await server.SendPacket(redId, packet);
            await server.SendPacket(blueId, packet);
        }

        public async Task TroopMoved(int redId, int blueId, TroopMovedEventArgs args)
        {
            Packet packet = new Packet(ServerPackets.TroopMoved);
            packet.Write(args.Position);
            packet.Write(args.Direction);
            packet.Write(args.BattleResults);
            packet.Write(args.Score);

            await server.SendPacket(redId, packet);
            await server.SendPacket(blueId, packet);
        }

        public async Task GameEnded(int redId, int blueId, GameEndedEventArgs args)
        {
            Packet packet = new Packet(ServerPackets.GameEnded);
            packet.Write(args.Score.Red);
            packet.Write(args.Score.Blue);

            await server.SendPacket(redId, packet);
            await server.SendPacket(blueId, packet);
        }
    }
}
