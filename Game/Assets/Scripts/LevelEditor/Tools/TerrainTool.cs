using System.Collections.Generic;
using Planes262.Saving;
using UnityEngine;
using Terrain = Planes262.LevelEditor.Terrains.Terrain;

namespace Planes262.LevelEditor.Tools
{
    public class TerrainTool : HexTool<Terrain>
    {
        private const string saveFileName = "terrains";
        
        protected override string GetName(Terrain template) => template.name;

        protected override GameObject CreateObject(Vector3 position, Terrain template)
        {
            TextMesh textMesh = TextCreator.CreateWorldText(template.name, null, position);
            textMesh.fontSize = 60;
            textMesh.transform.localScale /= 10;
            textMesh.name = template.name;
            return textMesh.gameObject;
        }

        
        public override void Save()
        {
            Saver.Save(LevelConfig.name + "/" + saveFileName, Dto());
        }

        private GridDto Dto()
        {
            List<string> gridTiles = new List<string>(); 
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                gridTiles.Add(hexGrid.GetTile(x, y)?.name);
            }

            return new GridDto {objects = gridTiles.ToArray()};
        }
        
        public override void Load()
        {
            GridDto dtos = Saver.Read<GridDto>(LevelConfig.name + "/" + saveFileName);
            int id = 0;
            for (int y = 0; y < gridBase.YSize; y++)
            for (int x = 0; x < gridBase.XSize; x++)
            {
                Terrain template = GetTemplate(dtos.objects[id++]);
                hexGrid.SetTile(x, y, template is null ? null : CreateObject(x, y, template));
            }
        }
    }
}