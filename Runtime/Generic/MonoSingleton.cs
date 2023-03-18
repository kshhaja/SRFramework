using UnityEngine;


namespace SRFramework.Utility
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;
                }
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    instance = obj.AddComponent(typeof(T)) as T;
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}