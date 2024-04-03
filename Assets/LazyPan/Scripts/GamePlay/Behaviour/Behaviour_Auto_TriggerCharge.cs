using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TriggerCharge : Behaviour {
        private Flow_Battle battleFlow;
        private Image energyImage;
        private Comp rangeComp;
        private bool isCharging;

        public Behaviour_Auto_TriggerCharge(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity,  Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.AddListener(ChargeOut);
            rangeComp = Cond.Instance.Get<Comp>(entity, Label.RANGE);
            Data.Instance.OnUpdateEvent.AddListener(Charge);
            energyImage = Cond.Instance.Get<Image>(Cond.Instance.Get<Comp>(entity, Label.ENERGY), Label.ENERGY);
            isCharging = false;
        }

        /*进入充能*/
        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = true;
                }
            }
        }

        /*充能*/
        private void Charge() {
            if (isCharging) {
                //能量充能
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy += Loader.LoadSetting().TowerSetting.ChargeEnergySpeed * Time.deltaTime;
                //能量充能上限
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy = Mathf.Min(entity.EntityData.BaseRuntimeData.TowerInfo.Energy, Loader.LoadSetting().TowerSetting.MaxEnergy);
            } else {
                //能量掉落
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy -= Loader.LoadSetting().TowerSetting.DownEnergySpeed * Time.deltaTime;
                //能量下限
                entity.EntityData.BaseRuntimeData.TowerInfo.Energy = Mathf.Max(entity.EntityData.BaseRuntimeData.TowerInfo.Energy, 0);
            }

            //充能条展示
            if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
                energyImage.gameObject.SetActive(true);
                rangeComp.gameObject.SetActive(true);
                energyImage.fillAmount = entity.EntityData.BaseRuntimeData.TowerInfo.Energy / Loader.LoadSetting().TowerSetting.MaxEnergy;
            } else {
                energyImage.gameObject.SetActive(false);
                rangeComp.gameObject.SetActive(false);
            }

            //范围旋转
            if (rangeComp.gameObject.activeSelf) {
                rangeComp.gameObject.transform.rotation *=
                    Quaternion.AngleAxis(Loader.LoadSetting().TowerSetting.RangeRotateAngle * Time.deltaTime, Vector3.forward);
            }
        }

        /*离开充能*/
        private void ChargeOut(Collider collider) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = false;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Charge);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerEnterEvent.RemoveListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.ENERGY, Label.TRIGGER)).OnTriggerExitEvent.RemoveListener(ChargeOut);
        }
    }
}