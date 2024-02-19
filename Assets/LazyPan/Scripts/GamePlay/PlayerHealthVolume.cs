using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealthVolume : MonoBehaviour {
    //备份后处理参数
    private float DefaultChromaticAberrationIntensityValue;
    private float DefaultVignetteIntensityValue;
    private Volume volume;
    private ChromaticAberration ChromaticAberration;
    private Vignette Vignette;

    public float changeSpeed;
    public static float targetChromaticAberrationValue;
    public static float targetVignetteValue;

    void Start() {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out ChromaticAberration)) {
            DefaultChromaticAberrationIntensityValue = ChromaticAberration.intensity.value;
        }

        if (volume.profile.TryGet(out Vignette)) {
            DefaultVignetteIntensityValue = Vignette.intensity.value;
        }
    }

    public static void SetTargetValue(float ratio) {
        targetChromaticAberrationValue = 1 - ratio;
        targetVignetteValue = (1 - ratio) * 0.3f;
    }

    void Update() {
        if (Mathf.Abs(ChromaticAberration.intensity.value - targetChromaticAberrationValue) > 0.01f) {
            ChromaticAberration.intensity.value = Mathf.Lerp(ChromaticAberration.intensity.value, targetChromaticAberrationValue, Time.deltaTime * changeSpeed);
        } else {
            ChromaticAberration.intensity.value = targetChromaticAberrationValue;
        }

        if (Mathf.Abs(Vignette.intensity.value - targetVignetteValue) > 0.01f) {
            Vignette.intensity.value = Mathf.Lerp(Vignette.intensity.value, targetVignetteValue, Time.deltaTime * changeSpeed);
        } else {
            Vignette.intensity.value = targetVignetteValue;
        }
    }

    private void OnApplicationQuit() {
        ChromaticAberration.intensity.value = DefaultChromaticAberrationIntensityValue;
        Vignette.intensity.value = DefaultVignetteIntensityValue;
    }
}