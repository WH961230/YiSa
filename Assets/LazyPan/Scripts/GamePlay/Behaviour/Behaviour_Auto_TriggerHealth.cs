using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TriggerHealth : Behaviour {
        private Comp battleui;
        private Flow_Battle battleFlow;
        private bool isHealthing;
        public Behaviour_Auto_TriggerHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.AddListener(HealthIn);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.AddListener(HealthOut);
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                battleui = battleFlow.GetUI();
            }
            Data.Instance.OnUpdateEvent.AddListener(Health);
        }

        /*离开回血区域*/
        private void HealthOut(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isHealthing = false;
                }
            }
        }

        /*进入回血区域*/
        private void HealthIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isHealthing = true;
                }
            }
        }

        /*血量回复*/
        private void Health() {
            if (isHealthing) {
                //加血
                entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint +=
                    Loader.LoadSetting().PlayerSetting.HealthRecoverSpeed * Time.deltaTime;
                //血量上限
                entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint = Mathf.Min(
                    entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint,
                    Loader.LoadSetting().PlayerSetting.MaxHealth);
            }

            //血条展示
            Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
            Cond.Instance.Get<Slider>(info, Label.HEALTH).value = entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint /
                                                                  Loader.LoadSetting().PlayerSetting.MaxHealth;
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Health);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.RemoveListener(HealthIn);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.RemoveListener(HealthOut);
        }
    }
}