using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Emit Particles")]
public class ParticlesEffect : GameEffect
{
    [Tooltip("Name of the gameobject in the source hierarchy holding the particles system that should emit")]
    [SerializeField] private string _particlesName;
    [SerializeField] private float _duration;


    public override void Execute(GameObject source, Action callback=null)
    {
        var particleSystems = source.GetComponentsInChildren<ParticleSystem>(false);
        var system = particleSystems.FirstOrDefault(sys => sys.name == _particlesName);
        if (ReferenceEquals(system, null))
            return;
        EmitForSeconds(system, _duration, callback);
    }
    private async void EmitForSeconds(ParticleSystem particles, float duration, Action callback)
    {
        var emission = particles.emission;
        emission.enabled = true;
        await Task.Delay((int)duration * 1000);
        emission.enabled = false;
        callback?.Invoke();
    }
    
}