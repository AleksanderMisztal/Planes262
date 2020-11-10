using System.Collections.Generic;
using GameDataStructures.Dtos;
using GameDataStructures.Positioning;
using Planes262.LevelEditor.Tilemaps;
using Planes262.LevelEditor.Troops;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public class TroopTool : MonoBehaviour
    {
        [SerializeField] private TroopTemplate[] templates;

        private readonly Dictionary<string, TroopTemplate> templateByName = new Dictionary<string, TroopTemplate>();
        private int activeId;

        private HexGrid hexGrid;
        private ResizableGridBase gridBase;

        public bool Enabled { get; set; }

        private TroopTemplate GetTemplate(string templateName)
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
            for (int i = 0; i < templates.Length - 1; i++)
            {
                TroopTemplate template = templates[i];
                templateByName.Add(template.name, template);
            }
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
                hexGrid.SetTile(position, CreateObject(position, templates[activeId])?.gameObject);
            }

            if (Input.GetMouseButtonDown(1))
            {
                VectorTwo cell = gridBase.ToOffset(gridBase.GetHexCenterWp());
                TroopObject go = hexGrid.GetTile(cell.x, cell.y).GetComponent<TroopObject>();
                if (go != null) go.Rotate();
            }
        }

        private void Resize()
        {
            foreach (VectorTwo v in gridBase.newReachable)
            {
                GameObject go = hexGrid.GetTile(v.x, v.y);
                if (go == null) continue;
                go.SetActive(true);
            }
            foreach (VectorTwo v in gridBase.newUnreachable)
            {
                GameObject go = hexGrid.GetTile(v.x, v.y);
                if (go == null) continue;
                go.SetActive(false);
            }
        }
        
        private TroopObject CreateObject(Vector3 position, TroopTemplate template)
        {
            if (template is null) return null;
            TroopObject troopObject = new GameObject(template.name).AddComponent<TroopObject>();
            troopObject.template = template;
            troopObject.transform.position = position;
            troopObject.transform.Rotate(Vector3.forward * -30);
            SpriteRenderer spriteRenderer = troopObject.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = template.Sprite;
            return troopObject;
        }

        private TroopObject CreateObject(VectorTwo p, int orientation, TroopTemplate template)
        {
            Vector3 position = gridBase.ToWorld(p.x, p.y);
            TroopObject go = CreateObject(position, template);
            go.GetComponent<TroopObject>().Rotate(orientation);
            return go;
        }

        public TroopDto[] Dto()
        {
            List<TroopDto> troopDtos = new List<TroopDto>(); 
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                TroopObject go = hexGrid.GetTile(x, y)?.GetComponent<TroopObject>();
                if (go == null) continue;
                troopDtos.Add(new TroopDto
                {
                    name = go.name,
                    type = go.template.type,
                    side = go.template.side,
                    movePoints = go.template.movePoints,
                    health = go.template.health,
                    position = new VectorTwo(x, y).Dto(),
                    orientation = go.GetComponent<TroopObject>().Rotation,
                });
            }

            return troopDtos.ToArray();
        }
        
        public void Load(TroopDto[] dtos)
        {
            foreach (TroopDto dto in dtos)
            {
                TroopTemplate template = GetTemplate(dto.name);
                hexGrid.SetTile(dto.position.Get(), CreateObject(dto.position.Get(), dto.orientation, template).gameObject);
            }
        }
    }
}