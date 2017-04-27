using UnityEngine;

namespace UnitedSolution
{
    public class SingletonBehaviour<T> : LazyBehaviour where T : LazyBehaviour
    {
        //just a singleton 
        private static T instance;
        /// <summary>
        /// Public property to get the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    GameObject container = new GameObject(typeof(T).ToString());
                    instance = container.AddComponent<T>();
                    DontDestroyOnLoad(container);
                }
                return instance;
            }
        }
        /// <summary>
        /// Initialize Singleton, destroy any second instance 
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = GetComponent<T>(); // the same as instance = this;
            }
            else
            {
                DestroyImmediate(this);
                return;
            }
        }
    }
}
