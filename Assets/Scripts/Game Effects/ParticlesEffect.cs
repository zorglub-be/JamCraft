using System;
using System.Linq;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Emit Particles")]
public class ParticlesEffect : GameEffect
{
    [Tooltip("Name of the gameobject in the source hierarchy holding the particles system that should emit")]
    [SerializeField] private string _particlesName;
    [SerializeField] private float _duration;


    public override void Execute(GameObject source, Action callback=null, CancellationTokenSource tokenSource = null)
    {
        var particleSystems = source.GetComponentsInChildren<ParticleSystem>(false);
        var system = particleSystems.FirstOrDefault(sys => sys.name == _particlesName);
        if (ReferenceEquals(system, null))
            return;
        EmitForSeconds(system, _duration, callback, tokenSource);
    }
    private async void EmitForSeconds(ParticleSystem particles, float duration, Action callback,
        CancellationTokenSource tokenSource)
    {
        var token = GameState.Instance.CancellationToken;
        var emission = particles.emission;
        emission.enabled = true;
        var wait = WaitForSeconds(duration, tokenSource);
        await wait;
        emission.enabled = false;
        if (token.IsCancellationRequested)
            return;
        callback?.Invoke();
    }
    
}