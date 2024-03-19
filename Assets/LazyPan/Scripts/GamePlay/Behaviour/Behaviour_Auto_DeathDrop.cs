using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_DeathDrop : Behaviour {
        private Vector3 dropPos;
        public Behaviour_Auto_DeathDrop(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.Dead, DeathDrop);
        }

        private void DeathDrop(Entity deadEntity) {
            if (!deadEntity.EntityData.BaseRuntimeData.DeathDrop) {
                GameObject template = Loader.LoadGo("经验值", "Obj/Battle/Obj_Fx_Drop", Data.Instance.ObjRoot, true);
                Comp comp = template.GetComponent<Comp>();
                Cond.Instance.Get<Transform>(comp, Label.FOOT).position = Cond.Instance.Get<Transform>(deadEntity, Label.BODY).position;
                deadEntity.EntityData.BaseRuntimeData.DeathDrop = true;
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.Dead, DeathDrop);
        }
    }
}