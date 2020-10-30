using System;
using System.Collections.Generic;
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

        public void Welcome(int toClient)
        {
            Packet packet = new Packet(ServerPackets.Welcome);
            server.SendPacket(toClient, packet);
        }

        
        public void GameJoined(int toClient, string opponentName, PlayerSide side, Board board, List<Troop> troops, ClockInfo clockInfo)
        {
            Packet packet = new Packet(ServerPackets.GameJoined);

            packet.Write(opponentName);
            packet.Write((int)side);
            packet.Write(board);
            packet.Write(troops.Count);
            foreach (Troop troop in troops) packet.Write(troop);
            packet.Write(clockInfo);

            server.SendPacket(toClient, packet);
        }

        public void MessageSent(int toClient, string message)
        {
            Packet packet = new Packet(ServerPackets.MessageSent);
            packet.Write(message);
            server.SendPacket(toClient, packet);
        }

        public void OpponentDisconnected(int toClient)
        {
            Packet packet = new Packet(ServerPackets.OpponentDisconnected);
            server.SendPacket(toClient, packet);
        }

        public void LostOnTime(int redId, int blueId, PlayerSide loser)
        {
            Console.WriteLine($"Sending {loser} lost on time");
            Packet packet = new Packet(ServerPackets.LostOnTime);
            packet.Write((int)loser);
            
            server.SendPacket(redId, packet);
            server.SendPacket(blueId, packet);
        }

        
        public void TroopsSpawned(int redId, int blueId, TroopsSpawnedEventArgs args, TimeInfo timeInfo)
        {
            Packet packet = new Packet(ServerPackets.TroopSpawned);
            
            packet.Write(args.troops.Count);
            foreach (Troop troop in args.troops) packet.Write(troop);
            packet.Write(timeInfo);

            server.SendPacket(redId, packet);
            server.SendPacket(blueId, packet);
        }

        public void TroopMoved(int redId, int blueId, TroopMovedEventArgs args)
        {
            Packet packet = new Packet(ServerPackets.TroopMoved);
            packet.Write(args.position);
            packet.Write(args.direction);
            packet.Write(args.battleResults.Count);
            foreach (BattleResult battleResult in args.battleResults) packet.Write(battleResult);
            packet.Write(args.score);

            server.SendPacket(redId, packet);
            server.SendPacket(blueId, packet);
        }

        public void GameEnded(int redId, int blueId, GameEndedEventArgs args)
        {
            Packet packet = new Packet(ServerPackets.GameEnded);
            packet.Write(args.score.Red);
            packet.Write(args.score.Blue);

            server.SendPacket(redId, packet);
            server.SendPacket(blueId, packet);
        }
    }
}
