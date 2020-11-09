using GameDataStructures.Dtos;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class BoardCamera : MonoBehaviour
    {
        private Camera boardCamera;

        private const float sensitivity = 5;
        private const float mobility = .2f;

        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;

        private float minSize = 0;
        private float maxSize = 100;

        public void Initialize(CameraDto dto)
        {
            boardCamera = GetComponent<Camera>();
            Vector3 offset = new Vector3(dto.xOffset,  dto.yOffset, -10);
            float ortoSize = dto.ortoSize;
            boardCamera.transform.position = offset;
            boardCamera.GetComponent<Camera>().orthographicSize = ortoSize;
            
            maxSize = ortoSize;
            minSize = ortoSize / 5;

            float aspect = boardCamera.aspect;
            xMin = offset.x - ortoSize * aspect;
            xMax = offset.x + ortoSize * aspect;
            yMin = offset.y - ortoSize;
            yMax = offset.y + ortoSize;
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
            const float d = .01f;
            Vector3 position = transform.position;
            float xOffset = boardCamera.orthographicSize * boardCamera.aspect;
            float yOffset = boardCamera.orthographicSize;
            float x = Mathf.Clamp(position.x,
                                            xMin + xOffset - d,
                                            xMax - xOffset + d);
            float y = Mathf.Clamp(position.y,
                                    yMin + yOffset - d,
                                    yMax - yOffset + d);
            transform.position = new Vector3(x, y, -10);
        }
    }
}