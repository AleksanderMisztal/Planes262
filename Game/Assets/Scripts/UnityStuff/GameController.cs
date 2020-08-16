﻿using System.Collections.Generic;
using GameServer.GameLogic;
using GameServer.Utils;
using Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
{
    public static class GameController
    {
        public static PlayerSide Side { get; private set; }

        private static VectorTwo troopPosition = null;
        private static TroopDto troopDto = null;
        private static HashSet<VectorTwo> reachableCells = null;
        private static VectorTwo targetPosition = null;
        private static List<int> directions = null;


        public static void OnCellClicked(VectorTwo cell)
        {
            bool troopIsSelectedAndCanReachCell = troopPosition != null && reachableCells.Contains(cell);
            if (troopIsSelectedAndCanReachCell)
                ChangePathOrSend(cell);
            else SelectTroop(cell);
        }

        private static void ChangePathOrSend(VectorTwo cell)
        {
            if (targetPosition == cell)
                SendMoves(troopPosition, troopDto.orientation, directions);
            else SetAsTarget(cell);
        }

        private static void SendMoves(VectorTwo position, int orientation, List<int> directions)
        {
            foreach (var dir in directions)
            {
                ClientSend.MoveTroop(position, dir);
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
            }
        }

        private static void SetAsTarget(VectorTwo cell)
        {
            targetPosition = cell;
            directions = GameState.GetDirections(troopPosition, targetPosition);
            HighlightPath(troopPosition, troopDto.orientation, directions);
        }

        private static void SelectTroop(VectorTwo cell)
        {
            troopDto = GameState.GetTroopSide(cell);
            if (troopDto != null && troopDto.side == Side)
                ActivateTroopAt(cell);
            else DeactivateTroops();
        }

        private static void ActivateTroopAt(VectorTwo cell)
        {
            troopPosition = cell;
            reachableCells = GameState.GetReachableCells(cell);
            Debug.Log($"Can reach {reachableCells.Count} cells...");
            TileManager.ActivateTiles(reachableCells);
        }

        private static void DeactivateTroops()
        {
            TileManager.DeactivateTiles();
            troopPosition = null;
        }

        private static void HighlightPath(VectorTwo position, int orientation, List<int> directions)
        {
            var cells = new List<VectorTwo>();
            foreach (var dir in directions)
            {
                orientation += dir;
                position = Hex.GetAdjacentHex(position, orientation);
                cells.Add(position);
            }
            TileManager.HighlightPath(cells);
        }
    }
}