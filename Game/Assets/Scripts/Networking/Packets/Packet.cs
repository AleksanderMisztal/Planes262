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

        public Packet(int _id)
        {
            buffer = new List<byte>();
            readPos = 0;

            Write(_id);
        }

        public Packet(byte[] _data)
        {
            buffer = new List<byte>();
            readPos = 0;

            SetBytes(_data);
        }

        #region Functions
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }

        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public void InsertInt(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
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

        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
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
        public void Write(byte _value)
        {
            buffer.Add(_value);
        }

        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }

        public void Write(short _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(int _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(long _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(float _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(bool _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(string _value)
        {
            Write(_value.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(_value));
        }

        public void Write(VectorTwo _value)
        {
            Write(_value.X);
            Write(_value.Y);
        }

        public void Write(Troop _value)
        {
            Write((int)_value.Player);
            Write(_value.Health);
            Write(_value.InitialMovePoints);
            Write(_value.Orientation);
            Write(_value.Position);
        }

        public void Write(BattleResult _value)
        {
            Write(_value.AttackerDamaged);
            Write(_value.DefenderDamaged);
        }

        public void Write(Board _value)
        {
            Write(_value.xMax);
            Write(_value.yMax);
        }
        #endregion

        #region Read Data
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = readableBuffer[readPos];
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = buffer.GetRange(readPos, _length).ToArray();
                if (_moveReadPos)
                {
                    readPos += _length;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = BitConverter.ToInt16(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 2;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = BitConverter.ToInt32(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = BitConverter.ToInt64(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 8;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = BitConverter.ToSingle(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                var _value = BitConverter.ToBoolean(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                var _length = ReadInt();
                var _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length);
                if (_moveReadPos && _value.Length > 0)
                {
                    readPos += _length;
                }
                return _value;
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

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}