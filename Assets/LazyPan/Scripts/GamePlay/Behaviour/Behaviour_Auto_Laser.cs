using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Laser : Behaviour {
	    private float deploy;
	    private float attackIntervalTime;
	    private int attackDamage;
	    private int LaserNum;
	    private BuffSettingInfo buffSettingInfo;

        public Behaviour_Auto_Laser(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Data.Instance.OnUpdateEvent.AddListener(Laser);
	        LaserNum = 1;
	        Loader.LoadSetting().BuffSetting
		        .GetSettingBySign(behaviourSign, out buffSettingInfo);

	        buffSettingInfo.GetParam("AttackIntervalTime", out string speed);
	        attackIntervalTime = float.Parse(speed);

	        buffSettingInfo.GetParam("AttackDamage", out string damage);
	        attackDamage = int.Parse(damage);
        }

		/*发射激光*/
		private void Laser() {
			if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
				if (deploy > 0) {
					deploy -= Time.deltaTime;
				} else {
					CreateLaser();
					deploy = attackIntervalTime;
				}
			}
		}

		/*创建激光*/
		private void CreateLaser() {
			/*找到 laserNum 个不重复的敌人*/
			bool findRobotEntity = Data.Instance.TryGetEntitiesByType(Label.ROBOT, out List<Entity> robotEntities);
			if (findRobotEntity) {
				/*找 laserNum 个血量大于 0 的敌人目标*/
				int[] randNoRepeatIndex = MathUtil.Instance.GetRandNoRepeatIndex(robotEntities.Count, LaserNum);
				if (randNoRepeatIndex != null) {
					bool soundplay = false;
					for (int i = 0; i < randNoRepeatIndex.Length; i++) {
						Entity robot = robotEntities[randNoRepeatIndex[i]];
						if (robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
							buffSettingInfo.GetParam("Bullet", out string bullet);
							GameObject template = Loader.LoadGo("弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
							Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
							template.transform.position = bulletMuzzle.position;
							Vector3 dir = (Cond.Instance.Get<Transform>(robot, Label.HIT).position -
							               bulletMuzzle.position).normalized;
							dir.y = 0;
							template.transform.forward = dir;
							Cond.Instance.Get<LineRenderer>(template.GetComponent<Comp>(), Label.LINE).widthMultiplier *= 4;
							Cond.Instance.Get<Comp>(template.GetComponent<Comp>(), Label.TRIGGER).OnTriggerEnterEvent.AddListener(LaserTrigger);
							ClockUtil.Instance.AlarmAfter(0.2f, () => {
								if (template != null) {
									Object.Destroy(template);
									template = null;
								}
							});
							soundplay = true;
						}
					}

					if (soundplay) {
						// Sound.Instance.SoundPlay("Laser", Vector3.zero, false, 1, 1);
					}
				}
			}
		}

		/*激光碰撞*/
		private void LaserTrigger(Collider collider) {
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
				}
			}
		}

		public override void Upgrade() {
			base.Upgrade();
			LaserNum++;
			if (LaserNum > buffSettingInfo.UpgradeLimit) {
				LaserNum = buffSettingInfo.UpgradeLimit;
			}
		}

		public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Laser);
        }
    }
}