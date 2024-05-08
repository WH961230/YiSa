using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/TowerSetting", fileName = "TowerSetting")]
    public class TowerSetting : ScriptableObject {
        [Header("充能速度")] public float ChargeEnergySpeed;
        [Header("能量上限")] public float MaxEnergy;
        [Header("能量掉落速度")] public float DownEnergySpeed;
        [Header("默认范围旋转角度")] public float RangeRotateAngle;
    }
}