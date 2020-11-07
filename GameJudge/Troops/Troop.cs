using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Packets;
using GameDataStructures.Positioning;

namespace GameJudge.Troops
{
    public abstract class Troop : ITroop, IReadable, IWriteable
    {
        public PlayerSide Player { get; private set; }
        public abstract TroopType Type { get; protected set; }

        protected int initialMovePoints;
        public int MovePoints { get; private set; }

        public VectorTwo Position { get; private set; }
        public int Orientation { get; private set; }

        public int Health { get; private set; }
        public bool Destroyed => Health <= 0;

        protected Troop(PlayerSide player, int movePoints, VectorTwo position, int orientation, int health)
        {
            Player = player;
            initialMovePoints = movePoints;
            MovePoints = movePoints;
            Position = position;
            Orientation = orientation;
            Health = health;
        }
        
        protected Troop() { }

        internal void AdjustPosition(VectorTwo newPosition)
        {
            Position = newPosition;
        }

        public virtual void MoveInDirection(int direction)
        {
            MovePoints--;
            Orientation = (6 + Orientation + direction) % 6;
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public void FlyOverOtherTroop()
        {
            Position = Hex.GetAdjacentHex(Position, Orientation);
        }

        public void ApplyDamage()
        {
            Health--;
            initialMovePoints--;
            if (MovePoints > 0)
                MovePoints--;
        }

        public virtual IEnumerable<VectorTwo> ControlZone => Hex.GetControlZone(Position, Orientation);

        public bool InControlZone(VectorTwo position) => ControlZone.Any(cell => cell == position);

        public void ResetMovePoints()
        {
            MovePoints = initialMovePoints;
        }

        public virtual void CleanUpSelf() { }

        public IReadable Read(string s) => Read(s, out _);

        protected virtual IReadable Read(string s, out List<string> otherArgs)
        {
            List<string> args = Merger.Split(s);

            int id = 0;
            Type = (TroopType) int.Parse(args[id++]);
            Troop troop;
            switch (Type)
            {
                case TroopType.Fighter:
                    troop = new Fighter();
                    break;
                case TroopType.Bomber:
                    troop = new Bomber();
                    break;
                case TroopType.Flak:
                    troop = new Flak();
                    break;
                default:
                    troop = new Fighter();
                    MyLogger.Log("Troop type not found!");
                    break;
            }
            troop.Player = (PlayerSide) int.Parse(args[id++]);
            troop.MovePoints = initialMovePoints = int.Parse(args[id++]);
            troop.Position = (VectorTwo)new VectorTwo().Read(args[id++]);
            troop.Orientation = int.Parse(args[id++]);
            troop.Health = int.Parse(args[id++]);

            otherArgs = args.GetRange(id, args.Count - id);
            
            return troop;
        }

        protected virtual Merger Merger => new Merger()
            .Write((int)Type)
            .Write((int) Player)
            .Write(initialMovePoints)
            .Write(Position)
            .Write(Orientation)
            .Write(Health);

        public string Data => Merger.Data;
        
        public abstract Troop Copy();
    }
}
