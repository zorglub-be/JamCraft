using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Flamable : MonoBehaviour
{
    public UnityEvent OnStartBurning;
    public UnityEvent OnTick;
    public UnityEvent OnStopBurning;
    private IDamageable _damageable;
    private bool _isBurning;

    private void Awake()
    {
        _damageable = GetComponentInChildren<IDamageable>();
    }

    private void OnDisable()
    {
        if (_isBurning)
            OnStopBurning?.Invoke();
        StopAllCoroutines();
    }

    public void Burn(int tickDamage, int duration)
    {
        if (_isBurning == false)
            StartCoroutine(BurnCoroutine(tickDamage, duration));
    }

    private IEnumerator BurnCoroutine(int tickDamage, int duration)
    {
        _isBurning = true;
        OnStartBurning?.Invoke();
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForSeconds(1);
            if (_damageable != null)
                _damageable.TakeDamage(tickDamage);
            OnTick?.Invoke();
        }
        _isBurning = false;
        OnStopBurning?.Invoke();
    }
}