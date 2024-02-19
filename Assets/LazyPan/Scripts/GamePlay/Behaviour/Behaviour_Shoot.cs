using MilkShake;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Shoot : Behaviour {
        private bool isLongShoot;
        private bool isReload;
        private float shootDeployTime;
        private float reloadDeployTime;
        public Behaviour_Shoot(Entity entity, string sign) : base(entity, sign) {
            //输入注册
            InputRegister.Instance.Load(InputRegister.Instance.LeftClick, InputShoot);
            InputRegister.Instance.Load(InputRegister.Instance.R, OnReload);

            Animator animator = entity.Comp.Get<Animator>("Animator");
            animator.SetInteger("WeaponType", entity.EntitySetting.WeaponType);

            Data.Instance.OnUpdateEvent.AddListener(OnShootUpdate);
            Data.Instance.OnUpdateEvent.AddListener(OnReloadUpdate);
            MessageRegister.Instance.Reg<Entity>(MessageCode.LevelUp, OnLevelUp);
        }

        private void OnLevelUp(Entity targetEntity) {
            if (targetEntity.ID == entity.ID) {
                isLongShoot = false;
            }
        }

        private void OnReload(InputAction.CallbackContext obj) {
            isReload = true;
            reloadDeployTime = entity.EntityData.ReloadInterval;
        }

        private void InputShoot(InputAction.CallbackContext obj) {
            if (Time.timeScale == 0) {
                return;
            }

            //先判断枪是否处于间隔状态
            if (shootDeployTime == 0) {
                //开始按下单发
                if (obj.started) {
                    Shoot();
                } else if (obj.performed) {//长按
                    isLongShoot = true;
                } else if (obj.canceled) {//取消抬起
                    isLongShoot = false;
                }
            } else {
                isLongShoot = false;
            }
        }

        private void OnShootUpdate() {
            if (isReload) {
                return;
            }

            if (shootDeployTime > 0) {
                shootDeployTime -= Time.deltaTime;
            } else {
                shootDeployTime = 0;//停止间隔状态
                if (isLongShoot) {
                    Shoot();
                }
            }
        }

        private void OnReloadUpdate() {
            if (isReload) {
                if (reloadDeployTime > 0) {
                    reloadDeployTime -= Time.deltaTime;
                } else {
                    Reload();
                    isReload = false;
                }
            }
        }

        private void Shoot() {
            //换弹时间
            if (isReload) {
                return;
            }

            //射击间隔时间
            if (shootDeployTime > 0) {
                return;
            }

            //弹药数量大于0
            if (entity.EntityData.BulletNum > 0) {
                Transform bulletMuzzleTran = entity.Comp.Get<Transform>("BulletMuzzleTran");
                GameObject bulletGo = Object.Instantiate(entity.EntityData.BulletPrefab, bulletMuzzleTran.position, entity.Prefab.transform.rotation);
                BulletTrigger bulletTrigger = bulletGo.GetComponentInChildren<BulletTrigger>();
                bulletTrigger.DamageVal = (int)(entity.EntityData.AttackBase * entity.EntityData.AttackRatio * entity.EntityData.AttackExtraRatio);
                Sound.Instance.SoundPlay("Gun_1", bulletMuzzleTran.position, false, 0.5f);

                if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity cameraEntity)) {
                    if (Data.Instance.TryGetShakerSetting(cameraEntity.EntitySetting, "Shoot", out ShakerSetting shakerSetting)) {
                        Shaker.ShakeAllSeparate(shakerSetting.CameraShakePreset);
                    }
                }

                entity.EntityData.BulletNum -= 1;
                entity.EntityData.BulletNum = Mathf.Max(0, entity.EntityData.BulletNum);
                shootDeployTime = entity.EntityData.AttackInterval;
            } else {
                isReload = true;
                reloadDeployTime = entity.EntityData.ReloadInterval;
            }

            MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, entity);
        }

        private void Reload() {
            entity.EntityData.BulletNum = entity.EntityData.BulletMaxNum;
            MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, entity);
        }

        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnShootUpdate);
            Data.Instance.OnUpdateEvent.RemoveListener(OnReloadUpdate);
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, InputShoot);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.LevelUp, OnLevelUp);
        }
    }
}