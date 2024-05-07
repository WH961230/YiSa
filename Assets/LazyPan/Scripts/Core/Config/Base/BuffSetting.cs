using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/BuffSetting", fileName = "BuffSetting")]
    public class BuffSetting : ScriptableObject {
        public List<BuffSettingInfo> BuffSettingInfo;

        /*根据等级获取描述*/
        public bool GetDescriptionByLevel(string sign, int level, out string description) {
            foreach (BuffSettingInfo info in BuffSettingInfo) {
                if (info.Sign == sign && info.UpgradeDescriptions.Count >= level) {
                    description = info.UpgradeDescriptions[level - 1];
                    return true;
                }
            }

            description = null;
            return false;
        }

        /*获取配置*/
        public bool GetSettingBySign(string buffsign, out BuffSettingInfo buffsettinginfo) {
            foreach (BuffSettingInfo info in BuffSettingInfo) {
                if (info.BehaviourSign == buffsign) {
                    buffsettinginfo = info;
                    return true;
                }
            }

            buffsettinginfo = default;
            return false;
        }
    }

    [Serializable]
    public class BuffSettingInfo {
        [Tooltip("标识")] public string Sign;
        [Tooltip("描述")] public string Description;
        [Tooltip("升级描述列表")] public List<string> UpgradeDescriptions;
        [Tooltip("升级参数列表")] public List<BuffParamInfo> UpgradeParams;
        [Tooltip("图标")] public Sprite Icon;
        [Tooltip("下标图标")] public Sprite SubscriptIcon;
        [Tooltip("行为标识")] public string BehaviourSign;
        [Tooltip("是否可升级")] public bool CanUpgrade;
        [Tooltip("行为升级上限")] public int UpgradeLimit;

        /*根据标识获取参数*/
        public bool GetParam(string sign, out string param) {
            foreach (BuffParamInfo paraminfo in UpgradeParams) {
                if (paraminfo.Sign == sign) {
                    param = paraminfo.Param;
                    return true;
                }
            }

            param = null;
            return false;
        }
    }

    [Serializable]
    public class BuffParamInfo {
        [Tooltip("标识")] public string Sign;
        [Tooltip("参数")] public string Param;
    }
}