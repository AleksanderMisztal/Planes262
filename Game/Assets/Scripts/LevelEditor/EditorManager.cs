using GameDataStructures.Dtos;
using Planes262.LevelEditor.Tilemaps;
using Planes262.LevelEditor.Tools;
using Planes262.UnityLayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Planes262.LevelEditor
{
    public class EditorManager : MonoBehaviour
    {
        [SerializeField] private TroopTool troopTool;
        [SerializeField] private TerrainTool terrainTool;
        [SerializeField] private BoardTool boardTool;
        
        [SerializeField] private BackgroundManager backgroundManager;

        private ResizableGridBase gridBase;
        private const float cellSize = 1f;

        private void Start()
        {
            backgroundManager.SetBackground(LevelConfig.background);
            InitializeTools();
            //if (LevelConfig.isLoaded) Load();
            Save();
        }

        private void InitializeTools()
        {
            gridBase = new ResizableGridBase(cellSize);
            if (gridBase == null) Debug.Log("Wtf");
            boardTool.Initialize(gridBase);
            troopTool.Initialize(gridBase);
            terrainTool.Initialize(gridBase);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) NextTool();
            if (Input.GetKeyDown(KeyCode.S)) Save();
            if (Input.GetKeyDown(KeyCode.L)) Load();
        }

        private void NextTool()
        {
            if (troopTool.Enabled)
            {
                troopTool.Enabled = false;
                terrainTool.Enabled = true;
            }
            else
            {
                terrainTool.Enabled = false;
                troopTool.Enabled = true;
            }
        }

        public void BackToMain()
        {
            Save();
            SceneManager.LoadScene("Main Menu");
        }

        private void Save()
        {
            LevelDto levelDto = new LevelDto
            {
                background = LevelConfig.background,
                board = new BoardDto{xSize = gridBase.XSize, ySize = gridBase.YSize},
                cameraDto = boardTool.Dto(),
                troopDtos = troopTool.Dto(),
            };
            Saver.Save(LevelConfig.name, levelDto);
        }

        private void Load()
        {
            LevelDto levelDto = Saver.Read<LevelDto>(LevelConfig.name);
            backgroundManager.SetBackground(levelDto.background);
            gridBase.Resize(levelDto.board.Get());
            boardTool.Load(levelDto.cameraDto);
            troopTool.Load(levelDto.troopDtos);
        }
    }
}