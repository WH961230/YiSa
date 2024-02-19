using MilkShake;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LazyPan {
    public class Behaviour_BeInjuried : Behaviour {
        private Entity playerEntity;
        public Behaviour_BeInjuried(Entity entity, string sign) : base(entity, sign) {
            Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out playerEntity);
            MessageRegister.Instance.Reg<Entity, int>(MessageCode.DamageEntity, Damage);
        }

        private void Damage(Entity beInjuriedEntity, int damage) {
            if (entity.ID == beInjuriedEntity.ID) {
                if (entity.EntityData.Health == 0) {
                    return;
                }

                entity.EntityData.Health -= damage;
                entity.EntityData.Health = Mathf.Max(0, entity.EntityData.Health);

                if (entity.EntityData.ObjType == ObjType.MainPlayer) {
                    if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity cameraEntity)) {
                        if (Data.Instance.TryGetShakerSetting(cameraEntity.EntitySetting, "BeInjuried", out ShakerSetting shakerSetting)) {
                            Shaker.ShakeAllSeparate(shakerSetting.CameraShakePreset);
                        }
                    }
                }

                if (entity.Prefab != null) {
                    GameObject ob = Object.Instantiate(entity.EntitySetting.BloodFloatingGo,
                        entity.Prefab.transform.position + Vector3.up * 1.8f, Camera.main.transform.rotation);
                    FloatingText floatText = ob.GetComponentInChildren<FloatingText>();
                    if (floatText != null) {
                        Object.Destroy(ob, floatText.LifeTime);
                        floatText.SetText(damage.ToString());
                    }
                }

                if (entity.EntityData != null && entity.EntityData.Health <= 0) {
                    MessageRegister.Instance.Dis(MessageCode.DeathEntity, entity);
                    return;
                }

                MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, entity);
                MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, playerEntity);
            }
        }

        public override void OnClear() {
            base.OnClear();
            MessageRegister.Instance.UnReg<Entity, int>(MessageCode.DamageEntity, Damage);
        }
    }
}