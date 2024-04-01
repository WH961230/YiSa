using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_DropExperience : Behaviour {
        public Behaviour_Event_DropExperience(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathDrop, DeathDrop);
        }

        private void DeathDrop(Entity deadEntity) {
            if (deadEntity.EntityData.BaseRuntimeData.RobotInfo.DeathDropType == 1) {
                ConsoleEx.Instance.Content("log", $"[{deadEntity.ID}] 死亡掉落经验值");
                GameObject template = Loader.LoadGo("经验值", "Common/Obj_Fx_Drop", Data.Instance.ObjRoot, true);
                Comp comp = template.GetComponent<Comp>();
                Cond.Instance.Get<Transform>(comp, Label.FOOT).position = Cond.Instance.Get<Transform>(deadEntity, Label.BODY).position;
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathDrop, DeathDrop);
        }
    }
}