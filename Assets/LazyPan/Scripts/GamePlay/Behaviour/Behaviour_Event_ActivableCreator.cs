using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_ActivableCreator : Behaviour {
        private Flow_Battle flow;
        private List<Entity> activableEventEntities;
        private Queue<PointData> activableEventQueuies;

        public Behaviour_Event_ActivableCreator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            activableEventEntities = new List<Entity>();
            activableEventQueuies = new Queue<PointData>();
            //可激活事件
            Data.Instance.ActivableEvents = new List<PointData>();
            Data.Instance.ActivableEvents.Add(new PointData() {
                Sign = "Obj_Activable_Activable",
                Point = Vector3.zero,
            });
            Prepare();
            Instance();
            MessageRegister.Instance.Reg<Entity>(MessageCode.Dead, DeadDrop);
        }

        /*死亡掉落*/
        private void DeadDrop(Entity deadEntity) {
            if (deadEntity.EntityData.BaseRuntimeData.DeathDrop == 2) {
                ConsoleEx.Instance.Content("log", $"[{deadEntity.ID}] 死亡掉落可激活");
                Data.Instance.ActivableEvents.Clear();
                Data.Instance.ActivableEvents.Add(new PointData() {
                    Sign = "Obj_Activable_Activable",
                    Point = Cond.Instance.Get<Transform>(deadEntity, Label.BODY).position,
                });
                Prepare();
                Instance();
                deadEntity.EntityData.BaseRuntimeData.DeathDrop = 0;
            }
        }

        /*预备*/
        private void Prepare() {
            foreach (PointData data in Data.Instance.ActivableEvents) {
                activableEventQueuies.Enqueue(data);
            }
        }

        /*生成*/
        private void Instance() {
            if (activableEventQueuies.Count > 0) {
                PointData data = activableEventQueuies.Dequeue();
                Entity instance = Obj.Instance.LoadEntity(data.Sign);
                if (data.Point != Vector3.zero) {
                    Cond.Instance.Get<Transform>(instance, Label.FOOT).position = data.Point;
                }
                activableEventEntities.Add(instance);
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.Dead, DeadDrop);
        }
    }
}