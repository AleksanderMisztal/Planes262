using Scripts.GameLogic;
using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    private static BoardCamera instance;

    [SerializeField]
    private GridLayout gridLayout;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying this...");
            Destroy(this);
        }
    }

    public static void Initialize(BoardParams board)
    {
        Vector3 bottomLeft = instance.gridLayout.CellToWorld(new Vector3Int(board.xMin - 1, board.yMin - 1, -10));
        Vector3 topRight = instance.gridLayout.CellToWorld(new Vector3Int(board.xMax + 1, board.yMax + 1, -10));

        Vector3 center = (bottomLeft + topRight) / 2;
        center.z = -10;

        instance.transform.position = center;

        float xSize = ((float)(board.xMax - board.xMin)) / 3 + 1.5f;
        float ySize = ((float)(board.yMax - board.yMin)) / 2 + 1;

        instance.GetComponent<Camera>().orthographicSize = Mathf.Max(xSize, ySize);
    }
}
