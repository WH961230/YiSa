using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_SelfDetonationAttack : Behaviour {
        public Behaviour_Auto_SelfDetonationAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP)).OnControllerColliderHitEvent
                .AddListener(OnHitTrigger);
        }

        private void OnHitTrigger(ControllerColliderHit hit) {
            if (hit.gameObject.layer != LayerMask.NameToLayer("FriendSideBeHit")) {
                return;
            }

            if (Data.Instance.TryGetEntityByBodyPrefabID(hit.gameObject.GetInstanceID(), out Entity tmpEntity)) {
                Debug.Log("hit GO:" + tmpEntity.EntityData.BaseRuntimeData.Type);

                Entity towerEntity = null;
                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Building") {
                    towerEntity = tmpEntity;
                }

                if (tmpEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    Cond.Instance.GetTowerEntity(out towerEntity);
                }

                if (towerEntity != null) {
                    towerEntity.EntityData.BaseRuntimeData.CurHealth -= entity.EntityData.BaseRuntimeData.CurAttack;
                    bool isGetFlow = Flo.Instance.GetFlow(out Flow_Battle battleFlow);
                    if (isGetFlow) {
                        Comp battleui = battleFlow.GetUI();
                        Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                        Cond.Instance.Get<Slider>(info, Label.HEALTH).value = entity.EntityData.BaseRuntimeData.CurHealth /
                                                                              entity.EntityData.BaseRuntimeData
                                                                                  .CurHealthMax;
                    }
                    Debug.Log($"血量:{towerEntity.EntityData.BaseRuntimeData.CurHealth}");
                    bool hasFightFlow = Flo.Instance.GetFlow(out Flow_Battle fight);
                    if (hasFightFlow) {
                        fight.RemoveRobot(entity);
                        fight.AddRobot();
                        if (towerEntity.EntityData.BaseRuntimeData.CurHealth <= 0) {
                            Debug.LogError("游戏结束");
                            fight.Next();
                        }
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
        }
    }
}