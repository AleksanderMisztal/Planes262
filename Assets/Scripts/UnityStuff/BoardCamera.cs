using Scripts.GameLogic;
using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    private static BoardCamera instance;
    private static Camera boardCamera;

    [SerializeField]
    private GridLayout gridLayout;

    private static float sensitivity = 5;
    private static float mobility = .05f;

    private static float xMin;
    private static float xMax;
    private static float yMin;
    private static float yMax;

    private static float minSize = 2;
    private static float maxSize = 5;
    private static Vector3 center;

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

        center = (bottomLeft + topRight) / 2;
        center.z = -10;

        instance.transform.position = center;

        float xSize = ((float)(board.xMax - board.xMin)) / 3 + 1.5f;
        float ySize = ((float)(board.yMax - board.yMin)) / 2 + 1;

        xMin = center.x - xSize;
        xMax = center.x + xSize;
        yMin = center.y - ySize;
        yMax = center.y + ySize;

        boardCamera = instance.GetComponent<Camera>();
        boardCamera.orthographicSize = maxSize = Mathf.Max(xSize, ySize);
    }

    private void Update()
    {
        float deltaSize = -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        boardCamera.orthographicSize = Mathf.Clamp(boardCamera.orthographicSize + deltaSize, minSize, maxSize);

        if (Input.GetKey(KeyCode.UpArrow))
            instance.transform.position += Vector3.up * mobility;
        if (Input.GetKey(KeyCode.DownArrow))
            instance.transform.position += Vector3.down * mobility;
        if (Input.GetKey(KeyCode.RightArrow))
            instance.transform.position += Vector3.right * mobility;
        if (Input.GetKey(KeyCode.LeftArrow))
            instance.transform.position += Vector3.left * mobility;

        //float x = Mathf.Clamp(instance.transform.position.x,
        //                        xMin + boardCamera.orthographicSize,
        //                        xMax - boardCamera.orthographicSize);
        //float y = Mathf.Clamp(instance.transform.position.y,
        //                        yMin + boardCamera.orthographicSize,
        //                        yMax - boardCamera.orthographicSize);

        //instance.transform.position = new Vector3(x, y, -10);
    }
}
