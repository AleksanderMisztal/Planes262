using Scripts.Utils;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class Troop : MonoBehaviour
    {
        private GridLayout gridLayout;
        private Animator animator;
        private TextMesh movePointsText;

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
            transform.position = gridLayout.CellToWorld((Vector3Int)spawn.position);
        }

        private void MatchWorldPosition()
        {
            transform.position = gridLayout.CellToWorld((Vector3Int)Position);

            int orientationModifier = ControllingPlayer == PlayerId.Blue ? 0 : 3;

            transform.Find("Body").transform.rotation = Quaternion.identity;
            transform.Find("Body").transform.Rotate(Vector3.forward * 60 * (Orientation + orientationModifier));

            transform.Find("Direction Arrow").transform.rotation = Quaternion.identity;
            transform.Find("Direction Arrow").transform.Rotate(Vector3.forward * 60 * (Orientation + orientationModifier));
        }

        private void MatchSpriteToHealth()
        {
            if (Health == 1)
            {
                if (ControllingPlayer == PlayerId.Red)
                    transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 127, 255);
                if (ControllingPlayer == PlayerId.Blue)
                    transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color32(127, 255, 255, 255);
            }
        }


        public PlayerId ControllingPlayer { get; private set; }
        public int Health { get; private set; }

        public int InitialMovePoints { get; private set; }
        public int MovePoints { get; private set; }

        public Vector2Int Position { get; private set; }
        public int Orientation { get; private set; }


        public void JumpForward()
        {
            Position = Hex.GetAdjacentHex(Position, Orientation);

            MatchWorldPosition();
        }

        public void MoveInDirection(int direction)
        {
            if (MovePoints <= 0)
            {
                throw new IllegalMoveException("Attempting to move a NewTroop with no move points!");
            }

            MovePoints--;
            movePointsText.text = MovePoints.ToString();
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
                if (MovePoints > 0)
                {
                    MovePoints--;
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
            Debug.Log($"Activated troop has orientation {Orientation}.");
        }

        public void Desactivate()
        {
            animator.SetBool("isActive", false);
        }

        public void OnTurnBegin()
        {
            //if (Health <= 0) return;
            MovePoints = InitialMovePoints;
            movePointsText.text = MovePoints.ToString();
            movePointsText.gameObject.SetActive(true);

            //transform.Find("Move Points Text").gameObject.SetActive(true);
        }

        public void OnTurnEnd()
        {
            movePointsText.gameObject.SetActive(false);
        }


        public override string ToString()
        {
            return $"cp: {ControllingPlayer}, p: {Position}, o: {Orientation}, imp: {InitialMovePoints}, mp: {MovePoints}, h: {Health}";
        }
    }
}
