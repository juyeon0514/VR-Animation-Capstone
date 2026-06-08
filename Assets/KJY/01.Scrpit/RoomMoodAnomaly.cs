using System.Collections;
using UnityEngine;

public class RoomMoodAnomaly : AnomalyBase
{
    [SerializeField] private Light[] roomLights;
    [SerializeField] private Color anomalyColor = Color.red;
    [SerializeField] private float anomalyIntensity = 0.5f;
    [SerializeField] private float duration = 5f;

    private Color[] originalColors;
    private float[] originalIntensities;
    private Coroutine moodRoutine;

    private void Awake()
    {
        originalColors = new Color[roomLights.Length];
        originalIntensities = new float[roomLights.Length];

        for (int i = 0; i < roomLights.Length; i++)
        {
            if (roomLights[i] == null)
            {
                continue;
            }

            originalColors[i] = roomLights[i].color;
            originalIntensities[i] = roomLights[i].intensity;
        }
    }

    public override void ActivateAnomaly()
    {
        if (moodRoutine != null)
        {
            StopCoroutine(moodRoutine);
        }

        moodRoutine = StartCoroutine(MoodRoutine());
    }

    public override void ResetAnomaly()
    {
        if (moodRoutine != null)
        {
            StopCoroutine(moodRoutine);
            moodRoutine = null;
        }

        for (int i = 0; i < roomLights.Length; i++)
        {
            if (roomLights[i] == null)
            {
                continue;
            }

            roomLights[i].color = originalColors[i];
            roomLights[i].intensity = originalIntensities[i];
        }
    }

    private IEnumerator MoodRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            for (int i = 0; i < roomLights.Length; i++)
            {
                if (roomLights[i] == null)
                {
                    continue;
                }

                roomLights[i].color = Color.Lerp(originalColors[i], anomalyColor, t);
                roomLights[i].intensity = Mathf.Lerp(originalIntensities[i], anomalyIntensity, t);
            }

            yield return null;
        }

        moodRoutine = null;
    }
}
