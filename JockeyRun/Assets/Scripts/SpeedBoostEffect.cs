using UnityEngine;

public class SpeedBoostEffect : MonoBehaviour
{
    [Header("Effect Object")]
    public GameObject lightningEffect;

    [Header("Settings")]
    public float defaultDuration = 2f;

    private Coroutine effectRoutine;

    private void Awake()
    {
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(false);
        }
    }

    public void PlayEffect()
    {
        PlayEffect(defaultDuration);
    }

    public void PlayEffect(float duration)
    {
        if (lightningEffect == null) return;

        if (effectRoutine != null)
        {
            StopCoroutine(effectRoutine);
        }

        effectRoutine = StartCoroutine(EffectTimer(duration));
    }

    private System.Collections.IEnumerator EffectTimer(float duration)
    {
        lightningEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        lightningEffect.SetActive(false);
        effectRoutine = null;
    }
}