using Planes262.GameLogic.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    class GdTroop : MonoBehaviour
    {
        private const int NoExplosions = 2;

        public VectorTwo Position { get; private set; }
        private int orientation;
        private int health;
        public bool Destroyed => health <= 0;
        public VectorTwo CellInFront => Hex.GetAdjacentHex(Position, orientation);

        [SerializeField] private Sprite[] sprites;
        private Transform body;
        private SpriteRenderer spriteRenderer;

        public void Initialize(VectorTwo position, int orientation, int health)
        {
            this.Position = position;
            this.orientation = orientation;
            this.health = health;
            body = transform.Find("Body");

            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[sprites.Length - 1];

            transform.position = MapGrid.CellToWorld(position);
            body.rotation = Quaternion.identity;
            body.Rotate(Vector3.forward * 60 * orientation);
        }

        public void AdjustOrientation(int direction)
        {
            orientation += direction;
            body.Rotate(Vector3.forward * 60 * direction);
        }

        public void MoveForward()
        {
            Position = CellInFront;
            transform.position = MapGrid.CellToWorld(Position);
        }

        public void ApplyDamage()
        {
            Effects.Explode(transform.position, NoExplosions);
            if (--health > 0) spriteRenderer.sprite = sprites[health - 1];
            else Destroy(this);
        }
    }
}
