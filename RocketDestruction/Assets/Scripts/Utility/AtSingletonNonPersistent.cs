using UnityEngine;

namespace RocketDestruction.Utility
{
    public class AtSingletonNonPersistent<T> : MonoBehaviour where T : Component
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;
                GameObject typeObject = new()
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                s_instance = typeObject.AddComponent<T>();

                return s_instance;
            }
        }
    }
}