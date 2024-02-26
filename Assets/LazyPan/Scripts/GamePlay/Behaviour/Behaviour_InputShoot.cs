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
                if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity cameraEntity)) {
                    cameraEntity.Comp.GetEvent("PlayerShootEvent")?.Invoke();
                }
            }
        }

        private void Shoot() {
            GameObject template = entity.Comp.Get<GameObject>("BulletPrefab");
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