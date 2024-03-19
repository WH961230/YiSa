using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_DeathDrop : Behaviour {
        private Vector3 dropPos;
        public Behaviour_Auto_DeathDrop(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.Dead, DeathDrop);
        }

        private void DeathDrop(Entity obj) {
            GameObject template = Loader.LoadGo("经验值", "Obj/Battle/Obj_Fx_Drop", Data.Instance.ObjRoot, true);
            template.transform.position = obj.Prefab.transform.position;
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.Dead, DeathDrop);
        }
    }
}