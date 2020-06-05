using UnityEngine;

namespace Scripts.GameLogic
{
    public class SpawnTemplate
    {
        public PlayerId controllingPlayer;
        public int movePoints;
        public int health;
        public int orientation;

        public Vector2Int position;

        //TODO: add unit type to be used on frontend

        public SpawnTemplate(PlayerId controllingPlayer, int movePoints, int health, int orientation, Vector2Int position)
        {
            this.controllingPlayer = controllingPlayer;
            this.movePoints = movePoints;
            this.health = health;
            this.orientation = orientation;
            this.position = position;
        }
    }
}
