using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
using GameJudge.Waves;
using NUnit.Framework;

namespace JudgeTests
{
    public class MoveValidatorTests
    {
        private TroopMap troopMap;
        private WavesBuilder wb;
        private PlayerSide player0;
        private Board board;

        private MoveValidator validator;


        private void CreateValidator()
        {
            board = new Board(5, 5);
            troopMap = new TroopMap(board);
            player0 = PlayerSide.Red;
            wb = new WavesBuilder();

            validator = new MoveValidator(troopMap, board, player0);
            validator.ToggleActivePlayer();
        }

        private void AddTroop(int x, int y)
        {
            wb.Add(1, x, y, PlayerSide.Blue);
        }

        private void DoAddTroops()
        {
            WaveProvider waveProvider = wb.GetWaves();
            troopMap.SpawnWave(waveProvider.GetTroops(1));
            OnTurnBegin(PlayerSide.Blue);
        }

        private void OnTurnBegin(PlayerSide player)
        {
            foreach (ITroop troop in troopMap.GetTroops(player))
            {
                troop.ResetMovePoints();
            }
        }

        private ITroop GetTroop(int x, int y)
        {
            return troopMap.Get(new VectorTwo(x, y));
        }

        [Test]
        public void Should_ReturnTrue_When_NormalMove()
        {
            CreateValidator();
            AddTroop(2, 2);
            DoAddTroops();
            ITroop troop = GetTroop(2, 2);

            Assert.IsTrue(validator.IsLegalMove(PlayerSide.Blue, troop.Position, 0));
        }

        [Test]
        public void Should_ReturnFalse_When_EntersFriend()
        {
            CreateValidator();
            AddTroop(2, 2);
            AddTroop(3, 2);
            DoAddTroops();
            ITroop troop = GetTroop(2, 2);

            Assert.IsFalse(validator.IsLegalMove(PlayerSide.Blue, troop.Position, 0));
        }

        [Test]
        public void Should_ReturnTrue_When_BlockedBy3Friends()
        {
            CreateValidator();
            AddTroop(2, 2);

            AddTroop(2, 3);
            AddTroop(3, 2);
            AddTroop(2, 1);
            DoAddTroops();

            ITroop troop = GetTroop(2, 2);

            Assert.IsTrue(validator.IsLegalMove(PlayerSide.Blue, troop.Position, 0));
        }
    }
}
