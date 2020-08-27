using Planes262.GameLogic.Utils;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class UnityTroop : MonoBehaviour
    {
        private static MapGrid mapGrid;
        private Effects effects;

        public static void Inject(MapGrid mapGrid)
        {
            UnityTroop.mapGrid = mapGrid;
        }
        
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
            effects = FindObjectOfType<Effects>();
            
            this.Position = position;
            this.orientation = orientation;
            this.health = health;
            body = transform.Find("Body");

            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[sprites.Length - 1];

            transform.position = mapGrid.CellToWorld(position);
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
            transform.position = mapGrid.CellToWorld(Position);
        }

        public void ApplyDamage()
        {
            effects.Explode(transform.position, 2);
            if (--health > 0) spriteRenderer.sprite = sprites[health - 1];
            else Destroy(this.gameObject);
        }
    }
}
