using Planes262.GameLogic;
using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    private static BoardCamera instance;
    private static Camera boardCamera;

    [SerializeField]
    private GridLayout gridLayout;

    private static float sensitivity = 5;
    private static float mobility = .2f;

    private static float xMin;
    private static float xMax;
    private static float yMin;
    private static float yMax;

    private static readonly float minSize = 2;
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


    public static void Initialize(Board board)
    {
        SetCameraPosition(board);
        SetCameraSize(board);
        InitializeBoardBoundaries();
    }

    private static void SetCameraPosition(Board board)
    {
        Vector3 bottomLeft = instance.gridLayout.CellToWorld(new Vector3Int(-1, -1, -10));
        Vector3 topRight = instance.gridLayout.CellToWorld(new Vector3Int(board.xMax + 1, board.yMax + 1, -10));

        center = (bottomLeft + topRight) / 2;
        center.z = -10;

        instance.transform.position = center;
    }

    private static void SetCameraSize(Board board)
    {

        //TODO: calculate based on screen size
        float xSize = ((float)board.xMax) / 3 + 1.5f;
        float ySize = ((float)board.yMax) / 2 + 1;

        boardCamera = instance.GetComponent<Camera>();
        boardCamera.orthographicSize = maxSize = Mathf.Max(xSize, ySize);
    }

    private static void InitializeBoardBoundaries()
    {
        Vector3 camBottomLeft = boardCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 camTopRight = boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth, boardCamera.pixelHeight, 0));

        xMin = camBottomLeft.x;
        yMin = camBottomLeft.y;
        xMax = camTopRight.x;
        yMax = camTopRight.y;
    }


    private void Update()
    {
        UpdateCameraSize();
        UpdateCameraPosition();
        ClampCameraPosition();
    }

    private static void UpdateCameraSize()
    {
        float deltaSize = -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        boardCamera.orthographicSize = Mathf.Clamp(boardCamera.orthographicSize + deltaSize, minSize, maxSize);
    }

    private static void UpdateCameraPosition()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            instance.transform.position += Vector3.up * mobility;
        if (Input.GetKey(KeyCode.DownArrow))
            instance.transform.position += Vector3.down * mobility;
        if (Input.GetKey(KeyCode.RightArrow))
            instance.transform.position += Vector3.right * mobility;
        if (Input.GetKey(KeyCode.LeftArrow))
            instance.transform.position += Vector3.left * mobility;
    }

    private void ClampCameraPosition()
    {
        float x = Mathf.Clamp(instance.transform.position.x,
                                        xMin + CenterXOffset,
                                        xMax - CenterXOffset);
        float y = Mathf.Clamp(instance.transform.position.y,
                                yMin + CenterYOffset,
                                yMax - CenterYOffset);

        instance.transform.position = new Vector3(x, y, -10);
    }

    private float CenterXOffset => -.1f
        + boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth, 0, 0)).x
        - boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth / 2, 0, 0)).x;

    private float CenterYOffset => -.1f
        + boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight, 0)).y
        - boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight / 2, 0)).y;
}
