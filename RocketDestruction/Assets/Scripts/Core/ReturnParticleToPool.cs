using UnityEngine;
using UnityEngine.Pool;

namespace RocketDestruction.Core
{
    public class ReturnParticleToPool : MonoBehaviour
    {
        public ParticleSystem              system;
        public IObjectPool<ParticleSystem> Pool;

        private void Start()
        {
            system = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            Pool.Release(system);
        }
    }
}