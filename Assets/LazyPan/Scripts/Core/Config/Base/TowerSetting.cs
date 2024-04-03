using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/TowerSetting", fileName = "TowerSetting")]
    public class TowerSetting : ScriptableObject {
        [Tooltip("攻击力")] public int Attack;
        [Header("攻击间隔时间")] public float AttackIntervalTime;
        [Header("攻击距离")] public float AttackRange;
        [Header("充能速度")] public float ChargeEnergySpeed;
        [Header("能量上限")] public float MaxEnergy;
        [Header("能量掉落速度")] public float DownEnergySpeed;
        [Header("默认范围旋转角度")] public float RangeRotateAngle;
    }
}