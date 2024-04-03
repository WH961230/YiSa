using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame : Behaviour {
        private Flow_Begin beginFlow;
        private Image energyImage;
        private bool isCharging;

        public Behaviour_Trigger_ChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            energyImage = Cond.Instance.Get<Image>(Cond.Instance.Get<Comp>(entity, Label.ENERGY), Label.ENERGY);
            Data.Instance.OnUpdateEvent.AddListener(Charge);
        }

        /*进入充能*/
        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = true;
                    energyImage.gameObject.SetActive(true);
                }
            }
        }

        /*充能刷新*/
        private void Charge() {
            if (isCharging) {
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy +=
                    Loader.LoadSetting().TowerSetting.ChargeEnergySpeed * Time.deltaTime;

                if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy >= Loader.LoadSetting().TowerSetting.MaxEnergy) {
                    bool getFlow = Flo.Instance.GetFlow(out beginFlow);
                    if (getFlow) {
                        beginFlow.Next("Battle");
                        return;
                    }
                }

                if (energyImage.gameObject.activeSelf) {
                    energyImage.fillAmount = entity.EntityData.BaseRuntimeData.TowerInfo.Energy /
                                             Loader.LoadSetting().TowerSetting.MaxEnergy;
                }
            } else {
                energyImage.gameObject.SetActive(false);
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy = 0;
            }
        }

        /*离开充能*/
        private void ChargeOut(Collider collider) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = false;
                    energyImage.gameObject.SetActive(false);
                    entity.EntityData.BaseRuntimeData.TowerInfo.Energy = 0;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Charge);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.RemoveListener(ChargeOut);
        }
    }
}