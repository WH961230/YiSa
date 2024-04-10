using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_DropExperience : Behaviour {
        public Behaviour_Auto_DropExperience(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathDrop, DeathDrop);
        }

        /*死亡掉落*/
        private void DeathDrop(Entity deadEntity) {
            if (deadEntity.ID == entity.ID) {
                if (deadEntity.EntityData.BaseRuntimeData.RobotInfo.DeathDropType < 33) {
                    ConsoleEx.Instance.Content("log", $"[{deadEntity.ID}] 死亡掉落经验值");
                    GameObject template = Loader.LoadGo("经验值", "Common/Obj_Fx_Drop", Data.Instance.ObjRoot, true);
                    Comp comp = template.GetComponent<Comp>();
                    Cond.Instance.Get<Transform>(comp, Label.FOOT).position = Cond.Instance.Get<Transform>(deadEntity, Label.BODY).position;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathDrop, DeathDrop);
        }
    }
}