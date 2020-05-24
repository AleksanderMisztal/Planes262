using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputParser : MonoBehaviour
{
    private GridLayout gridLayout;
    private GameController gameController;
    private void Awake()
    {
        gridLayout = FindObjectOfType<GridLayout>();
        gameController = FindObjectOfType<GameController>();
    }
    public Vector2Int GetCell(Vector3 v)
    {
        return (Vector2Int) gridLayout.WorldToCell(v);
    }
    private void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cell = GetCell(mousePosition);

        Troop activeTroop = gameController.GetActiveTroop();
        if (activeTroop && activeTroop.IsLegalMove(cell))
        {
            activeTroop.MoveTo(cell);
            return;
        }
        Troop troopAtCell = gameController.GetTroopAt(cell);
        if (troopAtCell)
        {
            gameController.SetActiveTroop(troopAtCell);
        }
    }
}
