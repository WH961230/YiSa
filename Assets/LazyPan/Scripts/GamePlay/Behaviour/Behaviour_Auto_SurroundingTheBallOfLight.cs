using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SurroundingTheBallOfLight : Behaviour {
	    private int surroundNum;
	    private GameObject surround;
	    private Comp Ball1;
	    private Comp Ball2;
	    private Comp Ball3;
        public Behaviour_Auto_SurroundingTheBallOfLight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Debug.Log("环绕球体注册");
	        surroundNum = 1;
	        surround = Loader.LoadGo("弹药", "Common/Obj_Fx_SurroundBullet", Data.Instance.ObjRoot, true);
	        Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
	        surround.transform.position = bulletMuzzle.position;
	        Ball1 = Cond.Instance
		        .Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.TRIGGER, "1"));
	        Ball2 = Cond.Instance
		        .Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.TRIGGER, "2"));
	        Ball3 = Cond.Instance
		        .Get<Comp>(surround.GetComponent<Comp>(), Label.Assemble(Label.TRIGGER, "3"));
	        Ball1.OnTriggerEnterEvent.RemoveAllListeners();
	        Ball1.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        Ball2.OnTriggerEnterEvent.RemoveAllListeners();
	        Ball2.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        Ball3.OnTriggerEnterEvent.RemoveAllListeners();
	        Ball3.OnTriggerEnterEvent.AddListener(SurroundTrigger);
	        ShowSurroundBallOfLight();
	        
	        Data.Instance.OnUpdateEvent.AddListener(Surround);
        }

        /*创建环绕球*/
        private void ShowSurroundBallOfLight() {
	        HideAllSurroundBall();
	        int num = surroundNum;
	        if (num == 1) {
		        if (!Ball1.gameObject.activeSelf) {
			        Ball1.gameObject.SetActive(true);
		        }
	        }
	        
	        if (num == 2) {
		        if (!Ball1.gameObject.activeSelf) {
			        Ball1.gameObject.SetActive(true);
		        }
		        if (!Ball2.gameObject.activeSelf) {
			        Ball2.gameObject.SetActive(true);
		        }
	        }
	        
	        if (num == 3) {
		        if (!Ball1.gameObject.activeSelf) {
			        Ball1.gameObject.SetActive(true);
		        }
		        if (!Ball2.gameObject.activeSelf) {
			        Ball2.gameObject.SetActive(true);
		        }
		        if (!Ball3.gameObject.activeSelf) {
			        Ball3.gameObject.SetActive(true);
		        }
	        }
        }

        /*隐藏所有的球*/
        private void HideAllSurroundBall() {
	        if (Ball1.gameObject.activeSelf) {
		        Ball1.gameObject.SetActive(false);
	        }
	        if (Ball2.gameObject.activeSelf) {
		        Ball2.gameObject.SetActive(false);
	        }
	        if (Ball3.gameObject.activeSelf) {
		        Ball3.gameObject.SetActive(false);
	        }
        }

		/*环绕*/
		private void Surround() {
			if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
				ShowSurroundBallOfLight();
				Cond.Instance.Get<Transform>(surround.GetComponent<Comp>(), Label.BODY).transform
					.Rotate(Vector3.up * Time.deltaTime * 200);
			} else {
				HideAllSurroundBall();
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
					MessageRegister.Instance.Dis(MessageCode.BeInjuried, tmpEntity, Loader.LoadSetting().TowerSetting.Attack);
					/*掉血表现*/
					GameObject template = Loader.LoadGo("掉血", "Common/Obj_Fx_BeHit", Data.Instance.ObjRoot, true);
					Transform squirt = Cond.Instance.Get<Transform>(tmpEntity, Label.SQUIRT);
					template.transform.position = squirt.position;
					template.transform.rotation = squirt.rotation;
					/*击退表现*/
					MessageRegister.Instance.Dis(MessageCode.BeHit, entity, tmpEntity);
					/*受击材质高亮*/
					Material mat = Cond.Instance.Get<Renderer>(tmpEntity, Label.Assemble(Label.BODY, Label.RENDERER)).material;
					mat.SetColor("_EmissionColor", Color.white);
					mat.EnableKeyword("_EMISSION");
					/*复原*/
					ClockUtil.Instance.AlarmAfter(0.1f, () => {
						Material mat = Cond.Instance
							.Get<Renderer>(tmpEntity, Label.Assemble(Label.BODY, Label.RENDERER)).material;
						mat.SetColor("_EmissionColor", Color.black);
						mat.EnableKeyword("_EMISSION");
					});
				}
			}
		}

		public override void Upgrade() {
			base.Upgrade();
			Debug.Log("环绕球体升级");
			surroundNum++;
			if (surroundNum > 3) {
				surroundNum = 3;
			}

			ShowSurroundBallOfLight();
		}

		public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Surround);
        }
    }
}