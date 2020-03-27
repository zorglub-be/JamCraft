using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    [Tooltip("Delay before the first tick")]
    [SerializeField] private float _startDelay;
    [Tooltip("Delay between ticks after the first")]
    [SerializeField] private float _repetitionInterval;
    [Tooltip("Repetitions beyond the first tick. Unlimited if < 0\r\nA value of 0 will result in 1 single tick")]
    [SerializeField] private int _repetitions;
    public UnityEvent OnTick;

    private void OnEnable()
    {
        StartCoroutine(TimerCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(_startDelay);
        OnTick?.Invoke();
        //repeats infinitely if _repetitions < 0
        for (int i = _repetitions; i != 0; i--)
        {
            yield return new WaitForSeconds(_repetitionInterval);
            OnTick?.Invoke();
            //avoid int overflow
            i = i < -1 ? -1 : i;
        }
    }
}

