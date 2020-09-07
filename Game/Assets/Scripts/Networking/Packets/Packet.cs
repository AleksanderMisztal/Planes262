using System;
using System.Collections.Generic;
using System.Text;
using Planes262.GameLogic.Area;
using Planes262.GameLogic.Data;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;

namespace Planes262.Networking.Packets
{
    public class Packet : IDisposable
    {
        private List<byte> buffer;
        private byte[] readableBuffer;
        private int readPos;

        public Packet(int id)
        {
            buffer = new List<byte>();
            readPos = 0;

            Write(id);
        }

        public Packet(byte[] data)
        {
            buffer = new List<byte>();
            readPos = 0;
            SetBytes(data);
        }

        #region Functions

        private void SetBytes(byte[] data)
        {
            Write(data);
            readableBuffer = buffer.ToArray();
        }

        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        #endregion

        #region Write Data

        private void Write(byte[] value)
        {
            buffer.AddRange(value);
        }

        public void Write(int value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            Write(value.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(value));
        }

        public void Write(VectorTwo value)
        {
            Write(value.X);
            Write(value.Y);
        }

        #endregion

        #region Read Data

        public int ReadInt(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'int'!");
            int value = BitConverter.ToInt32(readableBuffer, readPos);
            if (moveReadPos) readPos += 4;
            return value;
        }

        public bool ReadBool(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'bool'!");
            bool value = BitConverter.ToBoolean(readableBuffer, readPos);
            if (moveReadPos) readPos += 1;
            return value;
        }

        public string ReadString(bool moveReadPos = true)
        {
            try
            {
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
                if (moveReadPos && value.Length > 0) readPos += length;
                return value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        public VectorTwo ReadVector2Int()
        {
            int x = ReadInt();
            int y = ReadInt();

            return new VectorTwo(x, y);
        }

        private Troop ReadTroop()
        {
            PlayerSide side = (PlayerSide)ReadInt();
            int health = ReadInt();
            int initialMovePoints = ReadInt();
            int orientation = ReadInt();
            VectorTwo position = ReadVector2Int();

            return new Troop(side, initialMovePoints, position, orientation, health);
        }

        public List<Troop> ReadTroops()
        {
            int length = ReadInt();
            List<Troop> troops = new List<Troop>();
            for (int i = 0; i < length; i++)
            {
                Troop troop = ReadTroop();
                troops.Add(troop);
            }
            return troops;
        }

        private BattleResult ReadBattleResult()
        {
            bool attackerDamaged = ReadBool();
            bool defenderDamaged = ReadBool();

            return new BattleResult(defenderDamaged, attackerDamaged);
        }

        public List<BattleResult> ReadBattleResults()
        {
            int length = ReadInt();
            List<BattleResult> battleResults = new List<BattleResult>();
            for (int i = 0; i < length; i++)
            {
                BattleResult battleResult = ReadBattleResult();
                battleResults.Add(battleResult);
            }
            return battleResults;
        }

        public Board ReadBoard()
        {
            int xMax = ReadInt();
            int yMax = ReadInt();

            return new Board(xMax, yMax);
        }
        #endregion

        private bool disposed;
        
        public void Dispose()
        {
            if (disposed) return;
            buffer = null;
            readableBuffer = null;
            readPos = 0;
            disposed = true;
            
            GC.SuppressFinalize(this);
        }
    }
}