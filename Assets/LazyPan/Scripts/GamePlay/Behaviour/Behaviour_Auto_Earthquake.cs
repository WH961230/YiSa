using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Earthquake : Behaviour {
	    private float deploy;
	    private int EarthquakeNum;

        public Behaviour_Auto_Earthquake(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Data.Instance.OnUpdateEvent.AddListener(Earthquake);
        }

		/*地震*/
		private void Earthquake() {
			if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
				if (deploy > 0) {
					deploy -= Time.deltaTime;
				} else {
					CreateEarthquake();
					deploy = Loader.LoadSetting().TowerSetting.AttackIntervalTime;
				}
			}
		}

		/*创建地震*/
		private void CreateEarthquake() {
			GameObject template = Loader.LoadGo("弹药", "Common/Obj_Fx_EarthquakeBullet", Data.Instance.ObjRoot, true);
			Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.FOOT);
			template.transform.position = bulletMuzzle.position;
			Cond.Instance.Get<Comp>(template.GetComponent<Comp>(), Label.TRIGGER).OnTriggerEnterEvent.AddListener(EarthquakeTrigger);
			ClockUtil.Instance.AlarmAfter(1f, () => {
				if (template != null) {
					Object.Destroy(template);
					template = null;
				}
			});
		}

		/*地震碰撞*/
		private void EarthquakeTrigger(Collider collider) {
			if (collider.gameObject.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
				return;
			}
			if (Data.Instance.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity tmpEntity)) {
				if (tmpEntity.EntityData.BaseRuntimeData.Type != "Robot") {
					return;
				}
				tmpEntity.EntityData.BaseRuntimeData.RobotInfo.SlowTime = 2;
			}
		}

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Earthquake);
        }
    }
}