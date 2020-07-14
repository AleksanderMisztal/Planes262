using Cysharp.Threading.Tasks;
using Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.GameLogic
{
    public class Troop : MonoBehaviour
    {
        private GridLayout gridLayout;
        private Animator animator;
        private Animator bodyAnimator;
        private TextMesh movePointsText;
        private Transform body;

        public PlayerId ControllingPlayer { get; private set; }
        public int Health { get; private set; }

        public int InitialMovePoints { get; private set; }
        public int MovePoints { get; private set; }

        public Vector2Int Position { get; private set; }
        public Vector2Int StartingPosition { get; set; }
        public int Orientation { get; private set; }

        private Queue<int> moveQueue = new Queue<int>();
        private Vector3 target;
        private int spriteOrientation;
        private Vector2Int spritePosition;
        private float speed = 4f;
        private bool isCloseToTarget = true;


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

            target = transform.position;
            spritePosition = Position;
            spriteOrientation = Orientation;

            body = transform.Find("Body");
            body.Find("Damaged").gameObject.SetActive(false);
            bodyAnimator = body.GetComponent<Animator>();
        }


        private void Update()
        {
            isCloseToTarget = (transform.position - target).sqrMagnitude < 0.01;
            if (isCloseToTarget && moveQueue.Count == 0)
            {
                transform.position = target;
                bodyAnimator.SetBool("isIdle", true);
                return;
            }
            if (isCloseToTarget)
            {
                int direction = moveQueue.Dequeue();
                spriteOrientation = (6 + spriteOrientation + direction) % 6;
                spritePosition = Hex.GetAdjacentHex(spritePosition, spriteOrientation);
                target = gridLayout.CellToWorld((Vector3Int)spritePosition);

                int orientationModifier = ControllingPlayer == PlayerId.Blue ? 0 : 3;
                transform.Find("Body").transform.rotation = Quaternion.identity;
                transform.Find("Body").transform.Rotate(Vector3.forward * 60 * (spriteOrientation + orientationModifier));
            }

            transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
        }

        public UniTask JumpForward()
        {
            Debug.Log("Jumping forward");
            Position = Hex.GetAdjacentHex(Position, Orientation);

            moveQueue.Enqueue(0);
            bodyAnimator.SetBool("isIdle", false);

            return UniTask.WaitUntil(() => isCloseToTarget);
            //MatchWorldPosition();
        }

        public UniTask MoveInDirection(int direction)
        {
            Debug.Log("Moving forward");
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

            moveQueue.Enqueue(direction);
            bodyAnimator.SetBool("isIdle", false);

            return UniTask.WaitUntil(() => isCloseToTarget);
        }

        public Vector2Int GetAdjacentHex(int direction)
        {
            direction = (6 + Orientation + direction) % 6;
            return Hex.GetAdjacentHex(Position, direction);
        }

        public bool ApplyDamage()
        {
            EffectManager.InstantiateExplosion(transform.position + Random.insideUnitSphere);
            EffectManager.InstantiateExplosion(transform.position + Random.insideUnitSphere);

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

        private void MatchSpriteToHealth()
        {
            if (Health == 1)
            {
                body.Find("Healthy").gameObject.SetActive(false);
                body.Find("Damaged").gameObject.SetActive(true);
            }
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
