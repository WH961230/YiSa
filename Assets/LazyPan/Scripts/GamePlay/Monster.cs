using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Monster : MonoBehaviour {
        private bool isDead;
        public float DeadForce;
        public GameObject BloodFloatingGo;
        public GameObject AddHealthFloatingGo;
        public Rigidbody[] ragdollRigidbodies;
        public Collider[] ragdollColliders;
        public Collider damageTriggerCollider;
        public float attackDistance;
        private Slider monsterHealthSlider;
        private Animator animator;
        private Player player;
        private bool attackTrigger;
        private float attackDeploy;

        void Start() {
            animator = GetComponent<Animator>();
            player = FindObjectOfType<Player>();
            monsterHealthSlider = GetComponentInChildren<Slider>();
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            ragdollColliders = GetComponentsInChildren<Collider>();
            DisableRagdoll();
            CloseDamageCollider();
        }

        private void Update() {
            if (isDead) {
                return;
            }

            float sqrDistance = (transform.position - player.transform.position).sqrMagnitude;
            if (sqrDistance > attackDistance * attackDistance) {
                animator.SetFloat("Motion", 2);
                Vector3 dir = (player.transform.position - transform.position).normalized;
                Vector3 forward = new Vector3(dir.x, 0, dir.z);
                transform.forward = forward;
                animator.SetInteger("Attack", 0);
            } else {
                animator.SetFloat("Motion", 0);
                animator.SetInteger("Attack", 1);
            }
        }

        public void Damage(int damage) {
            // if (isDead) {
            //     return;
            // }
            //
            // Data.Instance.monsterData.MonsterHealth -= damage;
            // Data.Instance.monsterData.MonsterHealth = Mathf.Max(0, Data.Instance.monsterData.MonsterHealth);
            // Data.Instance.monsterData.MonsterHealthSlider.value = (float) Data.Instance.monsterData.MonsterHealth / Data.Instance.monsterData.MonsterHealthMax;
            // if (Data.Instance.monsterData.MonsterHealth == 0) {
            //     isDead = true;
            //     EnableRagdoll();
            //     CloseDamageCollider();
            //     GetComponent<Collider>().enabled = false;
            //     player.FightData.KillMonsterCount++;
            //     player.FightData.KillMonsterCountText.text =
            //         string.Concat("击杀怪物数量: ", player.FightData.KillMonsterCount);
            // }
            //
            // GameObject ob = Instantiate(BloodFloatingGo, transform.position + Vector3.up * 1.8f, Camera.main.transform.rotation);
            // FloatingText floatText = ob.GetComponentInChildren<FloatingText>();
            // if (floatText != null) {
            //     Destroy(ob, floatText.LifeTime);
            //     floatText.SetText(damage.ToString());
            // }
        }

        public void OpenDamageCollider() {
            damageTriggerCollider.enabled = true;
        }

        public void CloseDamageCollider() {
            damageTriggerCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Player>() != null) {
                other.GetComponent<Player>().BeDamage(10);
                attackTrigger = false;
            }
        }

        public void EnableRagdoll() {
            monsterHealthSlider.gameObject.SetActive(false);
            GetComponent<Animator>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GameObject Player = GameObject.FindWithTag("Player");
            for (int i = 0; i < ragdollColliders.Length; i++) {
                ragdollColliders[i].enabled = true;
            }

            for (int i = 0; i < ragdollRigidbodies.Length; i++) {
                Rigidbody rb = ragdollRigidbodies[i];
                rb.isKinematic = false;
                Vector3 dir = (rb.transform.position - Player.transform.position).normalized;
                rb.AddForceAtPosition(dir * DeadForce, rb.transform.position, ForceMode.Impulse);
            }
        }

        void DisableRagdoll() {
            for (int i = 0; i < ragdollRigidbodies.Length; i++) {
                ragdollRigidbodies[i].isKinematic = true;
            }

            for (int i = 0; i < ragdollColliders.Length; i++) {
                if (!ragdollColliders[i].CompareTag("Monster")) {
                    ragdollColliders[i].enabled = false;
                }
            }
        }
    }
}