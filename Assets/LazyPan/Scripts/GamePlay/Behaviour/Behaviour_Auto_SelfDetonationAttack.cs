using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_SelfDetonationAttack : Behaviour {
        public Behaviour_Auto_SelfDetonationAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP)).OnTriggerEnterEvent
                .AddListener(OnHitTriggerEnter);
        }

        private void OnHitTriggerEnter(Collider arg0) {
            if (arg0.gameObject.layer != LayerMask.NameToLayer("FriendSideBeHit")) {
                return;
            }

            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Building" ||
                    tmpEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    Entity beInjuredEntity = Cond.Instance.GetPlayerEntity();
                    bool isGetFlow = Flo.Instance.GetFlow(out Flow_Battle battleFlow);
                    if (isGetFlow) {
                        bool getSetting = Loader.LoadSetting().TryGetRobotBySign(entity.ObjConfig.Sign, out RobotSettingInfo i);
                        if (getSetting) {
                            MessageRegister.Instance.Dis(MessageCode.BeInjuried, beInjuredEntity, i.Attack);
                            entity.EntityData.BaseRuntimeData.RobotInfo.DeathType = 1;
                            MessageRegister.Instance.Dis(MessageCode.BeInjuried, entity,
                                entity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint);
                        }

                        Comp battleui = battleFlow.GetUI();
                        Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                        Cond.Instance.Get<Slider>(info, Label.HEALTH).value = beInjuredEntity.EntityData.BaseRuntimeData.CurHealth /
                                                                              beInjuredEntity.EntityData.BaseRuntimeData
                                                                                  .CurHealthMax;
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP)).OnTriggerEnterEvent
                .RemoveListener(OnHitTriggerEnter);
        }
    }
}