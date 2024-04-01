using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/TowerSetting", fileName = "TowerSetting")]
    public class TowerSetting : ScriptableObject {
        [Tooltip("攻击力")] public int Attack;
    }
}