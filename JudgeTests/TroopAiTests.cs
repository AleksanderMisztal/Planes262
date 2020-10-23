﻿using GameDataStructures;
using GameDataStructures.Positioning;
using GameJudge;
using GameJudge.Troops;
using GameJudge.Waves;
using NUnit.Framework;

namespace JudgeTests
{
    public class TroopAiTests
    {
        private TroopMap troopMap;
        private Board board;
        private TroopAi troopAi;
        private WavesBuilder wb;

        private void CreateTroopAi()
        {
            board = new Board(5, 5);
            troopMap = new TroopMap(board);
            troopAi = new TroopAi(troopMap, board);
            wb = new WavesBuilder();
        }

        private void AddTroop(int x, int y)
        {
            wb.Add(1, x, y, PlayerSide.Blue);
        }

        private void DoAddTroops()
        {
            WaveProvider waveProvider = wb.GetWaves();
            troopMap.SpawnWave(waveProvider.GetTroops(1));
        }

        private ITroop GetTroop(int x, int y)
        {
            return troopMap.Get(new VectorTwo(x, y));
        }


        [Test]
        public void Should_ReturnFalse_When_InMiddleBoard()
        {
            CreateTroopAi();

            AddTroop(2, 2);
            DoAddTroops();

            ITroop troop = GetTroop(2, 2);

            Assert.IsFalse(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnFalse_When_BlockedByFriends()
        {
            CreateTroopAi();

            AddTroop(3, 5);
            AddTroop(4, 5);
            AddTroop(4, 4);
            DoAddTroops();

            ITroop troop = GetTroop(3, 5);

            Assert.IsFalse(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_HasToExitBoard()
        {
            CreateTroopAi();

            AddTroop(5, 5);
            DoAddTroops();
            ITroop troop = GetTroop(5, 5);

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_OutsideTheBoard()
        {
            CreateTroopAi();

            AddTroop(8, 9);
            DoAddTroops();

            ITroop troop = GetTroop(8, 9);

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }

        [Test]
        public void Should_ReturnTrue_When_CanReenterBoard()
        {
            CreateTroopAi();

            AddTroop(3, 6);
            DoAddTroops();

            ITroop troop = GetTroop(3, 6);

            Assert.IsTrue(troopAi.ShouldControl(troop));
        }
    }
}