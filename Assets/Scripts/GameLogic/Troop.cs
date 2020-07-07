using Scripts.Utils;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class Troop : MonoBehaviour
    {
        private GridLayout gridLayout;
        private Animator animator;
        private TextMesh movePointsText;
        private Transform body;

        public void Initilalize(SpawnTemplate spawn)
        {
            gridLayout = FindObjectOfType<Grid>();
            animator = GetComponent<Animator>();
            movePointsText = transform.Find("Move Points Text").GetComponent<TextMesh>();

            ControllingPlayer = spawn.controllingPlayer;
            InitialMovePoints = spawn.movePoints;
            Health = spawn.health;
            Orientation = spawn.orientation;

            Position = spawn.position;
            StartingPosition = Position;
            transform.position = gridLayout.CellToWorld((Vector3Int)spawn.position);

            body = transform.Find("Body");
            body.Find("Damaged").gameObject.SetActive(false);
        }

        private void MatchWorldPosition()
        {
            transform.position = gridLayout.CellToWorld((Vector3Int)Position);

            int orientationModifier = ControllingPlayer == PlayerId.Blue ? 0 : 3;

            transform.Find("Body").transform.rotation = Quaternion.identity;
            transform.Find("Body").transform.Rotate(Vector3.forward * 60 * (Orientation + orientationModifier));
        }

        private void MatchSpriteToHealth()
        {
            if (Health == 1)
            {
                body.Find("Healthy").gameObject.SetActive(false);
                body.Find("Damaged").gameObject.SetActive(true);
            }
        }


        public PlayerId ControllingPlayer { get; private set; }
        public int Health { get; private set; }

        public int InitialMovePoints { get; private set; }
        public int MovePoints { get; private set; }

        public Vector2Int Position { get; private set; }
        public Vector2Int StartingPosition { get; set; }
        public int Orientation { get; private set; }


        public void JumpForward()
        {
            Position = Hex.GetAdjacentHex(Position, Orientation);

            MatchWorldPosition();
        }

        public void MoveInDirection(int direction)
        {
            if (MovePoints < 0)
            {
                throw new IllegalMoveException("Attempting to move a troop with no move points!");
            }

            if (MovePoints > 0)
            {
                MovePoints--;
                movePointsText.text = MovePoints.ToString();
            }

            if (MovePoints <= 0)
            {
                movePointsText.gameObject.SetActive(false);
            }

            Orientation = (6 + Orientation + direction) % 6;
            Position = Hex.GetAdjacentHex(Position, Orientation);

            MatchWorldPosition();
        }

        public Vector2Int GetAdjacentHex(int direction)
        {
            direction = (6 + Orientation + direction) % 6;
            return Hex.GetAdjacentHex(Position, direction);
        }

        public bool ApplyDamage()
        {
            Health--;
            MatchSpriteToHealth();
            if (Health > 0)
            {
                InitialMovePoints--;
                MovePoints--;
                movePointsText.text = MovePoints.ToString();
                if (MovePoints <= 0)
                {
                    movePointsText.gameObject.SetActive(false);
                }
            }
            else
            {
                Destroy(gameObject);
            }
            return Health <= 0;
        }

        public bool InControlZone(Vector2Int cell)
        {
            for (int rotation = -1; rotation <= 1; rotation++)
            {
                int dir = (6 + Orientation + rotation) % 6;
                Vector2Int controlledCell = Hex.GetAdjacentHex(Position, dir);
                // Potential source of bugs but should work with == overloaded
                if (cell == controlledCell) return true;
            }
            return false;
        }

        public void Activate()
        {
            animator.SetBool("isActive", true);
        }

        public void Deactivate()
        {
            animator.SetBool("isActive", false);
            TileManager.DeactivateTiles();
        }

        public void OnTurnBegin()
        {
            //if (Health <= 0) return;
            MovePoints = InitialMovePoints;
            movePointsText.text = MovePoints.ToString();
            movePointsText.gameObject.SetActive(true);
        }

        public void OnTurnEnd()
        {
            movePointsText.gameObject.SetActive(false);

            StartingPosition = Position;
        }


        public override string ToString()
        {
            return $"cp: {ControllingPlayer}, p: {Position}, o: {Orientation}, imp: {InitialMovePoints}, mp: {MovePoints}, h: {Health}";
        }
    }
}
