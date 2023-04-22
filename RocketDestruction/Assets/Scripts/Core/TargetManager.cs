using RocketDestruction.SignalSystem;
using RocketDestruction.Utility;
using Supyrb;
using UnityEngine;

namespace RocketDestruction.Core
{
    public class TargetManager : MonoBehaviour, IRdManager
    {
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private Transform  targetParentTransform;
        [SerializeField] private Vector3[]  targetSpawnPositions;

        private GameObject[] _spawnedTargets;

        private int _totalDeadTargets;

        private void OnEnable()
        {
            Signals.Get<RocketHitTarget>()
                .AddListener(TargetDead);

            Signals.Get<RocketMissedTarget>()
                .AddListener(TargetDead);
        }

        private void OnDisable()
        {
            Signals.Get<RocketHitTarget>()
                .RemoveListener(TargetDead);

            Signals.Get<RocketMissedTarget>()
                .RemoveListener(TargetDead);
        }

        public void Init()
        {
            _spawnedTargets = new GameObject[targetSpawnPositions.Length];
            SpawnTargets();
        }

        public void Reset()
        {
            DestroyTargets();
            SpawnTargets();
        }

        private void SpawnTargets()
        {
            for (int i = 0; i < targetSpawnPositions.Length; i++)
            {
                GameObject target = Instantiate(targetPrefab, targetParentTransform, false);
                target.transform.localPosition = targetSpawnPositions[i];

                _spawnedTargets[i] = target;
            }
        }

        private void DestroyTargets()
        {
            _totalDeadTargets = 0;
            for (int i = 0; i < _spawnedTargets.Length; i++)
            {
                Destroy(_spawnedTargets[i]);
            }
        }

        private void TargetDead()
        {
           _totalDeadTargets++;
            Signals.Get<TargetRekt>()
                .Dispatch(_totalDeadTargets);
            if (_totalDeadTargets == AtConstants.MaxTargetsCount)
            {
                Signals.Get<ResetLoop>()
                    .Dispatch();
            }
        }
    }
}