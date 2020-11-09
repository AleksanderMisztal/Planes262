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
            foreach (TroopTemplate template in templates)
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

            if (Input.GetMouseButtonDown(1))
            {
                VectorTwo cell = gridBase.ToOffset(gridBase.GetHexCenterWp());
                GameObject go = hexGrid.GetTile(cell.x, cell.y);
                go?.GetComponent<RotationHolder>().Rotate();
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
        
        private GameObject CreateObject(Vector3 position, TroopTemplate template)
        {
            GameObject go = new GameObject(template.name);
            go.AddComponent<RotationHolder>();
            go.transform.position = position;
            go.transform.Rotate(Vector3.forward * -30);
            SpriteRenderer spriteRenderer = go.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = template.Sprite;
            return go;
        }

        private GameObject CreateObject(VectorTwo p, int orientation, TroopTemplate template)
        {
            Vector3 position = gridBase.ToWorld(p.x, p.y);
            GameObject go = CreateObject(position, template);
            go.GetComponent<RotationHolder>().Rotate(orientation);
            return go;
        }

        public TroopDto[] Dto()
        {
            List<TroopDto> troopDtos = new List<TroopDto>(); 
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                GameObject go = hexGrid.GetTile(x, y);
                if (go is null) continue;
                troopDtos.Add(new TroopDto {position = new VectorTwo(x, y), orientation = go.GetComponent<RotationHolder>().Rotation, name = go.name});
            }

            return troopDtos.ToArray();
        }
        
        public void Load(TroopDto[] dtos)
        {
            foreach (TroopDto dto in dtos)
            {
                TroopTemplate template = GetTemplate(dto.name);
                hexGrid.SetTile(dto.position, template is null ? null : CreateObject(dto.position, dto.orientation, template));
            }
        }
    }
}