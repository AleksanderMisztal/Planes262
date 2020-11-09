using GameDataStructures.Dtos;
using GameJudge.Troops;
using Planes262.HexSystem;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityFighter : Fighter
    {
        public static Effects effects;
        public static GridBase gridBase;
        
        private readonly Transform go;
        private readonly Transform body;
        private readonly GameObject active;
        
        private readonly SpriteHolder spriteHolder;
        private readonly SpriteRenderer spriteRenderer;

        public UnityFighter(SpriteHolder spriteHolder, TroopDto dto) : base(dto)
        {
            this.spriteHolder = spriteHolder;
            
            go = spriteHolder.transform;
            go.position = gridBase.ToWorld(Position);
            
            body = go.Find("Body");
            active = go.Find("Active").gameObject;
            active.SetActive(false);
            body.Rotate(Vector3.forward * (60 * Orientation - 30));
            
            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteHolder.sprites[spriteHolder.sprites.Length - 1];
        }

        public override void ResetMovePoints()
        {
            base.ResetMovePoints();
            active.SetActive(true);
        }

        public override void MoveInDirection(int direction)
        {
            base.MoveInDirection(direction);
            body.Rotate(Vector3.forward * 60 * direction);
            go.position = gridBase.ToWorld(Position);

            if (MovePoints == 0) active.SetActive(false);
        }

        public override void FlyOverOtherTroop()
        {
            base.FlyOverOtherTroop();
            go.position = gridBase.ToWorld(Position);
        }

        public override void ApplyDamage()
        {
            base.ApplyDamage();
            effects.Explode(go.position, 2);
            if (Health > 0) spriteRenderer.sprite = spriteHolder.sprites[Health - 1];
            else Object.Destroy(go.gameObject);

            if (MovePoints == 0) active.SetActive(false);
        }
    }
}
