using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_SelfDetonationAttack : Behaviour {
        public Behaviour_Auto_SelfDetonationAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Comp bodyComp = Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP));
            Cond.Instance.Get<Comp>(bodyComp, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnHitTriggerEnter);
        }

        private void OnHitTriggerEnter(Collider arg0) {
            if (arg0.gameObject.layer != LayerMask.NameToLayer("FriendSideBeHit")) {
                return;
            }

            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Tower" ||
                    tmpEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    Entity playerEntity = Cond.Instance.GetPlayerEntity();
                    bool isGetFlow = Flo.Instance.GetFlow(out Flow_Battle battleFlow);
                    if (isGetFlow) {
                        bool getSetting = Loader.LoadSetting().TryGetRobotBySign(entity.ObjConfig.Sign, out RobotSettingInfo i);
                        if (getSetting) {
                            /*对玩家造成伤害*/
                            MessageRegister.Instance.Dis(MessageCode.BeInjuried, playerEntity, i.Attack);
                            /*碰撞到塔触发自爆*/
                            if (tmpEntity.EntityData.BaseRuntimeData.Type == "Tower") {
                                MessageRegister.Instance.Dis(MessageCode.BeSelfDetonation, entity);
                            }
                        }
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Comp bodyComp = Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP));
            Cond.Instance.Get<Comp>(bodyComp, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnHitTriggerEnter);
        }
    }
}