using System.Security.Cryptography;
using Planes262.GameLogic.Utils;
using Planes262.UnityLayer.Utils;
using UnityEngine;

namespace Planes262.UnityLayer
{
    public class TroopGO : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        private Transform body;
        private SpriteRenderer spriteRenderer;
        private static MapGrid mapGrid;

        public static void Inject(MapGrid mapGrid)
        {
            TroopGO.mapGrid = mapGrid;
        }
        
        public void Initialize()
        {
            body = transform.Find("Body");
            spriteRenderer = body.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[sprites.Length - 1];
        }

        public void SetPosition(VectorTwo position)
        {
            transform.position = mapGrid.CellToWorld(position);
        }

        public void Rotate(int direction)
        {
            body.Rotate(Vector3.forward * 60 * direction);
        }

        public void SetSprite(int health)
        {
            spriteRenderer.sprite = sprites[health - 1];
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}