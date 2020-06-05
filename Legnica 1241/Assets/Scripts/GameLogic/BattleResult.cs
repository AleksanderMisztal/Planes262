using UnityEngine;

namespace Scripts.GameLogic
{
    public class BattleResult
    {
        public bool DefenderDamaged { get; set; }
        public bool AttackerDamaged { get; set; }

        public BattleResult(bool defenderDamaged, bool attackerDamaged)
        {
            DefenderDamaged = defenderDamaged;
            AttackerDamaged = attackerDamaged;
        }

        public BattleResult()
        {
            DefenderDamaged = false;
            AttackerDamaged = false;
        }
    }

    public class Battles
    {
        public static BattleResult GetFightResult(Troop attacker, Troop defender)
        {
            Debug.Log("Fighting...");

            BattleResult battleResult = new BattleResult();

            if (Random.Range(0, 6) < 3)
            {
                Debug.Log("Defender damaged!");
                battleResult.DefenderDamaged = true;
            }
            if (defender.InControlZone(attacker.Position) && Random.Range(0, 6) < 3)
            {
                Debug.Log("Attacker damaged!");
                battleResult.AttackerDamaged = true;
            }

            return battleResult;
        }

        public static BattleResult GetCollisionResult()
        {
            Debug.Log("Colliding...");

            BattleResult battleResult = new BattleResult();

            if (Random.Range(0, 6) + Random.Range(0, 6) == 10)
            {
                Debug.Log("Collision!");
                battleResult.AttackerDamaged = true;
                battleResult.DefenderDamaged = true;
            }

            return battleResult;
        }
    }
}
