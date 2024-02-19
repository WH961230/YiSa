using UnityEngine;

namespace LazyPan {
    public class Behaviour_GainExperience : Behaviour {
        public Behaviour_GainExperience(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            entity.Comp.OnTriggerEnterEvent.AddListener(OnGainExperienceTriggerEnter);
            MessageRegister.Instance.Reg<int>(MessageCode.GainExperience, OnGainExperience);
        }

        private void OnGainExperienceTriggerEnter(Collider collider) {
            if (collider.tag != "Experience") {
                return;
            }

            Comp experienceComp = collider.GetComponent<Comp>();
            experienceComp.gameObject.SetActive(false);
            MessageRegister.Instance.Dis(MessageCode.GainExperience, int.Parse(experienceComp.ObjInfo));
        }

        private void OnGainExperience(int experienceValue) {
            Sound.Instance.SoundPlay("PickUpDropExperience", entity.Prefab.transform.position, false, 1f);
            entity.EntityData.Experience += experienceValue;
            if (Data.Instance.TryGetLevelSetting(entity.EntitySetting, entity.EntityData.Level,
                out LevelSetting levelSetting)) {
                int off = entity.EntityData.Experience - levelSetting.ExperienceMax;
                if (off == 0) {
                    entity.EntityData.Level++;
                    entity.EntityData.Experience = 0;
                    MessageRegister.Instance.Dis(MessageCode.LevelUp, entity);
                } else if (off > 0) {
                    entity.EntityData.Experience = off;
                    entity.EntityData.Level++;
                    MessageRegister.Instance.Dis(MessageCode.LevelUp, entity);
                }

                MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, entity);
            }
        }

        public override void OnClear() {
            base.OnClear();
            entity.Comp.OnTriggerEnterEvent.RemoveListener(OnGainExperienceTriggerEnter);
            MessageRegister.Instance.UnReg<int>(MessageCode.GainExperience, OnGainExperience);
        }
    }
}