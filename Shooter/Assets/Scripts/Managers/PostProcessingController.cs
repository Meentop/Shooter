using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] private float vignetteTargetValue;
    [SerializeField] private float bloodVignetteSpeed;

    private bool _isGettingDamage = false;
    private float curVignetteValue;

    private Vignette _vignette;
    void Start()
    {
        curVignetteValue = vignetteTargetValue;
    }

    void Update()
    {
        if (_isGettingDamage)
        {
            GetDamageEffect();
        }
        
    }

    private void GetDamageEffect()
    {
        globalVolume.profile.TryGet(out _vignette);
        curVignetteValue = Mathf.Lerp(curVignetteValue, 0, bloodVignetteSpeed);
        _vignette.intensity.value = curVignetteValue;
        if (curVignetteValue <= 0.01) 
        {
            _isGettingDamage = false;
            curVignetteValue = vignetteTargetValue;
        }
        print(curVignetteValue.ToString("F3"));
    }

    public void SetDamageBool(bool isGettingDamage)
    {
        _isGettingDamage = isGettingDamage;
    }
}
