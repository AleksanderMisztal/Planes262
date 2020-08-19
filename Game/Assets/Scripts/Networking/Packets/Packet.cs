using System;
using System.Collections.Generic;
using System.Text;
using Planes262.GameLogic;
using Planes262.GameLogic.Utils;

namespace Planes262.Networking.Packets
{
    public class Packet : IDisposable
    {
        private List<byte> buffer;
        private byte[] readableBuffer;
        private int readPos;

        public Packet()
        {
            buffer = new List<byte>();
            readPos = 0;
        }

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
        public void SetBytes(byte[] data)
        {
            Write(data);
            readableBuffer = buffer.ToArray();
        }

        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public void InsertInt(int value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(value));
        }

        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        public int Length()
        {
            return buffer.Count;
        }

        public int UnreadLength()
        {
            return Length() - readPos;
        }

        public void Reset(bool shouldReset = true)
        {
            if (shouldReset)
            {
                buffer.Clear();
                readableBuffer = null;
                readPos = 0;
            }
            else
            {
                readPos -= 4;
            }
        }
        #endregion

        #region Write Data
        public void Write(byte value)
        {
            buffer.Add(value);
        }

        public void Write(byte[] value)
        {
            buffer.AddRange(value);
        }

        public void Write(short value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(int value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(long value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(float value)
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

        public void Write(Troop value)
        {
            Write((int)value.Player);
            Write(value.Health);
            Write(value.InitialMovePoints);
            Write(value.Orientation);
            Write(value.Position);
        }

        public void Write(BattleResult value)
        {
            Write(value.AttackerDamaged);
            Write(value.DefenderDamaged);
        }

        public void Write(Board value)
        {
            Write(value.XMax);
            Write(value.YMax);
        }
        #endregion

        #region Read Data
        public byte ReadByte(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'byte'!");
            var value = readableBuffer[readPos];
            if (moveReadPos) readPos += 1;
            return value;
        }

        public byte[] ReadBytes(int length, bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'byte[]'!");
            var value = buffer.GetRange(readPos, length).ToArray();
            if (moveReadPos) readPos += length;
            return value;
        }

        public short ReadShort(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'short'!");
            var value = BitConverter.ToInt16(readableBuffer, readPos);
            if (moveReadPos) readPos += 2;
            return value;
        }

        public int ReadInt(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'int'!");
            var value = BitConverter.ToInt32(readableBuffer, readPos);
            if (moveReadPos) readPos += 4;
            return value;
        }

        public long ReadLong(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'long'!");
            var value = BitConverter.ToInt64(readableBuffer, readPos);
            if (moveReadPos) readPos += 8;
            return value;
        }

        public float ReadFloat(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'float'!");
            var value = BitConverter.ToSingle(readableBuffer, readPos);
            if (moveReadPos) readPos += 4;
            return value;
        }

        public bool ReadBool(bool moveReadPos = true)
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'bool'!");
            var value = BitConverter.ToBoolean(readableBuffer, readPos);
            if (moveReadPos) readPos += 1;
            return value;
        }

        public string ReadString(bool moveReadPos = true)
        {
            try
            {
                var length = ReadInt();
                var value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
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
            var x = ReadInt();
            var y = ReadInt();

            return new VectorTwo(x, y);
        }

        public Troop ReadTroop()
        {
            var side = (PlayerSide)ReadInt();
            var health = ReadInt();
            var initialMovePoints = ReadInt();
            var orientation = ReadInt();
            var position = ReadVector2Int();

            return new Troop(side, initialMovePoints, position, orientation, health);
        }

        public List<Troop> ReadTroops()
        {
            var length = ReadInt();
            var troops = new List<Troop>();
            for (var i = 0; i < length; i++)
            {
                var troop = ReadTroop();
                troops.Add(troop);
            }
            return troops;
        }

        public BattleResult ReadBattleResult()
        {
            var attackerDamaged = ReadBool();
            var defenderDamaged = ReadBool();

            return new BattleResult(defenderDamaged, attackerDamaged);
        }

        public List<BattleResult> ReadBattleResults()
        {
            var length = ReadInt();
            var battleResults = new List<BattleResult>();
            for (var i = 0; i < length; i++)
            {
                var battleResult = ReadBattleResult();
                battleResults.Add(battleResult);
            }
            return battleResults;
        }

        public Board ReadBoard()
        {
            var xMax = ReadInt();
            var yMax = ReadInt();

            return new Board(xMax, yMax);
        }
        #endregion

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}