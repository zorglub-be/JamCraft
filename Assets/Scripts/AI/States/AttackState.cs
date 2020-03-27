using System.Threading;
using UnityEngine;

public class AttackState : IState
{
    private bool _finished;
    private GameEffect[] _effects;
    private GameObject _gameObject;
    private float _attackDelay;
    public DelayEffect _delayEffect;
    private CancellationTokenSource _cancellationTokenSource;

    public bool Finished => _finished;

    public AttackState(GameEffect[] effects, GameObject gameObject, float delay = 0)
    {
        _attackDelay = delay;
        _effects = effects;
        _gameObject = gameObject;
        if (_attackDelay > 0f)
        {
            _delayEffect = ScriptableObject.CreateInstance<DelayEffect>();
            _delayEffect.delay = delay;
        }
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, GameState.Instance.CancellationToken);
        _finished = false;
        if (_attackDelay > 0)
            _delayEffect.Execute(_gameObject, () => ExecuteEffects(tokenSource), tokenSource);
        else
            ExecuteEffects(tokenSource);
    }

    public void ExecuteEffects(CancellationTokenSource tokenSource)
    {
        foreach (var effect in _effects)
        {
            effect.Execute(_gameObject, () => _finished = true, tokenSource);
        }
        
    }

    public void OnExit()
    {
        _cancellationTokenSource.Cancel();
    }

}