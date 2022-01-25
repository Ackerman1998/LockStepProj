using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T:new()
{
    protected static T mInstance;

    protected static object mLockobj=new object();

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                lock (mLockobj)
                {
                    if (mInstance == null)
                    {
                        mInstance = new T();
                    }
                }
            }
           
            return mInstance;
        }
    }
}
         