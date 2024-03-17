using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Even_TriggerChargeStartGame : Behaviour {
        private Flow_Begin beginFlow;
        private float deploy;
        private Image energyImage;

        public Behaviour_Even_TriggerChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            energyImage = Cond.Instance.Get<Image>(Cond.Instance.Get<Comp>(entity, Label.ENERGY), Label.ENERGY);
            Data.Instance.OnUpdateEvent.AddListener(Charge);
        }

        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsCharging = true;
                    deploy = 1;
                    energyImage.gameObject.SetActive(true);
                }
            }
        }

        private void Charge() {
            if (entity.EntityData.BaseRuntimeData.CurIsCharging) {
                entity.EntityData.BaseRuntimeData.CurEnergy +=
                    entity.EntityData.BaseRuntimeData.CurChargeEnergySpeed * Time.deltaTime;

                if (entity.EntityData.BaseRuntimeData.CurEnergy >= entity.EntityData.BaseRuntimeData.CurMaxEnergy) {
                    bool getFlow = Flo.Instance.GetFlow(out beginFlow);
                    if (getFlow) {
                        beginFlow.Next();
                        return;
                    }
                }

                if (energyImage.gameObject.activeSelf) {
                    energyImage.fillAmount = entity.EntityData.BaseRuntimeData.CurEnergy /
                        entity.EntityData.BaseRuntimeData.CurMaxEnergy;
                }
            } else {
                energyImage.gameObject.SetActive(false);
                entity.EntityData.BaseRuntimeData.CurEnergy = 0;
            }
        }

        private void ChargeOut(Collider collider) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsCharging = false;
                    energyImage.gameObject.SetActive(false);
                    entity.EntityData.BaseRuntimeData.CurEnergy = 0;
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