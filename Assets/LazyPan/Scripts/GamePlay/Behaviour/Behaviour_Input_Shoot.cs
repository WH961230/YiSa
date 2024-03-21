using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Shoot : Behaviour {
        public Behaviour_Input_Shoot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InputRegister.Instance.Load(InputRegister.Instance.LeftClick, MouseLeft);
        }

        private void MouseLeft(InputAction.CallbackContext obj) {
            if (!Data.Instance.CanControl) {
                return;
            }
            if (obj.performed) {
                Shoot();
                Cond.Instance.Get(Cond.Instance.GetCameraEntity(), Label.Assemble(Label.SHOOT, Label.EVENT))?.Invoke();
            }
        }

        private void Shoot() {
            GameObject template = Loader.LoadGo("弹药", "Common/Obj_Fx_Bullet", Data.Instance.ObjRoot, true);
            Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
            template.transform.position = bulletMuzzle.position;
            template.transform.rotation = bulletMuzzle.rotation;
            template.GetComponent<Rigidbody>().AddForce(template.transform.forward * 100f, ForceMode.Impulse);
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, MouseLeft);
        }
    }
}