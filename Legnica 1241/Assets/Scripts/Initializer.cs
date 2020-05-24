using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    void Start()
    {
        InitializeTroopPositions();
        FindObjectOfType<GameController>().Initialize();
        FindObjectOfType<GameController>().ToggleActivePlayer();
        
    }
    void InitializeTroopPositions()
    {
        Troop[] troops = FindObjectsOfType<Troop>();
        foreach(Troop troop in troops)
        {
            Vector3Int cellPosition = FindObjectOfType<GridLayout>().WorldToCell(troop.transform.position);
            troop.Initilalize((Vector2Int)cellPosition);
        }
    }
}
