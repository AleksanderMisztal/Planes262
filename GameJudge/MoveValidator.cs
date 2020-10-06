using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge.Troops;

namespace GameJudge
{
    internal class MoveValidator
    {
        private readonly TroopMap map;
        private readonly Board area;
        private PlayerSide activePlayer;
        private string message;


        public MoveValidator(TroopMap map, Board area, PlayerSide player0)
        {
            this.map = map;
            this.area = area;
            activePlayer = player0;
        }

        public void ToggleActivePlayer()
        {
            activePlayer = activePlayer.Opponent();
        }

        public bool IsLegalMove(PlayerSide player, VectorTwo position, int direction)
        {
            try
            {
                IsPlayersTurn(player);
                PositionContainsTroop(position);
                ITroop troop = map.Get(position);
                PlayerControlsTroop(player, troop);
                TroopHasMovePoints(troop);
                NotEnteringFriendOrBlocked(troop, direction);

                message = "Move is valid.";
                return true;
            }
            catch (IllegalMoveException ex)
            {
                message = ex.Message;
                MyLogger.Log(message);
                MyLogger.Log($"Pos: {position}, dir: {direction}");
                return false;
            }
        }


        private void IsPlayersTurn(PlayerSide player)
        {
            if (player != activePlayer)
                throw new IllegalMoveException("Attempting to make a move in opponent's turn!");
        }

        private void PositionContainsTroop(VectorTwo position)
        {
            if (map.Get(position) == null)
                throw new IllegalMoveException("No troop at the specified hex!");
        }

        private static void PlayerControlsTroop(PlayerSide player, ITroop troop)
        {
            if (player != troop.Player)
                throw new IllegalMoveException("Attempting to move enemy troop!");
        }

        private static void TroopHasMovePoints(ITroop troop)
        {
            if (troop.MovePoints <= 0)
                throw new IllegalMoveException("Attempting to move a troop with no move points!");
        }

        private void NotEnteringFriendOrBlocked(ITroop troop, int direction)
        {
            VectorTwo targetPosition = Hex.GetAdjacentHex(troop.Position, troop.Orientation + direction);
            ITroop encounter = map.Get(targetPosition);

            if (encounter == null || encounter.Player != troop.Player) return;

            // Tries to enter a friend so throw if has some other legal move
            foreach (VectorTwo cell in troop.ControlZone)
            {
                ThrowIfNotBlocked(troop, cell);
            }
        }

        private void ThrowIfNotBlocked(ITroop troop, VectorTwo cell)
        {
            if (!area.IsInside(cell)) return;

            ITroop encounter = map.Get(cell);
            if (encounter == null || encounter.Player != troop.Player)
            {
                throw new IllegalMoveException("Attempting to enter a cell with friendly troop!");
            }
        }
    }
}
