using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Effect Sequence")]
public class GameEffectSequence : GameEffect
{
    [SerializeField] private GameEffectSequenceElement[] _sequence;
    public override void Execute(GameObject source, Action callback = null)
    {
        var currentAction = callback;
        for (int i = _sequence.Length-1; i >=0 ; i--)
        {
            var sequencer = new GameEffectSequencer(source, _sequence[i], currentAction);
            currentAction = sequencer.Execute;
        }
        currentAction?.Invoke();
    }

    private class GameEffectSequencer
    {
        private GameEffectSequenceElement _sequenceElement;
        private GameObject _source;
        private Action _callback;

        public GameEffectSequencer(GameObject source, GameEffectSequenceElement sequenceElement, Action callback)
        {
            _source = source;
            _sequenceElement = sequenceElement;
            _callback = callback;
        }

        public async void Execute()
        {
            if (_sequenceElement.waitForSeconds > 0f)
            {
                _sequenceElement.effect.Execute(_source);
                await Task.Delay((int) (_sequenceElement.waitForSeconds * 1000));
                _callback?.Invoke();
                return;
            }

            if (_sequenceElement.waitForFinish)
            {
                _sequenceElement.effect.Execute(_source, _callback);
                return;
            }

            _sequenceElement.effect.Execute(_source);
            _callback?.Invoke();
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