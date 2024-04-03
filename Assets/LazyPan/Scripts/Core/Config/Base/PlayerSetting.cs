using UnityEngine;
using UnityEngine.Serialization;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/PlayerSetting", fileName = "PlayerSetting")]
    public class PlayerSetting : ScriptableObject {
        [Header("旋转速度")] public float RotateSpeed;
        [Header("移动速度")] public float MovementSpeed;
        [Header("重力速度")] public float GravitySpeed;
        [Header("冲刺速度")] public float SprintSpeed;
        [Header("冲刺时间")] public float SprintTime;
        [Header("血量回复速度")] public float HealthRecoverSpeed;
        [FormerlySerializedAs("HealthMax")] [Header("血量上限")] public float MaxHealth;
        [FormerlySerializedAs("ExperienceMax")] [Header("经验值最大值")] public float MaxExperience;
        
    }
}