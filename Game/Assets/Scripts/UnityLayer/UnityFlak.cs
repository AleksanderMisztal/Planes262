using GameDataStructures;
using GameDataStructures.Dtos;
using GameJudge.Troops;
using Planes262.HexSystem;
using Planes262.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityFlak : Flak
    {
        public static GridBase gridBase;
        public static Effects effects;
        
        private readonly Transform go;
        private readonly GameObject active;

        public UnityFlak(SpriteHolder spriteHolder, TroopDto dto) : base(dto)
        {
            go = spriteHolder.transform;
            go.position = gridBase.ToWorld(Position);

            active = go.Find("Active").gameObject;
            active.GetComponent<SpriteRenderer>().color = Player == PlayerSide.Red ? Color.red : Color.blue;
            active.SetActive(false);
            SpriteRenderer spriteRenderer = go.Find("Body").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spriteHolder.sprites[0];
        }

        public override void ResetMovePoints()
        {
            base.ResetMovePoints();
            active.SetActive(true);
        }

        public override void MoveInDirection(int direction)
        {
            base.MoveInDirection(direction);
            go.position = gridBase.ToWorld(Position);
            if (MovePoints == 0) active.SetActive(false);
        }

        public override void FlyOverOtherTroop()
        {
            base.FlyOverOtherTroop();
            go.position = gridBase.ToWorld(Position);
        }

        public void Fire()
        {
            effects.Fire(go.position);
        }
    }
}
