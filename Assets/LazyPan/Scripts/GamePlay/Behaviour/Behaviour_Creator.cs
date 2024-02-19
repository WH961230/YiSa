using UnityEngine;

namespace LazyPan {
    public class Behaviour_Creator : Behaviour {
        //创建的实体
        private float intervalDeployTime;
        public Behaviour_Creator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(OnCreatorMonsterUpdate);
        }

        private void OnCreatorMonsterUpdate() {
            if (intervalDeployTime < 0) {
                CreateMonster();
                intervalDeployTime = GetIntervalTime();
            } else {
                intervalDeployTime -= Time.deltaTime;
            }
        }

        //创建怪物
        private void CreateMonster() {
            Entity objEntity = Obj.Instance.LoadEntity(entity.EntitySetting.CreatorSetting.CreatorObjSign);
            float xRand = Random.Range(-entity.EntitySetting.CreatorSetting.CreatorObjDistance, entity.EntitySetting.CreatorSetting.CreatorObjDistance);
            float zRand = Random.Range(-entity.EntitySetting.CreatorSetting.CreatorObjDistance, entity.EntitySetting.CreatorSetting.CreatorObjDistance);
            Vector3 createPosition = entity.Prefab.transform.position + new Vector3(xRand, 2, zRand);
            objEntity.Prefab.transform.position = createPosition;
        }

        //创建间隔时间
        private float GetIntervalTime() {
            Vector2 intervalTime = entity.EntitySetting.CreatorSetting.CreatorObjIntervalTime;
            return Random.Range(intervalTime.x, intervalTime.y);
        }

        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnCreatorMonsterUpdate);
        }
    }
}