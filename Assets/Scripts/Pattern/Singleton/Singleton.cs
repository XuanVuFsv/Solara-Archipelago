using UnityEngine;

namespace VitsehLand.Scripts.Pattern.Singleton
{

    /// <summary>
    /// A generic class that implements the singleton pattern in Unity
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _reference;
        public static T Instance
        {
            get
            {
                if (!IsValidInstance())
                {
                    _reference = FindAnyObjectByType<T>();
                }
                return _reference;
            }
        }

        public static bool IsValidInstance()
        {
            return _reference != null;
        }

        public virtual void Awake()
        {
            if (IsValidInstance() && !ReferenceEquals(_reference, this))
            {
                Destroy(gameObject);
            }
            else
            {
                if (typeof(T) == typeof(Singleton<T>))
                {
                    _reference = (T)(MonoBehaviour)this;
                }
            }
        }
    }
}