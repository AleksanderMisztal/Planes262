using System.Collections.Generic;
using GameDataStructures.Positioning;
using Planes262.LevelEditor.Troops;
using Planes262.Saving;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public class TroopTool : HexTool<TroopTemplate>
    {
        private const string saveFileName = "troops";
        
        protected override string GetName(TroopTemplate template) => template.name;

        protected override void CustomUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                VectorTwo cell = gridBase.ToOffset(gridBase.GetHexCenterWp());
                GameObject go = hexGrid.GetTile(cell.x, cell.y);
                go.GetComponent<RotationHolder>().Rotate();
            }
        }

        protected override GameObject CreateObject(Vector3 position, TroopTemplate template)
        {
            GameObject go = new GameObject(template.name);
            go.AddComponent<RotationHolder>();
            go.transform.position = position;
            go.transform.Rotate(Vector3.forward * -30);
            SpriteRenderer spriteRenderer = go.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = template.Sprite;
            return go;
        }

        private GameObject CreateObject(int x, int y, int orientation, TroopTemplate template)
        {
            Vector3 position = gridBase.ToWorld(x, y);
            GameObject go = CreateObject(position, template);
            go.GetComponent<RotationHolder>().Rotate(orientation);
            return go;
        }

        
        public override void Save()
        {
            Saver.Save(LevelConfig.name + "/" + saveFileName, Dto());
        }
        
        private TroopDtos Dto()
        {
            List<TroopDto> troopDtos = new List<TroopDto>(); 
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                GameObject go = hexGrid.GetTile(x, y);
                if (go == null) continue;
                troopDtos.Add(new TroopDto {x = x, y = y, orientation = go.GetComponent<RotationHolder>().Rotation, name = go.name});
            }

            return new TroopDtos {dtos = troopDtos.ToArray()};
        }
        
        public override void Load()
        {
            TroopDtos dtos = Saver.Read<TroopDtos>(LevelConfig.name + "/" + saveFileName);
            foreach (TroopDto dto in dtos.dtos)
            {
                TroopTemplate template = GetTemplate(dto.name);
                hexGrid.SetTile(dto.x, dto.y, template is null ? null : CreateObject(dto.x, dto.y, dto.orientation, template));
            }
        }
    }
}