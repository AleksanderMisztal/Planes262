using Planes262.LevelEditor.Tilemaps;
using UnityEngine;

namespace Planes262.LevelEditor.Tools
{
    public abstract class HexTool : MonoBehaviour
    {
        public abstract bool Enabled { protected get; set; }
        public abstract void Initialize(ResizableGridBase theGridBase);
        public abstract void Load();
        public abstract void Save();
    }
}