using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/BuffSetting", fileName = "BuffSetting")]
    public class BuffSetting : ScriptableObject {
        public List<BuffSettingInfo> BuffSettingInfo;
    }

    [Serializable]
    public class BuffSettingInfo {
        [Tooltip("标识")] public string Sign;
        [Tooltip("描述")] public string Description;
        [Tooltip("图标")] public Sprite Icon;
        [Tooltip("行为标识")] public string BehaviourSign;
        [Tooltip("是否可升级")] public bool CanUpgrade;
    }
}