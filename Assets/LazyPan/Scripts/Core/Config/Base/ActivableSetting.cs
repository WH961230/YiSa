using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/ActivableSetting", fileName = "ActivableSetting")]
    public class ActivableSetting : ScriptableObject {
        [Header("充能速度")] public float ChargeSpeed;
        [Header("充能掉落速度")] public float DownChargeSpeed;
        [Header("最大充能量")] public float MaxCharge;
    }
}