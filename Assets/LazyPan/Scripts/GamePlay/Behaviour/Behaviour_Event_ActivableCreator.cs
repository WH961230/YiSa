using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_ActivableCreator : Behaviour {
        private Flow_Battle flow;
        private List<Entity> activableEventEntities;

        public Behaviour_Event_ActivableCreator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            activableEventEntities = new List<Entity>();
            //可激活事件
            Instance("Obj_Activable_Activable", Vector3.zero);
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathDrop, DeadDrop);
        }

        /*死亡掉落*/
        private void DeadDrop(Entity deadEntity) {
            if (deadEntity.EntityData.BaseRuntimeData.RobotInfo.DeathDropType >= 95) {
                ConsoleEx.Instance.Content("log", $"[{deadEntity.ID}] 死亡掉落可激活");
                Instance("Obj_Activable_Activable", Cond.Instance.Get<Transform>(deadEntity, Label.BODY).position);
            }
        }

        /*生成*/
        private void Instance(string sign, Vector3 point) {
            Entity instance = Obj.Instance.LoadEntity(sign);
            if (point != Vector3.zero) {
                Cond.Instance.Get<Transform>(instance, Label.FOOT).position = point;
            }
            activableEventEntities.Add(instance);
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathDrop, DeadDrop);
        }
    }
}