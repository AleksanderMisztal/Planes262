using Planes262.LevelEditor.Tilemaps;
using Planes262.Saving;
using Planes262.UnityLayer;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public class BoardTool : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float moveSpeed;
        [SerializeField] private BackgroundManager backgroundManager;
        private ResizableGridBase gridBase;

        public void Initialize(ResizableGridBase theGridBase)
        {
            gridBase = theGridBase;
            backgroundManager.SetBackground(LevelConfig.background);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                ResizeGrid();
            }
            else
            {
                AdjustCameraPosition();
                AdjustCameraSize();
            }
        }
        
        private void ResizeGrid()
        {
            int dx = 0;
            int dy = 0;
            if (Input.GetKeyDown(KeyCode.DownArrow)) dy--;
            if (Input.GetKeyDown(KeyCode.UpArrow)) dy++;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) dx--;
            if (Input.GetKeyDown(KeyCode.RightArrow)) dx++;

            if (dx == 0 && dy == 0) return;
            
            gridBase.ResizeByDelta(dx, dy);
        }

        private void AdjustCameraPosition()
        {
            Vector3 dp = Vector3.zero;
            if (Input.GetKey(KeyCode.DownArrow)) dp -= Vector3.down;
            if (Input.GetKey(KeyCode.UpArrow)) dp -= Vector3.up;
            if (Input.GetKey(KeyCode.LeftArrow)) dp -= Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) dp -= Vector3.right;
            camera.transform.position += dp * moveSpeed;
        }

        private void AdjustCameraSize()
        {
            if (Input.GetKey(KeyCode.Minus)) camera.orthographicSize += moveSpeed;
            if (Input.GetKey(KeyCode.Equals)) camera.orthographicSize -= moveSpeed;
        }

        
        public void Save()
        {
            BoardDto dto = new BoardDto
            {
                background = LevelConfig.background, 
                offset = camera.transform.position,
                ortoSize = camera.orthographicSize,
                xSize = gridBase.XSize,
                ySize = gridBase.YSize,
            };
            Saver.Save(LevelConfig.name + "/board", dto);
        }

        public void Load()
        {
            BoardDto dto = Saver.Read<BoardDto>(LevelConfig.name + "/board");
            camera.orthographicSize = dto.ortoSize;
            camera.transform.position = dto.offset;
            backgroundManager.SetBackground(dto.background);
        }
    }
}