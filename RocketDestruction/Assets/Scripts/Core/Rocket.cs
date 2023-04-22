using DG.Tweening;
using RocketDestruction.SignalSystem;
using RocketDestruction.Utility;
using Supyrb;
using UnityEngine;
using UnityEngine.Pool;

namespace RocketDestruction.Core
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private Vector3 tweenStartPos;
        [SerializeField] private Vector3 tweenEndPos;

        private float                       _multiplier = 1f;
        private float                       _speed;
        private Rigidbody                   _rigidbody;
        private bool                        _launchConfirm;
        private IObjectPool<ParticleSystem> _particlePool;
        private IObjectPool<Rocket>         _rocketPool;
        private Rocket                      _rocketReference;
        private Vector3                     _initialRotation;
        private bool                        _willHit;

        private float _rocketFlyTime;

        private void Awake()
        {
            _rocketReference = GetComponent<Rocket>();
            _rigidbody       = GetComponent<Rigidbody>();
            _multiplier      = PlayerPrefs.GetFloat(AtConstants.RocketSpeedMultiplier, 1f);
            _initialRotation = transform.localRotation.eulerAngles;

            _particlePool = FindObjectOfType<ParticlePool>()
                .Pool;

            Signals.Get<RocketSpeedMultiplierChanged>()
                .AddListener(OnRocketSpeedMultiplierChanged);
        }

        private void Update()
        {
            if (_willHit || !_launchConfirm) return;

            if (_rocketFlyTime >= AtConstants.RocketMissWaitTime)
            {
                _speed         = 0f;
                _launchConfirm = false;
                _rocketPool.Release(_rocketReference);
                Signals.Get<RocketMissedTarget>()
                    .Dispatch();
            }
            else
            {
                _rocketFlyTime += Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (!_launchConfirm) return;

            _speed              += Time.deltaTime * AtConstants.RocketAcceleration * _multiplier;
            _rigidbody.velocity =  transform.up   * _speed;
        }

        private void OnEnable()
        {
            transform.localPosition = tweenStartPos;
            transform.localRotation = Quaternion.Euler(_initialRotation);
            transform.DOLocalMove(tweenEndPos, 2)
                .SetEase(Ease.InOutQuad)
                .onStepComplete = OnRocketReadyForLaunch;
        }

        private void OnDestroy()
        {
            Signals.Get<RocketSpeedMultiplierChanged>()
                .RemoveListener(OnRocketSpeedMultiplierChanged);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_willHit)
            {
                Signals.Get<RocketMissedTarget>()
                    .Dispatch();
            }

            CreateExplosion(collision);
            _speed         = 0f;
            _launchConfirm = false;
            _rocketPool.Release(_rocketReference);
        }

        private void CreateExplosion(Collision collision)
        {
            ContactPoint   contact           = collision.GetContact(0);
            Quaternion     rot               = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3        pos               = contact.point;
            ParticleSystem explosion         = _particlePool.Get();
            Transform      particleTransform = explosion.transform;
            particleTransform.position = pos;
            particleTransform.rotation = rot;
        }

        private void OnRocketReadyForLaunch()
        {
            _launchConfirm = true;
        }

        private void OnRocketSpeedMultiplierChanged(int multiplierValue)
        {
            _multiplier = multiplierValue;
        }

        private void AddMissRotation()
        {
            float x = _initialRotation.x;
            float y = _initialRotation.y;

            float rand = Random.value;
            if (rand > 0.5)
            {
                x = Random.Range(x + 10, x + 45);
                y = Random.Range(y + 10, y + 45);
            }
            else
            {
                x = Random.Range(x - 10, x - 45);
                y = Random.Range(y - 10, y - 45);
            }

            Vector3 missRotation = new Vector3(x, y, _initialRotation.z);
            transform.localRotation = Quaternion.Euler(missRotation);
        }

        public void SetPool(IObjectPool<Rocket> pool)
        {
            _rocketPool = pool;
        }

        public void SetRocketHitStatus(bool willHit)
        {
            _willHit = willHit;
            if (willHit) return;

            _rocketFlyTime = 0f;
            AddMissRotation();
        }
    }
}