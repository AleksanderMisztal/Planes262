using Planes262.GameLogic;
using Planes262.GameLogic.Troops;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityTroop : Troop
    {
        public static Effects effects;
        public static MapGrid mapGrid;
        
        private readonly Transform go;
        private readonly Transform body;
        
        private readonly SpriteHolder spriteHolder;
        private readonly SpriteRenderer spriteRenderer;

        public UnityTroop(Troop troop, SpriteHolder spriteHolder) : base(troop)
        {
            this.spriteHolder = spriteHolder;
            
            go = spriteHolder.transform;
            go.position = mapGrid.CellToWorld(Position);
            
            body = go.Find("Body");
            body.Rotate(Vector3.forward * 60 * Orientation);
            
            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteHolder.sprites[spriteHolder.sprites.Length - 1];
        }

        public override void MoveInDirection(int direction)
        {
            base.MoveInDirection(direction);
            body.Rotate(Vector3.forward * 60 * direction);
            go.position = mapGrid.CellToWorld(Position);
        }

        public override void FlyOverOtherTroop()
        {
            base.FlyOverOtherTroop();
            go.position = mapGrid.CellToWorld(Position);
        }

        public override void ApplyDamage()
        {
            base.ApplyDamage();
            effects.Explode(go.position, 2);
            if (Health > 0) spriteRenderer.sprite = spriteHolder.sprites[Health - 1];
            else Object.Destroy(go.gameObject);
        }

        public override void CleanUpSelf()
        {
            Object.Destroy(go.gameObject);
        }
    }
}
