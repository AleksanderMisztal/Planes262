using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSpawner : MonoBehaviour
{
    // References
    private GameController gameController;

    // Params
    [SerializeField] Troop blueTroop;
    [SerializeField] Troop redTroop;
    [SerializeField] Vector2Int blueSpawn;
    [SerializeField] Vector2Int redSpawn;
    [SerializeField] int[] blueWaves;
    [SerializeField] int[] redWaves;

    // State
    private int waveNumber = 0;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    public void StartNextWave()
    {
        waveNumber++;
        SpawnTroops();
    }
    private void SpawnTroops()
    {
        if ((waveNumber & 1) == 1)
        {
            try
            {
                SpawnWave(blueTroop, blueSpawn, blueWaves[waveNumber / 2]);
            }
            catch { }
        } 
        else
        {
            try
            {
                SpawnWave(redTroop, redSpawn, redWaves[waveNumber / 2 - 1]);
            }
            catch { }
        }
    }

    private void SpawnWave(Troop troop, Vector2Int seedPosition, int amount)
    {
        while (amount --> 0)
        {
            Vector2Int position = gameController.GetEmptyCell(seedPosition);
            Troop newTroop = Instantiate(troop, new Vector3(0, 0, 0), Quaternion.identity);
            newTroop.Initilalize(position);
            gameController.InitializeTroop(newTroop);
        }
    }
}
