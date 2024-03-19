using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TriggerHealth : Behaviour {
        private Comp battleui;
        private Flow_Battle battleFlow;
        public Behaviour_Auto_TriggerHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.AddListener(HealthIn);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.AddListener(HealthOut);
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                battleui = battleFlow.GetUI();
            }
            Data.Instance.OnUpdateEvent.AddListener(Health);
        }

        private void HealthOut(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsHealthing = false;
                }
            }
        }

        private void HealthIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsHealthing = true;
                }
            }
        }

        private void Health() {
            if (entity.EntityData.BaseRuntimeData.CurIsHealthing) {
                //加血
                entity.EntityData.BaseRuntimeData.CurHealth +=
                    entity.EntityData.BaseRuntimeData.CurHealthSpeed * Time.deltaTime;
                //血量上限
                entity.EntityData.BaseRuntimeData.CurHealth = Mathf.Min(entity.EntityData.BaseRuntimeData.CurHealth,
                    entity.EntityData.BaseRuntimeData.CurHealthMax);
            }

            //血条展示
            if (entity.EntityData.BaseRuntimeData.CurHealth > 0) {
                Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                Cond.Instance.Get<Slider>(info, Label.HEALTH).value = entity.EntityData.BaseRuntimeData.CurHealth /
                                                                          entity.EntityData.BaseRuntimeData
                                                                              .CurHealthMax;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Health);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.RemoveListener(HealthIn);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.RemoveListener(HealthOut);
        }
    }
}