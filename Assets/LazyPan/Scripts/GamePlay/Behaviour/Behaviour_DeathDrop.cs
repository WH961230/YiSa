using UnityEngine;

namespace LazyPan {
    public class Behaviour_DeathDrop : Behaviour {
        public Behaviour_DeathDrop(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathEntity, DeathDrop);
        }

        private void DeathDrop(Entity tempEntity) {
            if (tempEntity.ID == entity.ID) {
                if (Data.Instance.TryGetDropSetting(entity.EntitySetting, "Exp", out DropSetting dropSetting)) {
                    GameObject dropPrefab = Object.Instantiate(dropSetting.DropPrefab, Obj.Instance.ObjRoot);
                    Data.Instance.ExperiencePrefabs.Add(dropPrefab);
                    dropPrefab.transform.position = entity.Prefab.transform.position + Vector3.up * 2;
                    Rigidbody rb = dropPrefab.GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.forward * dropSetting.DropForce, ForceMode.Force);
                    MessageRegister.Instance.Dis(MessageCode.ClearEntity, entity);
                }
            }
        }

        public override void OnClear() {
            base.OnClear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathEntity, DeathDrop);
        }
    }
}