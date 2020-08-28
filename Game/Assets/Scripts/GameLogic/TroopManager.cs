using System.Collections.Generic;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly Score score = new Score();
        protected TroopMap TroopMap = new TroopMap();
        private PlayerSide activePlayer = PlayerSide.Red;

        
        public virtual void ResetForNewGame(Board board)
        {
            foreach (Troop troop in TroopMap.Troops) 
                troop.CleanUpSelf();
            TroopMap = new TroopMap();
        }
        
        public virtual void BeginNextRound(IEnumerable<Troop> troops)
        {
            TroopMap.SpawnWave(troops);
            ChangeActivePlayer();
        }

        private void ChangeActivePlayer()
        {
            HashSet<Troop> beginningTroops = TroopMap.GetTroops(activePlayer.Opponent());
            foreach (Troop troop in beginningTroops)
                troop.ResetMovePoints();

            activePlayer = activePlayer.Opponent();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            Troop troop = TroopMap.Get(position);
            troop.MoveInDirection(direction);

            Troop encounter = TroopMap.Get(troop.Position);
            if (encounter == null)
            {
                TroopMap.AdjustPosition(troop);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop();
            
            while ((encounter = TroopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);

                troop.FlyOverOtherTroop();
            }

            if (troop.Health > 0)
                TroopMap.AdjustPosition(troop);
        }

        private void ApplyDamage(Troop troop)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            troop.ApplyDamage();
            if (troop.Health <= 0)
                DestroyTroop(troop);
        }

        private void DestroyTroop(Troop troop)
        {
            TroopMap.Remove(troop);
        }
    }
}
