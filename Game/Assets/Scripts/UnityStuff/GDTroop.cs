using GameServer.GameLogic;
using GameServer.Utils;
using UnityEngine;

namespace Assets.Scripts.UnityStuff
{
    class GDTroop : MonoBehaviour
    {
        private const int NO_EXPLOSIONS = 2;

        private PlayerSide side;
        public VectorTwo Position { get; private set; }
        private int orientation;
        public int health;
        public bool Destroyed => health <= 0;

        [SerializeField] private Sprite[] sprites;
        private Transform body;
        private SpriteRenderer spriteRenderer;

        public void Initialize(VectorTwo position, int orientation, PlayerSide side)
        {
            this.side = side;
            this.Position = position;
            this.orientation = orientation;
            body = transform.Find("Body");

            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[sprites.Length - 1];

            transform.position = MapGrid.CellToWorld(position); 
            int modifier = side == PlayerSide.Red ? 3 : 0;
            body.rotation = Quaternion.identity;
            body.Rotate(Vector3.forward * 60 * (orientation + modifier));
        }

        public VectorTwo CellInFront => Hex.GetAdjacentHex(Position, orientation);

        public void AdjustOrientation(int direction)
        {
            if (health <= 0) return;
            orientation += direction;
            int modifier = side == PlayerSide.Red ? 3 : 0;
            body.rotation = Quaternion.identity;
            body.Rotate(Vector3.forward * 60 * (orientation + modifier));
        }

        public void MoveForward()
        {
            if (health <= 0) return;
            Position = CellInFront;
            transform.position = MapGrid.CellToWorld(Position);
        }

        public void ApplyDamage()
        {
            Effects.Explode(transform.position, NO_EXPLOSIONS);
            MatchSpriteToHealth();
        }

        private void MatchSpriteToHealth()
        {
            if (health --> 0)
                spriteRenderer.sprite = sprites[health - 1];
            else
                Destroy(this);
        }
    }
}
