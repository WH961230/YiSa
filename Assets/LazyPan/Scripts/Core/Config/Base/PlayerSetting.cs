using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/PlayerSetting", fileName = "PlayerSetting")]
    public class PlayerSetting : ScriptableObject {
        [Header("血量")] public int HealthPoint;
        [Header("旋转速度")] public float RotateSpeed;
        [Header("重力速度")] public float GravitySpeed;
    }
}