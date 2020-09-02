using Planes262.GameLogic;
using Planes262.GameLogic.Area;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class BoardCamera : MonoBehaviour
    {
        private Camera boardCamera;

        [SerializeField]
        private GridLayout gridLayout;

        private const float Sensitivity = 5;
        private const float Mobility = .2f;

        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;

        private const float MinSize = 2;
        private float maxSize = 5;
        private Vector3 center;


        public void Initialize(Board board)
        {
            SetCameraPosition(board);
            SetCameraSize(board);
            InitializeBoardBoundaries();
        }

        private void SetCameraPosition(Board board)
        {
            Vector3 bottomLeft = gridLayout.CellToWorld(new Vector3Int(-1, -1, -10));
            Vector3 topRight = gridLayout.CellToWorld(new Vector3Int(board.XMax + 1, board.YMax + 1, -10));

            center = (bottomLeft + topRight) / 2;
            center.z = -10;

            transform.position = center;
        }

        private void SetCameraSize(Board board)
        {

            //TODO: calculate based on screen size
            float xSize = (float)board.XMax / 3 + 1.5f;
            float ySize = (float)board.YMax / 2 + 1;

            boardCamera = GetComponent<Camera>();
            boardCamera.orthographicSize = maxSize = Mathf.Max(xSize, ySize);
        }

        private void InitializeBoardBoundaries()
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

        private void UpdateCameraSize()
        {
            float deltaSize = -Input.GetAxis("Mouse ScrollWheel") * Sensitivity;
            boardCamera.orthographicSize = Mathf.Clamp(boardCamera.orthographicSize + deltaSize, MinSize, maxSize);
        }

        private void UpdateCameraPosition()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += Vector3.up * Mobility;
            if (Input.GetKey(KeyCode.DownArrow))
                transform.position += Vector3.down * Mobility;
            if (Input.GetKey(KeyCode.RightArrow))
                transform.position += Vector3.right * Mobility;
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.position += Vector3.left * Mobility;
        }

        private void ClampCameraPosition()
        {
            float x = Mathf.Clamp(transform.position.x,
                                            xMin + CenterXOffset,
                                            xMax - CenterXOffset);
            float y = Mathf.Clamp(transform.position.y,
                                    yMin + CenterYOffset,
                                    yMax - CenterYOffset);

            transform.position = new Vector3(x, y, -10);
        }

        private float CenterXOffset => -.1f
                                              + boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth, 0, 0)).x
                                              - boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth / 2, 0, 0)).x;

        private float CenterYOffset => -.1f
                                              + boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight, 0)).y
                                              - boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight / 2, 0)).y;
    }
}