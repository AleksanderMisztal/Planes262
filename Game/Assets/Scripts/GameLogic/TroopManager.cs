using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.GameLogic
{
    public class TroopManager
    {
        private readonly TroopMap troopMap;
        private PlayerSide activePlayer = PlayerSide.Red;

        public TroopManager(TroopMap troopMap)
        {
            this.troopMap = troopMap;
        }

        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            troopMap.SpawnWave(troops);
            activePlayer = activePlayer.Opponent();
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer);
            foreach (Troop troop in beginningTroops) troop.ResetMovePoints();
        }

        public void MoveTroop(VectorTwo position, int direction, BattleResult[] battleResults)
        {
            Troop troop = troopMap.Get(position);
            VectorTwo startingPosition = troop.Position;
            if (troop.Type == TroopType.Flak)
            {
                troop.MoveInDirection(direction);
                troopMap.AdjustPosition(troop, startingPosition);
                return;
            }
            BattleResult result;
            int battleId = 0;


            void DoFightDamages()
            {
                Troop encounter = troopMap.Get(troop.Position);
                if (result.fightResult.attackerDamaged) ApplyDamage(troop, startingPosition);
                if (result.fightResult.defenderDamaged) ApplyDamage(encounter, encounter.Position);
            }

            void DoFlakDamages()
            {
                result = battleResults[battleId++];
                foreach (FlakDamage d in result.flakDamages)
                {
                    ((UnityFlak) troopMap.Get(d.flakPosition)).Fire();
                    if (d.damaged) ApplyDamage(troop, startingPosition);
                    if (troop.Destroyed) return;
                }
            }
            
            Debug.Log("Battle results length = " + battleResults.Length);
            
            troop.MoveInDirection(direction);
            DoFlakDamages();
            if (troop.Destroyed) return;
            if (troopMap.Get(troop.Position) == null)
            {
                troopMap.AdjustPosition(troop, startingPosition);
                return;
            }
            DoFightDamages();

            
            while (!troop.Destroyed)
            {
                troop.FlyOverOtherTroop();
                DoFlakDamages();
                if (troop.Destroyed) return;
                if (troopMap.Get(troop.Position) != null)
                    DoFightDamages();
                else
                {
                    troopMap.AdjustPosition(troop, startingPosition);
                    return;
                }
            }
        }

        private void ApplyDamage(Troop troop, VectorTwo startingPosition)
        {
            troop.ApplyDamage();
            if (troop.Destroyed)
                troopMap.Remove(troop, startingPosition);
        }
    }
}
