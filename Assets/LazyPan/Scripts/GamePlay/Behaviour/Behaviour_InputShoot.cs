using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_InputShoot : Behaviour {
        public Behaviour_InputShoot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.LeftClick, MouseLeft);
        }

        private void MouseLeft(InputAction.CallbackContext obj) {
            if (!Data.Instance.CanControl) {
                return;
            }
            if (obj.performed) {
                Shoot();
                entity.Comp.GetEvent("PlayerShootEvent")?.Invoke();
                if (Data.Instance.TryGetEntityByType(entity.EntityData.BaseRuntimeData.CameraType, out Entity cameraEntity)) {
                    cameraEntity.Comp.GetEvent("PlayerShootEvent")?.Invoke();
                }
            }
        }

        private void Shoot() {
            GameObject template = Loader.LoadGo("弹药", "Obj/Resource/Obj_Bullet", Data.Instance.ObjRoot, true);
            Transform bulletMuzzle = entity.Comp.Get<Transform>("Muzzle");
            GameObject bullet = Object.Instantiate(template);
            bullet.transform.position = bulletMuzzle.position;
            bullet.transform.rotation = bulletMuzzle.rotation;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 100f, ForceMode.Impulse);
        }

        public override void OnClear() {
            base.OnClear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, MouseLeft);
        }
    }
}