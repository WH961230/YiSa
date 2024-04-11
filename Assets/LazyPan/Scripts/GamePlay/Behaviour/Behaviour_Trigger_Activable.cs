using System;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Trigger_Activable : Behaviour {
        private bool isCharging;
        private Image energyImage;
        private float energy;
        private bool isRealize;
        public Behaviour_Trigger_Activable(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargingIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargingOut);
            Data.Instance.OnUpdateEvent.AddListener(Charge);
            energyImage = Cond.Instance.Get<Image>(Cond.Instance.Get<Comp>(entity, Label.ENERGY), Label.ENERGY);
            isCharging = false;
            isRealize = false;
            energy = 0;
        }

        /*充能*/
        private void Charge() {
            if (isRealize) {
                return;
            }

            if (isCharging) {
                //能量充能
                energy += Loader.LoadSetting().ActivableSetting.ChargeSpeed * Time.deltaTime;
                //能量充能上限
                energy = Mathf.Min(energy, Loader.LoadSetting().ActivableSetting.MaxCharge);
            } else {
                //能量掉落
                energy -= Loader.LoadSetting().ActivableSetting.DownChargeSpeed * Time.deltaTime;
                //能量下限
                energy = Mathf.Max(energy, 0);
            }

            //充能条展示
            if (energy > 0) {
                energyImage.gameObject.SetActive(true);
                energyImage.fillAmount = energy / Loader.LoadSetting().ActivableSetting.MaxCharge;
                if (Math.Abs(energyImage.fillAmount - 1) < 0.001f) {
                    RealizeActive();
                    MessageRegister.Instance.Dis(MessageCode.RecycleActivable, entity);
                }
            } else {
                energyImage.gameObject.SetActive(false);
            }
        }

        /*实现激活事件*/
        private void RealizeActive() {
            MessageRegister.Instance.Dis(MessageCode.PlayerUpgrade);
            isRealize = true;
        }

		/*激活进入*/
		private void ChargingIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = true;
                }
            }
		}

        /*激活退出*/
        private void ChargingOut(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    isCharging = false;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Charge);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(ChargingIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.RemoveListener(ChargingOut);
        }
    }
}