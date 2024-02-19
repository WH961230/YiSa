using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_DeathRagdoll : Behaviour {
        public Rigidbody[] ragdollRigidbodies;
        public Collider[] ragdollColliders;
        public Behaviour_DeathRagdoll(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            ragdollRigidbodies = entity.Prefab.GetComponentsInChildren<Rigidbody>();
            ragdollColliders = entity.Prefab.GetComponentsInChildren<Collider>();
            DisableRagdoll();
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathEntity, DeathOpenRagdoll);
        }

        private void DeathOpenRagdoll(Entity beInjuriedEntity) {
            if (entity.ID == beInjuriedEntity.ID) {
                if (!entity.EntityData.IsDeathRagdoll) {
                    EnableRagdoll();
                    entity.EntityData.IsDeathRagdoll = true;
                }
            }
        }

        public void EnableRagdoll() {
            entity.Comp.Get<Slider>("HealthBar").gameObject.SetActive(false);
            entity.Comp.Get<Animator>("Animator").enabled = false;
            Object.Destroy(entity.Comp.Get<Collider>("AttackCollider"));
            Object.Destroy(entity.Comp.Get<Collider>("BodyCollider"));
            Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out Entity PlayerEntity);
            for (int i = 0; i < ragdollColliders.Length; i++) {
                ragdollColliders[i].enabled = true;
            }

            for (int i = 0; i < ragdollRigidbodies.Length; i++) {
                Rigidbody rb = ragdollRigidbodies[i];
                rb.isKinematic = false;
                Vector3 dir = (rb.transform.position - PlayerEntity.Prefab.transform.position).normalized;
                rb.AddForceAtPosition(dir * entity.EntitySetting.RobotDeadRagdollForce, rb.transform.position, ForceMode.Impulse);
            }
        }

        private void DisableRagdoll() {
            for (int i = 0; i < ragdollRigidbodies.Length; i++) {
                ragdollRigidbodies[i].isKinematic = true;
            }

            for (int i = 0; i < ragdollColliders.Length; i++) {
                if (!ragdollColliders[i].CompareTag("Monster")) {
                    ragdollColliders[i].enabled = false;
                }
            }
        }

        public override void OnClear() {
            base.OnClear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathEntity, DeathOpenRagdoll);
        }
    }
}