using System;
using GameDataStructures;
using Planes262.UnityLayer.HexSystem;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class BoardCamera : MonoBehaviour
    {
        private Camera boardCamera;
        public GridBase gridBase;

        private const float sensitivity = 5;
        private const float mobility = .2f;

        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;

        private const float minSize = 2;
        private float maxSize = 5;
        private Vector3 center;

        private void Start()
        {
            Initialize(Board.test);
        }

        public void Initialize(Board board)
        {
            Debug.Log("Initializing camera.");
            if (gridBase == null) Debug.Log("Grid base is null!!");
            boardCamera = GetComponent<Camera>();
            SetCameraPositionAndSize(board);
            InitializeBoardBoundaries();
        }

        private void SetCameraPositionAndSize(Board board)
        {
            Vector3 bottomLeft = gridBase.ToWorld(-2, -2);
            Vector3 topRight = gridBase.ToWorld(board.xSize + 1, board.ySize + 1);

            center = (bottomLeft + topRight) / 2;
            center.z = -10;

            transform.position = center;
            boardCamera.orthographicSize = maxSize = (topRight.y - bottomLeft.y) / 2;
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
            float deltaSize = -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            boardCamera.orthographicSize = Mathf.Clamp(boardCamera.orthographicSize + deltaSize, minSize, maxSize);
        }

        private void UpdateCameraPosition()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += Vector3.up * mobility;
            if (Input.GetKey(KeyCode.DownArrow))
                transform.position += Vector3.down * mobility;
            if (Input.GetKey(KeyCode.RightArrow))
                transform.position += Vector3.right * mobility;
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.position += Vector3.left * mobility;
        }

        private void ClampCameraPosition()
        {
            Vector3 position = transform.position;
            float x = Mathf.Clamp(position.x,
                                            xMin + CenterXOffset,
                                            xMax - CenterXOffset);
            float y = Mathf.Clamp(position.y,
                                    yMin + CenterYOffset,
                                    yMax - CenterYOffset);

            position = new Vector3(x, y, -10);
            transform.position = position;
        }

        private float CenterXOffset => -.1f
                                              + boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth, 0, 0)).x
                                              - boardCamera.ScreenToWorldPoint(new Vector3(boardCamera.pixelWidth / 2, 0, 0)).x;

        private float CenterYOffset => -.1f
                                              + boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight, 0)).y
                                              - boardCamera.ScreenToWorldPoint(new Vector3(0, boardCamera.pixelHeight / 2, 0)).y;
    }
}