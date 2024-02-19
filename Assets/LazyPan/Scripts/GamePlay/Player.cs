using System;
using MilkShake;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LazyPan {
    public class Player : MonoBehaviour {
        [Header("是否持枪")] public bool isHoldWeapon;
        public float playerWalkSpeed;
        public float playerRunSpeed;
        public float switchInterval;
        public GameObject FootStepFx;
        public Transform BulletMuzzleTran;
        public GameObject BulletFx;
        public GameObject BloodFloatingGo;
        public GameObject AddHealthFloatingGo;
        public ShakePreset shakePreset;
        public ShakePreset beDamagedShakePreset;
        public FightData FightData;
        public int timeDownMaxTime;
        [HideInInspector] public Vector3 cursorPos;
        [HideInInspector] public bool isAim;
        private Animator playerAnimator;
        private Camera mainCamera;
        private float timeDeploy;
        private CharacterController characterController;
        private GameObject bulletGo;
        private float footStepDeploy;
        private float timeDownDeployTime; //倒计时
        private TextMeshProUGUI TimeDownText;

        void Start() {
            characterController = GetComponent<CharacterController>();
            playerAnimator = GetComponentInChildren<Animator>();
            playerAnimator.SetLayerWeight(1, 0);
            mainCamera = Camera.main;

            //击杀怪物数量
            FightData.KillMonsterCountText = UI.Instance.Get("UI_Main").Get<TextMeshProUGUI>("UI_Main_KillMonsterCount");
            FightData.KillMonsterCountText.text = string.Concat("击杀怪物数量: ", FightData.KillMonsterCount);
            playerAnimator.SetBool("HasWeapon", isHoldWeapon);

            //倒计时
            TimeDownText = UI.Instance.Get("UI_Main").Get<TextMeshProUGUI>("UI_Main_TimeDown");
            timeDownDeployTime = timeDownMaxTime;
        }

        void Update() {
            if (timeDownDeployTime <= 0) {
                timeDownDeployTime = 0;
                BossAppear();
            } else {
                timeDownDeployTime -= Time.deltaTime;
            }

            TimeDownText.text = FormatTwoTime((int) timeDownDeployTime);

            //根据是否按下 Shift 获取移动速度
            bool isShift = Input.GetKey(KeyCode.LeftShift);
            float speed = isShift ? playerRunSpeed : playerWalkSpeed;
            //根据相机的方向计算移动方向 获取运动向量
            Vector3 cameraVec = mainCamera.transform.forward;
            cameraVec.y = 0;
            Vector3 moveDirection = Input.GetAxis("Vertical") * cameraVec * speed;
            moveDirection += Input.GetAxis("Horizontal") * mainCamera.transform.right * speed;
            //尘土特效
            if (moveDirection != Vector3.zero) {
                if (footStepDeploy > 0) {
                    footStepDeploy -= Time.deltaTime;
                } else {
                    GameObject footStepFxGo = Instantiate(FootStepFx);
                    footStepFxGo.transform.position = transform.position + Vector3.up * 0.2f;
                    footStepDeploy = 0.2f;
                    Destroy(footStepFxGo, 2f);
                }

                timeDeploy = switchInterval;
                playerAnimator.SetBool("Walk", true);
                playerAnimator.SetFloat("MotionSpeed", isShift ? 2 : 1);
            } else {
                if (timeDeploy > 0) {
                    cursorPos = Vector3.zero;
                    timeDeploy -= Time.deltaTime;
                } else {
                    timeDeploy = 0;
                    playerAnimator.SetBool("Walk", false);
                }
            }

            //移动
            characterController.Move(moveDirection * Time.deltaTime);

            //朝向
            if (isAim) {
                //瞄准朝向光标的方向
                Vector3 lookForwardVec = (cursorPos - transform.position).normalized;
                lookForwardVec.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, lookForwardVec, Time.deltaTime * 10);
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward,
                    Color.yellow);
                if (Input.GetMouseButtonDown(0)) {
                    //弹药数量大于0
                    // if (Data.Instance.playerData.WeaponData.BulletNum > 0) {
                    //     Instantiate(BulletFx, BulletMuzzleTran.position, BulletMuzzleTran.rotation);
                    //     Sound.Instance.SoundPlay("Gun_1", transform.position);
                    //     Shaker.ShakeAllSeparate(shakePreset);
                    //     Shoot();
                    // } else {
                    //     Reload();
                    // }
                }
            } else {
                //没瞄准朝向移动的方向
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 10);
            }

            //瞄准动画
            playerAnimator.SetBool("Aim", isAim);

            //计算当前面向的方向 和 移动的方向的角度
            float engle = Vector3.Angle(transform.forward, moveDirection.normalized);
            float sign = 1;
            //判断移动方向和朝向的左右关系
            if (Vector3.Cross(transform.forward, moveDirection.normalized).y < 0) {
                sign = -1;
            }

            float tempHor = Mathf.Sin(sign * engle * Mathf.Deg2Rad);
            float tempVer = Mathf.Cos(sign * engle * Mathf.Deg2Rad);
            if (isShift) {
                tempHor *= 2;
                tempVer *= 2;
            }

            playerAnimator.SetFloat("Horizontal", Mathf.Lerp(playerAnimator.GetFloat("Horizontal"), tempHor, Time.deltaTime * 5));
            playerAnimator.SetFloat("Vertical", Mathf.Lerp(playerAnimator.GetFloat("Vertical"), tempVer, Time.deltaTime * 5));
            
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.P)) {
                EditorApplication.isPaused = true;
            }
#endif

            if (Input.GetKeyDown(KeyCode.H)) {
                BeDamage(10);
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                AddHealth(10);
            }

            Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + moveDirection.normalized,
                Color.red);
        }

        private void BossAppear() {
            Debug.Log("Boss 显示");
        }

        public string FormatTwoTime(int totalSeconds) {
            int minutes = totalSeconds / 60;
            string mm = minutes < 10f ? "0" + minutes : minutes.ToString();
            int seconds = (totalSeconds - (minutes * 60));
            string ss = seconds < 10 ? "0" + seconds : seconds.ToString();
            return string.Format("{0}:{1}", mm, ss);
        }

        public void AddHealth(int healthValue) {
            // Data.Instance.playerData.PlayerHealth += healthValue;
            // Data.Instance.playerData.PlayerHealth = Mathf.Min(Data.Instance.playerData.PlayerHealthMax, Data.Instance.playerData.PlayerHealth);
            // Data.Instance.playerData.PlayerHealthSlider.value = (float) Data.Instance.playerData.PlayerHealth / Data.Instance.playerData.PlayerHealthMax;
            // Data.Instance.playerData.PlayerHealthText.text = string.Concat(Data.Instance.playerData.PlayerHealth, "/", Data.Instance.playerData.PlayerHealthMax);
            // PlayerHealthVolume.SetTargetValue(Data.Instance.playerData.PlayerHealthSlider.value);
            // GameObject ob = Instantiate(AddHealthFloatingGo, transform.position + Vector3.up * 1.8f,
            //     Camera.main.transform.rotation);
            // FloatingText floatText = ob.GetComponentInChildren<FloatingText>();
            // if (floatText != null) {
            //     Destroy(ob, floatText.LifeTime);
            //     floatText.SetText("+ " + healthValue);
            // }
        }

        public void BeDamage(int damage) {
            // Data.Instance.playerData.PlayerHealth -= damage;
            // Data.Instance.playerData.PlayerHealth = Mathf.Max(0, Data.Instance.playerData.PlayerHealth);
            // if (Data.Instance.playerData.PlayerHealth == 0) {
            //     Dead();
            // } else {
            //     Shaker.ShakeAllSeparate(beDamagedShakePreset);
            // }
            //
            // Data.Instance.playerData.PlayerHealthSlider.value = (float) Data.Instance.playerData.PlayerHealth / Data.Instance.playerData.PlayerHealthMax;
            // Data.Instance.playerData.PlayerHealthText.text = string.Concat(Data.Instance.playerData.PlayerHealth, "/", Data.Instance.playerData.PlayerHealthMax);
            // PlayerHealthVolume.SetTargetValue(Data.Instance.playerData.PlayerHealthSlider.value);
            // GameObject ob = Instantiate(BloodFloatingGo, transform.position + Vector3.up * 1.8f,
            //     Camera.main.transform.rotation);
            // FloatingText floatText = ob.GetComponentInChildren<FloatingText>();
            // if (floatText != null) {
            //     Destroy(ob, floatText.LifeTime);
            //     floatText.SetText(damage.ToString());
            // }
        }

        public void Dead() {
            Debug.Log("角色死亡!");
        }

        public void Quit() {
            SceneManager.LoadScene("Launch");
        }
    }
}

[Serializable]
public class FightData {
    public int KillMonsterCount; //击杀怪物数量
    public TextMeshProUGUI KillMonsterCountText; //击杀怪物数量文本
}