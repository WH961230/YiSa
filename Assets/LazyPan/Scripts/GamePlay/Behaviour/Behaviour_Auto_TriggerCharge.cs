using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TriggerCharge : Behaviour {
        private Flow_Battle battleFlow;
        private Image energyImage;
        private Comp rangeComp;
        public Behaviour_Auto_TriggerCharge(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            rangeComp = Cond.Instance.Get<Comp>(entity, Label.RANGE);
            Data.Instance.OnUpdateEvent.AddListener(Charge);
            energyImage = Cond.Instance.Get<Image>(Cond.Instance.Get<Comp>(entity, Label.ENERGY), Label.ENERGY);
        }

        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsCharging = true;
                }
            }
        }

        private void Charge() {
            if (entity.EntityData.BaseRuntimeData.CurIsCharging) {
                //能量充能
                entity.EntityData.BaseRuntimeData.CurEnergy +=
                    entity.EntityData.BaseRuntimeData.CurChargeEnergySpeed * Time.deltaTime;
                //能量充能上限
                entity.EntityData.BaseRuntimeData.CurEnergy = Mathf.Min(entity.EntityData.BaseRuntimeData.CurEnergy,
                    entity.EntityData.BaseRuntimeData.CurMaxEnergy);
            } else {
                //能量掉落
                entity.EntityData.BaseRuntimeData.CurEnergy -=
                    entity.EntityData.BaseRuntimeData.DefEnergyDownSpeed * Time.deltaTime;
                //能量下限
                entity.EntityData.BaseRuntimeData.CurEnergy = Mathf.Max(entity.EntityData.BaseRuntimeData.CurEnergy,
                    0);
            }

            //充能条展示
            if (entity.EntityData.BaseRuntimeData.CurEnergy > 0) {
                energyImage.gameObject.SetActive(true);
                rangeComp.gameObject.SetActive(true);
                energyImage.fillAmount = entity.EntityData.BaseRuntimeData.CurEnergy /
                                         entity.EntityData.BaseRuntimeData.CurMaxEnergy;
            } else {
                energyImage.gameObject.SetActive(false);
                rangeComp.gameObject.SetActive(false);
            }

            //范围旋转
            if (rangeComp.gameObject.activeSelf) {
                rangeComp.gameObject.transform.rotation *=
                    Quaternion.AngleAxis(entity.EntityData.BaseRuntimeData.DefRangeRotateAngle * Time.deltaTime, Vector3.forward);
            }
        }

        private void ChargeOut(Collider collider) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    entity.EntityData.BaseRuntimeData.CurIsCharging = false;
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