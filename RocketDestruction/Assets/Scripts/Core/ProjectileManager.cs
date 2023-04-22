using RocketDestruction.Core.ScriptableObjects;
using RocketDestruction.SignalSystem;
using RocketDestruction.Utility;
using Supyrb;
using UnityEngine;

namespace RocketDestruction.Core
{
    public class ProjectileManager : MonoBehaviour, IRdManager
    {
        [SerializeField] private RocketPool[] rocketPools;

        private RocketPool _currentRocketPool;
        private int        _rocketNumber;
        private bool       _willRocketHit;

        private void OnEnable()
        {
            Signals.Get<LaunchRocket>()
                .AddListener(OnLaunchRocket);

            Signals.Get<RocketHitStatus>()
                .AddListener(SetHitStatusForRockets);
        }

        private void OnDisable()
        {
            Signals.Get<LaunchRocket>()
                .RemoveListener(OnLaunchRocket);
            
            Signals.Get<RocketHitStatus>()
                .RemoveListener(SetHitStatusForRockets);
        }

        private void OnLaunchRocket()
        {
            if (rocketPools.Length == 0)
            {
                Debug.LogError("No Rocket Pools Available!");
                return;
            }

            GameMode gameMode = GameManager.Instance.GameMode;

            if (gameMode == GameMode.TripleBarrage)
            {
                LaunchTripleBarrage();
            }
            else
            {
                _currentRocketPool = rocketPools[_rocketNumber];
                ThrowRocketProjectile();
                _rocketNumber++;

                if (_rocketNumber == AtConstants.MaxRocketsCount)
                {
                    Signals.Get<RocketBarrageFinished>()
                        .Dispatch();
                }
            }
        }

        private void LaunchTripleBarrage()
        {
            Signals.Get<RocketBarrageFinished>()
                .Dispatch();

            for (int i = 0; i < rocketPools.Length; i++, _rocketNumber++)
            {
                _currentRocketPool = rocketPools[_rocketNumber];
                ThrowRocketProjectile();
            }
        }

        private void ThrowRocketProjectile()
        {
            Rocket rocket = _currentRocketPool.Pool.Get();
            rocket.SetPool(_currentRocketPool.Pool);
            rocket.SetRocketHitStatus(_willRocketHit);
        }

        private void SetHitStatusForRockets(bool willHit)
        {
            _willRocketHit = willHit;
        }

        public void Init()
        {
            _rocketNumber = 0;
            for (int i = 0; i < rocketPools.Length; i++)
            {
                rocketPools[i]
                    .InitializePool();
            }
        }

        public void Reset()
        {
            _rocketNumber = 0;
            for (int i = 0; i < rocketPools.Length; i++)
            {
                rocketPools[i]
                    .Pool.Clear();
            }
        }
    }
}