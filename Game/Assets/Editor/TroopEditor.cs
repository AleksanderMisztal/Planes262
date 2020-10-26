using System.Collections.Generic;
using Planes262.LevelEditor.Troops;
using UnityEditor;
using UnityEngine;

namespace LevelEditor.Editor
{
    [CustomEditor(typeof(TroopTemplate), true)]
    public class TroopEditor : UnityEditor.Editor
    {
        private List<Sprite> maxSprites;

        private void OnEnable()
        {
            maxSprites = new List<Sprite>();
        }

        private void OnDisable()
        {
            maxSprites.Clear();
        }

        public override void OnInspectorGUI()
        {
            TroopTemplate fighter = (TroopTemplate) target;
            fighter.movePoints = EditorGUILayout.IntField("Move Points", fighter.movePoints);
            fighter.health = EditorGUILayout.IntField("Health", fighter.health);
            if (fighter.health < 1) fighter.health = 1;

            while (fighter.health > fighter.sprites.Count)
            {
                Sprite add = maxSprites.Count <= fighter.sprites.Count ? null : maxSprites[fighter.sprites.Count];
                fighter.sprites.Add(add);
            }
            while (fighter.health < fighter.sprites.Count) fighter.sprites.RemoveAt(fighter.sprites.Count - 1);

            for (int h = fighter.health; h > 0; h--)
            {
                fighter.sprites[h-1] = (Sprite) EditorGUILayout.ObjectField("Health = " + h, fighter.sprites[h-1], typeof(Sprite), false);
                if (fighter.sprites[h - 1] == null) continue;
                while (maxSprites.Count < h) maxSprites.Add(null);
                maxSprites[h - 1] = fighter.sprites[h - 1];
            }
            
            if (GUI.changed) EditorUtility.SetDirty(fighter);
        }
    }
}
