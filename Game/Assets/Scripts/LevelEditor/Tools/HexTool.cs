using System.Collections.Generic;
using GameDataStructures.Positioning;
using Planes262.LevelEditor.Tilemaps;
using Planes262.Saving;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public abstract class HexTool<T> : HexTool where T : class
    {
        // ReSharper disable once Unity.RedundantSerializeFieldAttribute
        [SerializeField] private T[] templates;

        private readonly Dictionary<string, T> templateByName = new Dictionary<string, T>();
        protected abstract string GetName(T template);
        private int activeId;

        protected HexGrid hexGrid;
        protected ResizableGridBase gridBase;
        
        public override bool Enabled { protected get; set; }

        protected T GetTemplate(string templateName)
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

        private class TemplatesDto
        {
            public T[] templates;
        }
        
        public override void Initialize(ResizableGridBase theGridBase)
        {
            theGridBase.GridResized += Resize;
            gridBase = theGridBase;
            hexGrid = new HexGrid(theGridBase);
            foreach (T template in templates)
                templateByName.Add(GetName(template), template);
            
            if (!LevelConfig.isLoaded) Save();
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

        protected GameObject CreateObject(int x, int y, T template)
        {
            Vector3 position = gridBase.ToWorld(x, y);
            return CreateObject(position, template);
        }

        protected abstract GameObject CreateObject(Vector3 position, T template);
    }
}