using System.Collections.Generic;
using System.Linq;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
using Planes262.UnityLayer;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly TroopMap troopMap;
        private readonly ITroopInstantiator troopInstantiator;
        private PlayerSide activePlayer = PlayerSide.Red;

        public TroopManager(TroopMap troopMap, ITroopInstantiator troopInstantiator)
        {
            this.troopMap = troopMap;
            this.troopInstantiator = troopInstantiator;
        }

        public void ResetForNewGame()
        {
            foreach (ITroop troop in troopMap.Troops) 
                troop.CleanUpSelf();
            troopMap.ResetForNewGame();
        }
        
        public void BeginNextRound(IEnumerable<ITroop> troops)
        {
            IEnumerable<ITroop> uTroops = troops.Select(t => troopInstantiator.InstantiateTroop(t));
            troopMap.SpawnWave(uTroops);
            
            HashSet<ITroop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (ITroop troop in beginningTroops)
                troop.ResetMovePoints();
            
            activePlayer = activePlayer.Opponent();
        }

        public void MoveTroop(VectorTwo position, int direction, List<BattleResult> battleResults)
        {
            int battleId = 0;
            ITroop troop = troopMap.Get(position);
            VectorTwo startingPosition = troop.Position;
            troop.MoveInDirection(direction);

            ITroop encounter = troopMap.Get(troop.Position);
            if (encounter == null)
            {
                troopMap.AdjustPosition(troop, startingPosition);
                return;
            }
            BattleResult result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
            if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && !troop.Destroyed)
            {
                result = battleResults[battleId++];
                if (result.AttackerDamaged) ApplyDamage(troop, startingPosition);
                if (result.DefenderDamaged) ApplyDamage(encounter, encounter.Position);

                troop.FlyOverOtherTroop();
            }

            if (!troop.Destroyed)
                troopMap.AdjustPosition(troop, startingPosition);
        }

        private void ApplyDamage(ITroop troop, VectorTwo startingPosition)
        {
            troop.ApplyDamage();
            if (troop.Destroyed)
                troopMap.Remove(troop, startingPosition);
        }
    }

    public interface ITroopInstantiator
    {
        ITroop InstantiateTroop(ITroop troop);
    }
}
