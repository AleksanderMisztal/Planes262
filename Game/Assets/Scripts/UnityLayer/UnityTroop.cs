using Planes262.GameLogic;
using Planes262.UnityLayer.Utils;

namespace Planes262.UnityLayer
{
    public class UnityTroop : Troop
    {
        private static Effects effects;

        public static void Inject(Effects effects)
        {
            UnityTroop.effects = effects;
        }

        private readonly TroopGO troopGO;

        public UnityTroop(Troop troop, TroopGO troopGO) : base(troop)
        {
            troopGO.Initialize();
            troopGO.SetPosition(Position);
            troopGO.Rotate(Orientation);
            this.troopGO = troopGO;
        }

        public override void MoveInDirection(int direction)
        {
            base.MoveInDirection(direction);
            troopGO.Rotate(direction);
            troopGO.SetPosition(Position);
        }

        public override void FlyOverOtherTroop()
        {
            base.FlyOverOtherTroop();
            troopGO.SetPosition(Position);
        }

        public override void ApplyDamage()
        {
            base.ApplyDamage();
            effects.Explode(troopGO.transform.position, 2);
            if (Health > 0) troopGO.SetSprite(Health);
            else troopGO.DestroySelf();
        }

        public override void CleanUpSelf()
        {
            troopGO.DestroySelf();
        }
    }
}
