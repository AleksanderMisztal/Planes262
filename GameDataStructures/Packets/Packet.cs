using System;
using System.Collections.Generic;
using System.Text;

namespace GameDataStructures.Packets
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

        public void Write(long value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        private void Write(bool value)
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

        public void Write(TroopDto value)
        {
            Write((int)value.Player);
            Write(value.Health);
            Write(value.InitialMovePoints);
            Write(value.Orientation);
            Write(value.Position);
        }

        public void Write(ICollection<TroopDto> troops)
        {
            Write(troops.Count);
            foreach (TroopDto troop in troops) Write(troop);
        }

        public void Write(BattleResult value)
        {
            Write(value.AttackerDamaged);
            Write(value.DefenderDamaged);
        }

        public void Write(ICollection<BattleResult> battleResults)
        {
            Write(battleResults.Count);
            foreach (BattleResult battleResult in battleResults) Write(battleResult);
        }

        public void Write(Board value)
        {
            Write(value.XMax);
            Write(value.YMax);
        }

        public void Write(TimeInfo timeInfo)
        {
            Write(timeInfo.RedTimeMs);
            Write(timeInfo.BlueTimeMs);
            Write(timeInfo.ChangeTimeMs);
        }
        #endregion

        #region Read Data

        public int ReadInt()
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'int'!");
            int value = BitConverter.ToInt32(readableBuffer, readPos);
            readPos += 4;
            return value;
        }

        public long ReadLong()
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'long'!");
            long value = BitConverter.ToInt64(readableBuffer, readPos);
            readPos += 8;
            return value;
        }

        public bool ReadBool()
        {
            if (buffer.Count <= readPos) throw new Exception("Could not read value of type 'bool'!");
            bool value = BitConverter.ToBoolean(readableBuffer, readPos);
            readPos += 1;
            return value;

        }

        public string ReadString()
        {
            try
            {
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
                if (value.Length > 0) readPos += length;
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

        private TroopDto ReadTroop()
        {
            PlayerSide side = (PlayerSide)ReadInt();
            int health = ReadInt();
            int initialMovePoints = ReadInt();
            int orientation = ReadInt();
            VectorTwo position = ReadVector2Int();

            return new TroopDto(initialMovePoints, side, position, orientation, health);
        }

        public List<TroopDto> ReadTroops()
        {
            int length = ReadInt();
            List<TroopDto> troops = new List<TroopDto>();
            for (int i = 0; i < length; i++)
            {
                TroopDto troop = ReadTroop();
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

        public TimeInfo ReadTimeInfo()
        {
            int redTimeMs = ReadInt();
            int blueTimeMs = ReadInt();
            long changeTimeMs = ReadLong();

            return new TimeInfo(redTimeMs, blueTimeMs, changeTimeMs);
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