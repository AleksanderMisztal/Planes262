using Planes262.LevelEditor.Tilemaps;
using Planes262.LevelEditor.Tools;
using Planes262.Saving;
using UnityEngine;

namespace Planes262.LevelEditor
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private HexTool[] tools;
        private BoardTool boardTool;
        private int activeTool;

        private ResizableGridBase gridBase;
        private const float cellSize = 1f;

        private void Start()
        {
            LineDrawer.lineParent = new GameObject("Line Parent").transform;
            gridBase = new ResizableGridBase(cellSize);
            boardTool = GetComponent<BoardTool>();
            boardTool.Initialize(gridBase);
            foreach (HexTool tool in tools) tool.Initialize(gridBase);
            tools[0].Enabled = true;

            if (LevelConfig.isLoaded) Load();
            else Save();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) NextTool();
            if (Input.GetKeyDown(KeyCode.S)) Save();
            if (Input.GetKeyDown(KeyCode.L)) Load();
        }

        private void NextTool()
        {
            tools[activeTool++].Enabled = false;
            if (activeTool >= tools.Length) activeTool -= tools.Length;
            tools[activeTool].Enabled = true;
        }

        private void Save()
        {
            boardTool.Save();
            foreach (HexTool tool in tools)
                tool.Save();
        }

        private void Load()
        {
            BoardDto dto = Saver.Read<BoardDto>(LevelConfig.name + "/board");
            gridBase.Resize(dto.xSize, dto.ySize);
            boardTool.Load();
            foreach (HexTool tool in tools)
                tool.Load();
        }
    }
}