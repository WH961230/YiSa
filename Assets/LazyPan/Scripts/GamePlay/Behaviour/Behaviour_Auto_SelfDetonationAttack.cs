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
                Entity towerEntity = null;
                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Building") {
                    towerEntity = tmpEntity;
                }

                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    Cond.Instance.GetTowerEntity(out towerEntity);
                }

                if (towerEntity != null) {
                    towerEntity.EntityData.BaseRuntimeData.CurHealth -= entity.EntityData.BaseRuntimeData.CurAttack;

                    Debug.Log($"血量:{towerEntity.EntityData.BaseRuntimeData.CurHealth}");
                    bool isGetFlow = Flo.Instance.GetFlow(out Flow_Battle battleFlow);
                    if (isGetFlow) {
                        MessageRegister.Instance.Dis(MessageCode.DeadRecycle, tmpEntity);

                        Comp battleui = battleFlow.GetUI();
                        Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                        Cond.Instance.Get<Slider>(info, Label.HEALTH).value = towerEntity.EntityData.BaseRuntimeData.CurHealth /
                                                                              towerEntity.EntityData.BaseRuntimeData
                                                                                  .CurHealthMax;
                        if (towerEntity.EntityData.BaseRuntimeData.CurHealth <= 0) {
                            MessageRegister.Instance.Dis(MessageCode.GameOver);
                            Data.Instance.GameOver = true;
                        }
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