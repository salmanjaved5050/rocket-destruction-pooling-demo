using UnityEngine;
using UnityEngine.Pool;

namespace RocketDestruction.Core
{
    public class ParticlePool : MonoBehaviour
    {
        [SerializeField] private GameObject particleSystemPrefab;

        private IObjectPool<ParticleSystem> _pool;

        private const int PrewarmPoolCount = 3;

        public IObjectPool<ParticleSystem> Pool
        {
            get
            {
                if (_pool != null) return _pool;
                _pool = new ObjectPool<ParticleSystem>(OnCreateParticle, OnParticleRetrieved, OnParticleReleased,
                    OnParticleDestroyed, false, 50, 100);
                PrewarmPool();
                return _pool;
            }
        }

        private void Start()
        {
            _pool = new ObjectPool<ParticleSystem>(OnCreateParticle, OnParticleRetrieved, OnParticleReleased,
                OnParticleDestroyed, true, 50, 100);
            PrewarmPool();
        }

        private void PrewarmPool()
        {
            for (int i = 0; i < PrewarmPoolCount; i++)
            {
                _pool.Get();
            }
        }

        private ParticleSystem OnCreateParticle()
        {
            GameObject     go = Instantiate(particleSystemPrefab);
            ParticleSystem ps = go.GetComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystem.MainModule main = ps.main;
            main.duration      = 3;
            main.startLifetime = 1;
            main.loop          = false;

            ReturnParticleToPool returnToPool = go.AddComponent<ReturnParticleToPool>();
            returnToPool.Pool = Pool;
            return ps;
        }

        private void OnParticleReleased(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
        }

        private void OnParticleRetrieved(ParticleSystem system)
        {
            system.gameObject.SetActive(true);
        }

        private void OnParticleDestroyed(ParticleSystem system)
        {
            Destroy(system.gameObject);
        }
    }
}