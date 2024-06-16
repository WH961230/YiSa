// using UnityEngine;
//
// namespace LazyPan {
//     public class Behaviour_Auto_Grenade : Behaviour {
//         /*等级*/
//         private int grenadeNum = 1;
//         private float deploy;
//         private BuffSettingInfo buffSettingInfo;
//         private float attackIntervalTime;
//         private float attackDamage;
//         public Behaviour_Auto_Grenade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             Data.Instance.OnUpdateEvent.AddListener(Grenade);
//             /*获取配置*/
//             Loader.LoadSetting().BuffSetting.GetSettingBySign(behaviourSign, out buffSettingInfo);
//             /*攻击间隔时间*/
//             buffSettingInfo.GetParam("AttackIntervalTime", out string attackintervaltime);
//             attackIntervalTime = float.Parse(attackintervaltime);
//             /*攻击伤害*/
//             buffSettingInfo.GetParam("AttackDamage", out string attackdamage);
//             attackDamage = float.Parse(attackdamage);
//         }
//
//         /*手雷*/
//         private void Grenade() {
//             if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
//                 if (deploy > 0) {
//                     deploy -= Time.deltaTime;
//                 } else {
//                     GrenadeThrow();
//                     deploy = attackIntervalTime;
//                 }
//             }
//         }
//
//         /*手雷丢出*/
//         private void GrenadeThrow() {
//             /*手雷运动的曲线*/
//             GrenadeBoom();
//         }
//
//         /*手雷爆炸*/
//         private void GrenadeBoom() {
//             /*圆形爆炸区域*/
//             
//         }
//
//         public override void Upgrade() {
//             base.Upgrade();
//             grenadeNum++;
//             if (grenadeNum > buffSettingInfo.UpgradeLimit) {
//                 grenadeNum = buffSettingInfo.UpgradeLimit;
//             }
//         }
//
//         public override void Clear() {
//             base.Clear();
//             Data.Instance.OnUpdateEvent.RemoveListener(Grenade);
//         }
//     }
// }