using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Effect Sequence")]
public class GameEffectSequence : GameEffect
{
    [SerializeField] private bool _waitForAllFinished;
    [SerializeField] private bool _disableInputDuringSequence = true;
    [SerializeField] private GameEffectSequenceElement[] _sequence;

    public override async void Execute(GameObject source, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        if (_disableInputDuringSequence)
        {
            var input = source.GetComponent<PlayerInputManager>();
            if (input)
            {
                input.enabled = false;
                callback = (() => input.enabled = true) + callback;
            }
        }
        var currentAction = _waitForAllFinished ? null : callback;
        var sequencers = new GameEffectSequencer[_sequence.Length];
        var token = tokenSource?.Token ?? GameState.Instance.CancellationToken;
        for (int i = _sequence.Length-1; i >=0 ; i--)
        {
            sequencers[i] = new GameEffectSequencer(source, _sequence[i], currentAction, tokenSource);
            currentAction = sequencers[i].Execute;
        }
        currentAction?.Invoke();
        if (_waitForAllFinished)
        {
            var allFinished = false;
            while (!allFinished)
            {
                await Task.Yield();
                if (token.IsCancellationRequested)
                    return;
                for (int i = 0; i < sequencers.Length; i++)
                {
                    if (sequencers[i].Finished == false)
                        break;
                    if (i == sequencers.Length-1)
                        allFinished = true;
                }
            }
            callback?.Invoke();
        }
    }


    private class GameEffectSequencer
    {
        private GameEffectSequenceElement _sequenceElement;
        private GameObject _source;
        private Action _callback;
        private bool _finished;
        private CancellationTokenSource _tokenSource;
        public bool Finished => _finished;

        public GameEffectSequencer(GameObject source, GameEffectSequenceElement sequenceElement, Action callback,
            CancellationTokenSource tokenSourceSource)
        {
            _source = source;
            _sequenceElement = sequenceElement;
            _callback = callback;
            _tokenSource = tokenSourceSource;
        }

        public async void Execute()
        {
            if (_sequenceElement.waitForSeconds > 0f)
            {
                _sequenceElement.effect.Execute(_source, () => _finished = true, _tokenSource);
                var wait = WaitForSeconds(_sequenceElement.waitForSeconds, _tokenSource);
                await wait;
                var token = _tokenSource?.Token ?? GameState.Instance.CancellationToken;
                if (token.IsCancellationRequested)
                    return;
                _callback?.Invoke();
                return;
            }

            if (_sequenceElement.waitForFinish)
            {
                _sequenceElement.effect.Execute(_source, _callback, _tokenSource);
                _finished = true;
                return;
            }

            _sequenceElement.effect.Execute(_source, () => _finished = true, _tokenSource);
            _callback?.Invoke();
        }
        private async Task WaitForSeconds(float duration, CancellationTokenSource tokenSource)
        {
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                await Task.Yield();
                var token = tokenSource?.Token ?? GameState.Instance.CancellationToken;
                if (token.IsCancellationRequested)
                    return;
            }
        }        
    }
    
    [Serializable]
    private struct GameEffectSequenceElement
    {
        public GameEffect effect;
        [Tooltip("Starts the next element in the sequence after x seconds\r\nThis has precedence over Wait for finish")]
        public float waitForSeconds;
        [Tooltip("Starts the next element in the sequence after this one is finished\r\nHas no effect if Wait For Seconds > 0")]
        public bool waitForFinish;
    }
}