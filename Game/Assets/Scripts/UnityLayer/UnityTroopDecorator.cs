using GameDataStructures;
using GameJudge.Troops;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityTroopDecorator : TroopDecorator
    {
        public static Effects effects;
        public static MapGrid mapGrid;
        
        private readonly Transform go;
        private readonly Transform body;
        
        private readonly SpriteHolder spriteHolder;
        private readonly SpriteRenderer spriteRenderer;

        public UnityTroopDecorator(SpriteHolder spriteHolder, ITroop troop) : base(troop)
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
            Debug.Log("Troop: moving from " + Position);
            base.MoveInDirection(direction);
            body.Rotate(Vector3.forward * 60 * direction);
            go.position = mapGrid.CellToWorld(Position);
            MyLogger.Log("New position is " + Position);
        }

        public override void FlyOverOtherTroop()
        {
            Debug.Log("Troop: flying");
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
            base.CleanUpSelf();
            Object.Destroy(go.gameObject);
        }
    }
}
