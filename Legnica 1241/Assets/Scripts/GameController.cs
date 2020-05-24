using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    // References
    TroopSpawner troopSpawner;

    // State
    PlayerID activePlayer = PlayerID.Red;
    Troop activeTroop;
    HashSet<Troop> blueTroops;
    HashSet<Troop> redTroops;
    Dictionary<Vector2Int, Troop> troopAtPosition;
    private int movePointsLeft;

    private void Awake()
    {
        troopSpawner = FindObjectOfType<TroopSpawner>();
        blueTroops = new HashSet<Troop>();
        redTroops = new HashSet<Troop>();
        troopAtPosition = new Dictionary<Vector2Int, Troop>();
    }
    public void Initialize()
    {
        Troop[] troops = FindObjectsOfType<Troop>();
        foreach (Troop troop in troops)
        {
            InitializeTroop(troop);
        }
    }
    public void InitializeTroop(Troop troop)
    {
        troopAtPosition.Add(troop.GetPosition(), troop);

        if (troop.controllingPlayer == PlayerID.Blue)
        {
            blueTroops.Add(troop);
        }
        else
        {
            redTroops.Add(troop);
        }
    }
    public PlayerID GetActivePlayer() 
    {
        return activePlayer; 
    }
    public Troop GetActiveTroop() 
    { 
        return activeTroop; 
    }
    public Vector2Int GetEmptyCell(Vector2Int seedPosition)
    {
        if (!GetTroopAt(seedPosition)) return seedPosition;
        Vector2Int[] neighbours = Hex.GetNeighbours(seedPosition);
        Randomize(neighbours);
        foreach (var position in neighbours)
        {
            if (!GetTroopAt(position))
            {
                return position;
            }
        }
        return GetEmptyCell(neighbours[0]);
    }
    private void Randomize<T>(T[] items)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < items.Length - 1; i++)
        {
            int j = rand.Next(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
    public void ChangeTroopPosition(Vector2Int oldPosition, Vector2Int newPosition)
    {
        troopAtPosition.Add(newPosition, GetTroopAt(oldPosition));
        troopAtPosition.Remove(oldPosition);
    }
    public Troop GetTroopAt(Vector2Int position)
    {
        try
        {
            return troopAtPosition[position];
        }
        catch
        {
            return null;
        }
    }
    public void DestroyTroopAtPosition(Vector2Int position)
    {
        Troop troop = GetTroopAt(position);
        troopAtPosition.Remove(position);
        if (troop.controllingPlayer == PlayerID.Blue)
        {
            blueTroops.Remove(troop);
            if (blueTroops.Count == 0) EndGame();
        }
        else
        {
            redTroops.Remove(troop);
            if (redTroops.Count == 0) EndGame();
        }
        if (troop.controllingPlayer == activePlayer)
        {
            movePointsLeft -= troop.GetMovePoints();
            if (movePointsLeft <= 0)
                ToggleActivePlayer();
        }
    }
    private void EndGame()
    {
        FindObjectOfType<SceneLoader>().LoadWinScreen();
    }
    public void ToggleActivePlayer()
    {
        if (activeTroop) { 
            activeTroop.Desactivate();
            activeTroop = null;
        }
        troopSpawner.StartNextWave();

        if (activePlayer == PlayerID.Blue)
        {
            foreach(Troop troop in redTroops)
            {
                troop.HandleTurnBegin();
            }
            foreach (Troop troop in blueTroops)
            {
                troop.HandleTurnEnd();
            }
            activePlayer = PlayerID.Red;
        }
        else 
        {
            foreach(Troop troop in blueTroops)
            {
                troop.HandleTurnBegin();
            }
            foreach (Troop troop in redTroops)
            {
                troop.HandleTurnEnd();
            }
            activePlayer = PlayerID.Blue;
        }
        SetInitialMovePointsLeft(activePlayer);
        if (movePointsLeft == 0)
            ToggleActivePlayer();
    }
    private void SetInitialMovePointsLeft(PlayerID player)
    {
        HashSet<Troop> troops = player == PlayerID.Blue ? blueTroops : redTroops;
        movePointsLeft = troops.Aggregate(0, (acc, t) => acc + t.GetInitialMovePoints());
    }
    public void DecrementMovePointsLeft()
    {
        movePointsLeft--;
        if (movePointsLeft <= 0)
            ToggleActivePlayer();
    }
    public void SetActiveTroop(Troop troop)
    {
        if (activeTroop) activeTroop.Desactivate();
        if (activeTroop == troop || troop.controllingPlayer != activePlayer)
        {
            activeTroop = null;
            return;
        }
        troop.Activate();
        activeTroop = troop;
    }
}
