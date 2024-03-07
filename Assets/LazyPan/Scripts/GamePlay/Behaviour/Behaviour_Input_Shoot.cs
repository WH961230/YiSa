﻿using UnityEngine;
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
            GameObject template = Loader.LoadGo("弹药", "Obj/Fight/Obj_Bullet", Data.Instance.ObjRoot, true);
            Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
            GameObject bullet = Object.Instantiate(template);
            bullet.transform.position = bulletMuzzle.position;
            bullet.transform.rotation = bulletMuzzle.rotation;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 100f, ForceMode.Impulse);
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, MouseLeft);
        }
    }
}