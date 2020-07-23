using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeAffector : MonoBehaviour
{

    public static TimeAffector instance;

    [Range(0.25f,3f)]
    public float duration;

    public float fadeInMultiplier = 5;

    [Range(0f,1f)]
    public float affectMultiplier;

    private bool slowMotionEnabled = false;

    [SerializeField]
    private Volume _volume;

    private ChromaticAberration _aberration;
    private Vignette _vignette;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        DontDestroyOnLoad(gameObject);

        _volume.profile.TryGet(out _aberration);
        _volume.profile.TryGet(out _vignette);

        _aberration.intensity.value = 0;
        _vignette.intensity.value = 0;
    }

    public void ToggleSlowMotion(bool value)
    {
        slowMotionEnabled = value;
        if(value)
            StartCoroutine(SlowMotion());
    }

    private IEnumerator SlowMotion()
    {
        float time = 0f;
        while (time < duration && slowMotionEnabled)
        {
            time = Mathf.Clamp(time + Time.unscaledDeltaTime, 0f, duration);

            if (time < duration / fadeInMultiplier)
            {
                float lerpTime = time * fadeInMultiplier;
                
                Time.timeScale = Mathf.Lerp(1f, 1f - affectMultiplier, lerpTime);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                _aberration.intensity.value = Mathf.Lerp(0f, 1f, lerpTime);
                _vignette.intensity.value = Mathf.Lerp(0f, 0.2f, lerpTime);
            }
            yield return null;
        }
        print("Slow-Motion dissabled!");
        
        _aberration.intensity.value = 0;
        _vignette.intensity.value = 0;
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        slowMotionEnabled = false;
    }
}
