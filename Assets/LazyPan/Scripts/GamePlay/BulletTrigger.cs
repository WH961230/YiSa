using UnityEngine;

namespace LazyPan {
    public class BulletTrigger : MonoBehaviour {
        public GameObject bulletGo;
        public int DamageVal;

        private void Start() {
            Destroy(bulletGo, 1);
        }

        private void OnParticleCollision(GameObject other) {
            bool isBeHit = other.layer == LayerMask.NameToLayer("Monster");
            if (isBeHit) {
                Data.Instance.TryGetEntityByComp(other.GetComponent<Comp>(), out Entity entity);
                if (entity != null) {
                    MessageRegister.Instance.Dis(MessageCode.DamageEntity, entity, DamageVal);
                }
            }
        }
    }
}