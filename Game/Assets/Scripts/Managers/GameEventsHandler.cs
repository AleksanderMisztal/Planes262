using System.Collections.Generic;
using GameDataStructures;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.Managers
{
    public class GameEventsHandler
    {
        private readonly GameManager gameManager;
        private readonly ScoreDisplay score;
        private readonly ClockDisplay clockDisplay;

        public GameEventsHandler(GameManager gameManager, ScoreDisplay score, ClockDisplay clockDisplay)
        {
            this.gameManager = gameManager;
            this.score = score;
            this.clockDisplay = clockDisplay;
        }


        public void OnGameEnded(ScoreInfo scoreInfo)
        {
            gameManager.EndGame($"Final score: red: {scoreInfo.Red}, blue: {scoreInfo.Blue}", 1.5f);
        }

        public void OnOpponentDisconnected()
        {
            gameManager.EndGame("Opponent has disconnected :(", 0);
        }

        public void OnLostOnTime(PlayerSide loser)
        {
            gameManager.EndGame($"Player {loser} lost on time :(", 0);
        }

        
        public void OnGameReady(string opponentName, PlayerSide side, Board board, IEnumerable<TroopDto> troops, ClockInfo clockInfo)
        {
            Debug.Log("Game joined received! Playing against " + opponentName);
            clockDisplay.Initialize(clockInfo);
            gameManager.StartNewGame(board, troops, side);
            if (side == PlayerSide.Red) score.SetNames(PlayerMeta.name, opponentName);
            else score.SetNames(opponentName, PlayerMeta.name);
        }

        public void OnTroopsSpawned(IEnumerable<TroopDto> troops, TimeInfo timeInfo)
        {
            clockDisplay.ToggleActivePlayer(timeInfo);
            gameManager.BeginNextRound(troops);
        }

        public void OnTroopMoved(VectorTwo position, int direction, BattleResult[] battleResults, ScoreInfo scoreInfo)
        {
            gameManager.MoveTroop(position, direction, battleResults);
            score.Set(scoreInfo.Red, scoreInfo.Blue);
        }
    }
}