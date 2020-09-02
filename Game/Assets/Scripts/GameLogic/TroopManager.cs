using System.Collections.Generic;
using Planes262.GameLogic.Troops;
using Planes262.GameLogic.Utils;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly Score score;
        private readonly TroopMap troopMap;
        private PlayerSide activePlayer = PlayerSide.Red;


        public TroopManager(TroopMap troopMap, Score score)
        {
            this.troopMap = troopMap;
            this.score = score;
        }

        public void ResetForNewGame()
        {
            foreach (Troop troop in troopMap.Troops) 
                troop.CleanUpSelf();
            troopMap.ResetForNewGame();
            score.Reset();
        }
        
        public virtual void BeginNextRound(IEnumerable<Troop> troops)
        {
            foreach (Troop troop in troops) troop.Inject(score);
            troopMap.SpawnWave(troops);
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (Troop troop in beginningTroops)
                troop.ResetMovePoints();
            activePlayer = activePlayer.Opponent();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            Troop troop = troopMap.Get(position);
            troop.MoveInDirection(direction);

            Troop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop);
                if (result.DefenderDamaged) ApplyDamage(encounter);

                troop.FlyOverOtherTroop();
            }

            if (troop.Health > 0)
                troopMap.AdjustPosition(troop);
        }

        private void ApplyDamage(Troop troop)
        {
            PlayerSide opponent = troop.Player.Opponent();
            score.Increment(opponent);

            troop.ApplyDamage();
            if (troop.Health <= 0)
                troopMap.Remove(troop);
        }
    }
}
