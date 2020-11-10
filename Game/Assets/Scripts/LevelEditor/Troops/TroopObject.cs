using UnityEngine;

namespace Planes262.LevelEditor.Troops
{
    public class TroopObject : MonoBehaviour
    {
        public TroopTemplate template;
        public int Rotation { get; private set; }

        public void Rotate(int orientation = 1)
        {
            transform.Rotate(60 * orientation * Vector3.forward);
            Rotation += orientation;
            Rotation %= 6;
        }
    }
}