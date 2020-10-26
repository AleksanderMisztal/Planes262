using System.Collections.Generic;
using Planes262.LevelEditor.Troops;
using Planes262.Saving;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public class TroopTool : HexTool<TroopTemplate>
    {
        private const string saveFileName = "troops";
        
        protected override string GetName(TroopTemplate template) => template.name;

        protected override GameObject CreateObject(Vector3 position, TroopTemplate template)
        {
            GameObject go = new GameObject(template.name);
            go.transform.position = position;
            SpriteRenderer spriteRenderer = go.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = template.Sprite;
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
                troopDtos.Add(new TroopDto {x = x, y = y, orientation = 5, name = go.name});
            }

            return new TroopDtos {dtos = troopDtos.ToArray()};
        }
        
        public override void Load()
        {
            TroopDtos dtos = Saver.Read<TroopDtos>(LevelConfig.name + "/" + saveFileName);
            foreach (TroopDto dto in dtos.dtos)
            {
                TroopTemplate template = GetTemplate(dto.name);
                hexGrid.SetTile(dto.x, dto.y, template is null ? null : CreateObject(dto.x, dto.y, template));
            }
        }
    }
}