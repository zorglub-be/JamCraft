using System;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightFlicker2D : MonoBehaviour
{
    [SerializeField] private float timeScale = 4f;
    [SerializeField] private float _minFlickerInterval = 5f;
    [SerializeField] private float _maxFlickerInterval = 10f;
    
    private float _intensity = 1.5f;
    private Light2D _light2D;
    private float _randomSeed;
    private float _timer = 0f;

    void Awake()
    {
        _light2D = GetComponent<Light2D>();
        _randomSeed = Random.Range(1, 9999);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer > Random.Range(_minFlickerInterval, _maxFlickerInterval))
        {
            StartCoroutine(Flicker());
        }

        if (_timer > _maxFlickerInterval ) _timer = 0;
        }

    private IEnumerator Flicker()
    {
        float intensity = _intensity *
                          Mathf.PerlinNoise(Random.Range(Time.time * timeScale, Time.time * timeScale), _randomSeed);
        _light2D.intensity = intensity;
        
        yield return null;
    }
}
