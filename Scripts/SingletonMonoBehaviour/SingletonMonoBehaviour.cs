using UnityEngine;

namespace U1W.MoonPlant
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = (T)FindObjectOfType(typeof(T));
 
                if (_instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
 
                return _instance;
            }
        }
 
        public static T InstanceNullable
        {
            get
            {
                return _instance;
            }
        }
 
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError(typeof(T) + " is multiple created", this);
                return;
            }
 
            _instance = this as T;
        }
        
        // シーンアンロード時にDontDestroyOnLoad()してない場合これがついているオブジェクトは破棄されるので
        // instanceは一度nullになる（起動中ずっと同一のインスタンスがいすわるわけではない）
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}