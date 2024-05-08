using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Orbitingball : Behaviour {
	    /*环绕等级*/
	    private int surroundNum = 1;
	    /*环绕速度*/
	    private float surroundSpeed;
	    /*攻击力*/
	    private float attackDamage;
	    /*父物体*/
	    private GameObject surround;
	    /*不同等级*/
	    private Comp Level1;
	    private Comp Level2;
	    private Comp Level3;
	    /*配置*/
	    private BuffSettingInfo buffSettingInfo;
        public Behaviour_Auto_Orbitingball(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        /*获取配置*/
	        Loader.LoadSetting().BuffSetting.GetSettingBySign(behaviourSign, out buffSettingInfo);
	        /*初始化弹药*/
	        InitBullet();
	        /*注册环绕刷新*/
	        Data.Instance.OnUpdateEvent.AddListener(Surround);
	        /*获取环绕速度*/
	        buffSettingInfo.GetParam("SurroundSpeed", out string speed);
	        surroundSpeed = float.Parse(speed);
	        /*攻击伤害*/
	        buffSettingInfo.GetParam("AttackDamage", out string attackdamage);
	        attackDamage = float.Parse(attackdamage);
        }

        /*初始化弹药*/
        private void InitBullet() {
	        buffSettingInfo.GetParam("Bullet", out string bullet);
	        surround = Loader.LoadGo("弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
	        surround.transform.position = Cond.Instance.Get<Transform>(entity, Label.MUZZLE).position;
	        Level1 = Cond.Instance.Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "1"));
	        Level2 = Cond.Instance.Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "2"));
	        Level3 = Cond.Instance.Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "3"));
	        RegisterTriggerBallEvent();
        }

        /*注册触发球的事件*/
        private void RegisterTriggerBallEvent() {
	        foreach (Comp.CompData compData in Level1.Comps) {
		        compData.Comp.OnTriggerEnterEvent.RemoveAllListeners();
		        compData.Comp.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        }
	        foreach (Comp.CompData compData in Level2.Comps) {
		        compData.Comp.OnTriggerEnterEvent.RemoveAllListeners();
		        compData.Comp.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        }
	        foreach (Comp.CompData compData in Level3.Comps) {
		        compData.Comp.OnTriggerEnterEvent.RemoveAllListeners();
		        compData.Comp.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        }
        }

        /*隐藏所有的球*/
        private void HideAllSurround() {
	        Level1.gameObject.SetActive(false);
	        Level2.gameObject.SetActive(false);
	        Level3.gameObject.SetActive(false);
        }

		/*环绕*/
		private void Surround() {
			if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
				Level1.gameObject.SetActive(surroundNum == 1);
				Level2.gameObject.SetActive(surroundNum == 2);
				Level3.gameObject.SetActive(surroundNum == 3);
				Cond.Instance.Get<Transform>(surround.GetComponent<Comp>(), Label.BODY).transform
					.Rotate(Vector3.up * Time.deltaTime * surroundSpeed);
			} else {
				HideAllSurround();
			}
		}

		/*环绕碰撞*/
		private void SurroundTrigger(Collider collider) {
			if (collider.gameObject.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
				return;
			}
			if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity tmpEntity)) {
				if (tmpEntity.EntityData.BaseRuntimeData.Type != "Robot") {
					return;
				}
				if (tmpEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
					tmpEntity.EntityData.BaseRuntimeData.RobotInfo.BeAttackType = 1;
					MessageRegister.Instance.Dis(MessageCode.BeInjuried, tmpEntity, attackDamage);
					Sound.Instance.SoundPlay("Orbitingball", Vector3.zero, false, 1);
				}
			}
		}

		public override void Upgrade() {
			base.Upgrade();
			surroundNum++;
			if (surroundNum > 3) {
				surroundNum = 3;
			}
		}

		public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Surround);
        }
    }
}