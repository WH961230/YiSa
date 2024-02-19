using UnityEngine;

namespace LazyPan {
    public class Behaviour_CloseCombat : Behaviour {
        public Behaviour_CloseCombat(Entity entity, string sign) : base(entity, sign) {
            Animator animator = entity.Comp.Get<Animator>("Animator");
            animator.SetInteger("WeaponType", entity.EntitySetting.WeaponType);
        }
    }
}