using System;
using System.Collections.Generic;
using GameServer.Utils;

namespace GameServer.GameLogic
{
    public class GameController
    {
        private PlayerSide activePlayer = PlayerSide.Red;

        private readonly Score score = new Score();

        private readonly TroopMap troopMap;
        private readonly MoveValidator validator;

        public GameController(Board board)
        {
            troopMap = new TroopMap();
            validator = new MoveValidator(troopMap, board, activePlayer);
        }


        public void BeginNextRound(IEnumerable<Troop> troops)
        {
            troopMap.SpawnWave(troops);
            ChangeActivePlayer();
        }

        private void ChangeActivePlayer()
        {
            HashSet<Troop> beginningTroops = troopMap.GetTroops(activePlayer.Opponent());
            foreach (var troop in beginningTroops)
                troop.ResetMovePoints();

            activePlayer = activePlayer.Opponent();
            validator.ToggleActivePlayer();
        }

        public void MoveTroop(Vector2Int position, int direction, List<BattleResult> battleResults)
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
            var result = battleResults[battleId++];

            if (result.AttackerDamaged) ApplyDamage(troop);
            if (result.DefenderDamaged) ApplyDamage(encounter);

            troop.FlyOverOtherTroop();
            
            while ((encounter = troopMap.Get(troop.Position)) != null && troop.Health > 0)
            {
                result = battleResults[battleId++];
                battleResults.Add(result);
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
                DestroyTroop(troop);
        }

        private void DestroyTroop(Troop troop)
        {
            troopMap.Remove(troop);
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }
    }
}
