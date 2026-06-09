using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class RoomMoodAnomaly : AnomalyBase
{
    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;

    [Header("Anomaly Color Adjustments")]
    [SerializeField] private float anomalyPostExposure = -1.2f;
    [SerializeField] private float anomalyContrast = 35f;
    [SerializeField] private Color anomalyColorFilter = Color.red;
    [SerializeField] private float anomalySaturation = -30f;

    [Header("Anomaly Vignette")]
    [SerializeField] private Color anomalyVignetteColor = Color.black;
    [SerializeField] private float anomalyVignetteIntensity = 0.55f;

    [Header("Anomaly Bloom")]
    [SerializeField] private float anomalyBloomIntensity = 1.2f;
    [SerializeField] private float anomalyBloomThreshold = 0.8f;

    [Header("Transition")]
    [SerializeField] private float activateDuration = 15f;
    [SerializeField] private float resetDuration = 1f;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private Bloom bloom;

    private float originalPostExposure;
    private float originalContrast;
    private Color originalColorFilter;
    private float originalSaturation;

    private Color originalVignetteColor;
    private float originalVignetteIntensity;

    private float originalBloomIntensity;
    private float originalBloomThreshold;

    private Coroutine moodRoutine;

    private void Awake()
    {
        if (globalVolume == null)
        {
            globalVolume = FindFirstObjectByType<Volume>();
        }

        if (globalVolume == null || globalVolume.profile == null)
        {
            Debug.LogError("Global Volume 또는 Volume Profile을 찾지 못했습니다.");
            return;
        }

        globalVolume.profile = Instantiate(globalVolume.profile);

        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            originalPostExposure = colorAdjustments.postExposure.value;
            originalContrast = colorAdjustments.contrast.value;
            originalColorFilter = colorAdjustments.colorFilter.value;
            originalSaturation = colorAdjustments.saturation.value;
        }

        if (globalVolume.profile.TryGet(out vignette))
        {
            originalVignetteColor = vignette.color.value;
            originalVignetteIntensity = vignette.intensity.value;
        }

        if (globalVolume.profile.TryGet(out bloom))
        {
            originalBloomIntensity = bloom.intensity.value;
            originalBloomThreshold = bloom.threshold.value;
        }
    }

    public override void ActivateAnomaly()
    {
        if (globalVolume == null || globalVolume.profile == null)
        {
            return;
        }

        if (moodRoutine != null)
        {
            StopCoroutine(moodRoutine);
        }

        moodRoutine = StartCoroutine(MoodRoutine(true, activateDuration));
    }

    public override void ResetAnomaly()
    {
        if (globalVolume == null || globalVolume.profile == null)
        {
            return;
        }

        if (moodRoutine != null)
        {
            StopCoroutine(moodRoutine);
        }

        moodRoutine = StartCoroutine(MoodRoutine(false, resetDuration));
    }

    private IEnumerator MoodRoutine(bool toAnomaly, float transitionDuration)
    {
        float timer = 0f;

        float startPostExposure = colorAdjustments != null ? colorAdjustments.postExposure.value : 0f;
        float startContrast = colorAdjustments != null ? colorAdjustments.contrast.value : 0f;
        Color startColorFilter = colorAdjustments != null ? colorAdjustments.colorFilter.value : Color.white;
        float startSaturation = colorAdjustments != null ? colorAdjustments.saturation.value : 0f;

        Color startVignetteColor = vignette != null ? vignette.color.value : Color.black;
        float startVignetteIntensity = vignette != null ? vignette.intensity.value : 0f;

        float startBloomIntensity = bloom != null ? bloom.intensity.value : 0f;
        float startBloomThreshold = bloom != null ? bloom.threshold.value : 1f;

        float targetPostExposure = toAnomaly ? anomalyPostExposure : originalPostExposure;
        float targetContrast = toAnomaly ? anomalyContrast : originalContrast;
        Color targetColorFilter = toAnomaly ? anomalyColorFilter : originalColorFilter;
        float targetSaturation = toAnomaly ? anomalySaturation : originalSaturation;

        Color targetVignetteColor = toAnomaly ? anomalyVignetteColor : originalVignetteColor;
        float targetVignetteIntensity = toAnomaly ? anomalyVignetteIntensity : originalVignetteIntensity;

        float targetBloomIntensity = toAnomaly ? anomalyBloomIntensity : originalBloomIntensity;
        float targetBloomThreshold = toAnomaly ? anomalyBloomThreshold : originalBloomThreshold;

        if (transitionDuration <= 0f)
        {
            ApplyMoodValues(
                targetPostExposure,
                targetContrast,
                targetColorFilter,
                targetSaturation,
                targetVignetteColor,
                targetVignetteIntensity,
                targetBloomIntensity,
                targetBloomThreshold
            );

            moodRoutine = null;
            yield break;
        }

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionDuration);

            ApplyMoodValues(
                Mathf.Lerp(startPostExposure, targetPostExposure, t),
                Mathf.Lerp(startContrast, targetContrast, t),
                Color.Lerp(startColorFilter, targetColorFilter, t),
                Mathf.Lerp(startSaturation, targetSaturation, t),
                Color.Lerp(startVignetteColor, targetVignetteColor, t),
                Mathf.Lerp(startVignetteIntensity, targetVignetteIntensity, t),
                Mathf.Lerp(startBloomIntensity, targetBloomIntensity, t),
                Mathf.Lerp(startBloomThreshold, targetBloomThreshold, t)
            );

            yield return null;
        }

        ApplyMoodValues(
            targetPostExposure,
            targetContrast,
            targetColorFilter,
            targetSaturation,
            targetVignetteColor,
            targetVignetteIntensity,
            targetBloomIntensity,
            targetBloomThreshold
        );

        moodRoutine = null;
    }

    private void ApplyMoodValues(
        float postExposure,
        float contrast,
        Color colorFilter,
        float saturation,
        Color vignetteColor,
        float vignetteIntensity,
        float bloomIntensity,
        float bloomThreshold)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = postExposure;
            colorAdjustments.contrast.value = contrast;
            colorAdjustments.colorFilter.value = colorFilter;
            colorAdjustments.saturation.value = saturation;
        }

        if (vignette != null)
        {
            vignette.color.value = vignetteColor;
            vignette.intensity.value = vignetteIntensity;
        }

        if (bloom != null)
        {
            bloom.intensity.value = bloomIntensity;
            bloom.threshold.value = bloomThreshold;
        }
    }
}
