using RocketDestruction.SignalSystem;
using Supyrb;
using UnityEngine;

namespace RocketDestruction.Core
{
    public class Target : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private Animator    _animator;

        private void Awake()
        {
            _animator    = GetComponentInChildren<Animator>();
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rocket rocket = collision.gameObject.GetComponent<Rocket>();
            if (rocket == null) return;
            EnableRagdoll();
        }

        private void EnableRagdoll()
        {
            Signals.Get<RocketHitTarget>()
                .Dispatch();
            _animator.enabled = false;
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _rigidbodies[i]
                    .isKinematic = false;
            }
        }
    }
}