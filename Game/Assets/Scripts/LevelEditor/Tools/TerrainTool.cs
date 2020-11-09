using System.Collections.Generic;
using GameDataStructures.Positioning;
using Planes262.LevelEditor.Tilemaps;
using UnityEngine;
using Terrain = Planes262.LevelEditor.Terrains.Terrain;

namespace Planes262.LevelEditor.Tools
{
    public class TerrainTool : MonoBehaviour
    {
        [SerializeField] private Terrain[] templates;

        private readonly Dictionary<string, Terrain> templateByName = new Dictionary<string, Terrain>();
        private int activeId;

        private HexGrid hexGrid;
        private ResizableGridBase gridBase;
        
        public bool Enabled { get; set; }

        private Terrain GetTemplate(string templateName)
        {
            try
            {
                return templateByName[templateName];
            }
            catch (KeyNotFoundException)
            {
                if (!string.IsNullOrEmpty(templateName))Debug.Log("Couldn't find template with name " + templateName);
                return null;
            }
        }

        public void Initialize(ResizableGridBase theGridBase)
        {
            theGridBase.GridResized += Resize;
            gridBase = theGridBase;
            hexGrid = new HexGrid(theGridBase);
            foreach (Terrain template in templates)
                templateByName.Add(template.name, template);
        }
        
        private void Update()
        {
            if (!Enabled) return;
            
            if (Input.GetKeyDown("n"))
            {
                activeId++;
                activeId %= templates.Length;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = gridBase.GetHexCenterWp();
                VectorTwo v = gridBase.ToOffset(position);
                if (!gridBase.IsInside(v.x, v.y)) return;
                hexGrid.SetTile(position, CreateObject(position, templates[activeId]));
            }
        }

        private void Resize()
        {
            foreach (VectorTwo v in gridBase.newReachable)
            {
                GameObject go = hexGrid.GetTile(v.x, v.y);
                if (go is null) continue;
                go.SetActive(true);
            }
            foreach (VectorTwo v in gridBase.newUnreachable)
            {
                GameObject go = hexGrid.GetTile(v.x, v.y);
                if (go is null) continue;
                go.SetActive(false);
            }
        }

        private GameObject CreateObject(int x, int y, Terrain template)
        {
            Vector3 position = gridBase.ToWorld(x, y);
            return CreateObject(position, template);
        }

        private GameObject CreateObject(Vector3 position, Terrain template)
        {
            TextMesh textMesh = TextCreator.CreateWorldText(template.name, null, position);
            textMesh.fontSize = 60;
            textMesh.transform.localScale /= 10;
            textMesh.name = template.name;
            return textMesh.gameObject;
        }

        public string[] Dto()
        {
            List<string> gridTiles = new List<string>(); 
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                gridTiles.Add(hexGrid.GetTile(x, y)?.name);
            }

            return gridTiles.ToArray();
        }
        
        public void Load(string[] dtos)
        {
            int id = 0;
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                Terrain template = GetTemplate(dtos[id++]);
                hexGrid.SetTile(x, y, template is null ? null : CreateObject(x, y, template));
            }
        }
    }
}