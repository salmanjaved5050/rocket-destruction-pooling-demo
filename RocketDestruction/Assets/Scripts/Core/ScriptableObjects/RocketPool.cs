using UnityEngine;
using UnityEngine.Pool;

namespace RocketDestruction.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RocketPool", menuName = "Rocket/RocketPool")]
    public class RocketPool : ScriptableObject
    {
        [SerializeField] private GameObject rocketPrefab;

        private Transform           _rocketParent;
        private IObjectPool<Rocket> _pool;

        public IObjectPool<Rocket> Pool => _pool;

        private Rocket OnCreateRocket()
        {
            return Instantiate(rocketPrefab, _rocketParent)
                .GetComponent<Rocket>();
        }

        private void OnRocketRetrieved(Rocket rocket)
        {
            rocket.gameObject.SetActive(true);
        }

        private void OnRocketReleased(Rocket rocket)
        {
            rocket.gameObject.SetActive(false);
        }

        private void OnRocketDestroyed(Rocket rocket)
        {
            Destroy(rocket.gameObject);
        }

        public void InitializePool()
        {
            _pool = new ObjectPool<Rocket>(OnCreateRocket, OnRocketRetrieved, OnRocketReleased,
                OnRocketDestroyed, false, 50, 100);

            _rocketParent = GameObject.FindWithTag("RocketParent")
                .transform;
        }
    }
}