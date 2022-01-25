using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mono单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T>: MonoBehaviour where T:MonoBehaviour
{
    protected static T mInstance;

    protected static object mLockobj = new object();

    public virtual void Awake()
    {
        if(!transform.parent) DontDestroyOnLoad(this.gameObject);
        mInstance = GetComponent<T>();
    }
    public static T Instance
    {
        get
        {
            lock (mLockobj)
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<T>();

                    if (mInstance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);

                        mInstance=obj.AddComponent<T>();

                    }
                }
                return mInstance;
            }
        }
    }
}
        