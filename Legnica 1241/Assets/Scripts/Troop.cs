using UnityEngine;

public class Troop : MonoBehaviour
{
    // Refs
    GameController gameController;
    GridLayout gridLayout;
    Animator animator;
    TextMesh movePointsText;
    ScoreController scoreController;

    // Meta
    [SerializeField] int initialMovePoints = 5;
    [SerializeField] public PlayerID controllingPlayer;

    // State
    private Vector2Int position;
    [SerializeField] int orientation = 0;
    [SerializeField] int health = 2;
    int movePoints;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gridLayout = FindObjectOfType<GridLayout>();
        animator = GetComponent<Animator>();
        movePointsText = transform.Find("Move Points Text").GetComponent<TextMesh>();
        scoreController = FindObjectOfType<ScoreController>();
    }
    public void Initilalize(Vector2Int initialPosition)
    {
        position = initialPosition;
        transform.position = gridLayout.CellToWorld((Vector3Int)position);
        movePoints = initialMovePoints;
    }
    internal bool IsLegalMove(Vector2Int cell)
    {
        if (!InControlZone(cell) || !HasMovePoints()) return false;
        Troop encounter = gameController.GetTroopAt(cell);
        if (!encounter || IsEnemy(encounter)) return true;
        return false;
    }
    public int GetMovePoints()
    {
        return movePoints;
    }
    public int GetInitialMovePoints()
    {
        return initialMovePoints;
    }
    public bool HasMovePoints()
    {
        return movePoints > 0;
    }
    public void ApplyDamage()
    {
        health--;
        PlayerID oponent = controllingPlayer == PlayerID.Blue ? PlayerID.Red : PlayerID.Blue;
        scoreController.IncrementScore(oponent);
        if (health <= 0)
        {
            Debug.Log("Destroyed!  " + position);
            gameController.DestroyTroopAtPosition(position);
            Destroy(gameObject);
        }
        else 
        {
            initialMovePoints--;
            MatchSpriteToHealth();
            if (movePoints > 0)
                DecrementMovePoints();
        }
    }
    private void DecrementMovePoints()
    {
        movePoints--;
        gameController.DecrementMovePointsLeft();
        movePointsText.text = movePoints.ToString();
        if (movePoints <= 0)
        {
            transform.Find("Move Points Text").gameObject.SetActive(false);
            Desactivate();
        }
    }
    private void MatchSpriteToHealth()
    {
        if (health == 1)
        {
            if (controllingPlayer == PlayerID.Red)
                transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 127, 255);
            if (controllingPlayer == PlayerID.Blue)
                transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color32(127, 255, 255, 255);
        }
    }
    public void MoveTo(Vector2Int newPosition)
    {
        if (movePoints <= 0) return;
        DecrementMovePoints();
        int direction = ControlZoneDirection(newPosition);
        Vector2Int oldPosition = position;
        ChangeDirection(direction);
        JumpOtherTroops();
        if (health <= 0) return;
        transform.position = gridLayout.CellToWorld((Vector3Int)position);
        gameController.ChangeTroopPosition(oldPosition, position);
    }
    public void JumpOtherTroops()
    {
        Vector2Int tempPosition = Hex.GetAdjacentHex(position, orientation);
        Troop firstEncounterd = gameController.GetTroopAt(tempPosition);
        if (firstEncounterd && IsEnemy(firstEncounterd))
        {
            Debug.Log("About to fight");
            Battles.Fight(this, firstEncounterd);
            Debug.Log("Fight ended!");
            tempPosition = Hex.GetAdjacentHex(tempPosition, orientation);
        }
        Troop other = gameController.GetTroopAt(tempPosition);
        while (other)
        {
            Battles.Collide(this, other);
            tempPosition = Hex.GetAdjacentHex(tempPosition, orientation);
            other = gameController.GetTroopAt(tempPosition);
        }
        position = tempPosition;
    }
    private bool IsEnemy(Troop other)
    {
        return other.controllingPlayer != controllingPlayer;
    }
    public bool InControlZone(Vector2Int cell)
    {
        for (int rotation = -1; rotation <= 1; rotation++)
        {
            int dir = (6 + orientation + rotation) % 6;
            Vector2Int controlledCell = Hex.GetAdjacentHex(position, dir);
            if (cell == controlledCell) return true;
        }
        return false;
    }
    public int ControlZoneDirection(Vector2Int cell)
    {
        for (int rotation = -1; rotation <= 1; rotation++)
        {
            int dir = (6 + orientation + rotation) % 6;
            Vector2Int controlledCell = Hex.GetAdjacentHex(position, dir);
            if (cell == controlledCell) return rotation;
        }
        Debug.Log("Something wrong with control zone direction.");
        return 42;
    }
    public void ChangeDirection(int rotation)
    {
        transform.Find("Body").transform.Rotate(Vector3.forward * 60 * rotation);
        transform.Find("Direction Arrow").transform.Rotate(Vector3.forward * 60 * rotation);
        orientation = (6 + orientation + rotation) % 6;
    }
    public Vector2Int GetPosition() 
    { 
        return position; 
    }
    public void HandleTurnBegin()
    {
        if (health <= 0) return;
        transform.Find("Move Points Text").gameObject.SetActive(true);
        movePoints = initialMovePoints;
        movePointsText.text = movePoints.ToString();
    }
    public void HandleTurnEnd()
    {
        transform.Find("Move Points Text").gameObject.SetActive(false);
    }
    public void Activate()
    {
        animator.SetBool("isActive", true);
    }
    public void Desactivate()
    {
        animator.SetBool("isActive", false);
    }
}
