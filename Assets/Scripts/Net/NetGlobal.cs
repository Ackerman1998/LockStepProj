using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetGlobal : Singleton<NetGlobal>
{
    List<Action> ac_Pool = new List<Action>();
    Mutex mutex = new Mutex();
    public void AddAction(Action ac) {
        mutex.WaitOne();
        ac_Pool.Add(ac);
        mutex.ReleaseMutex();
    }
    public void RunAction() {
        mutex.WaitOne();
        for (int i=0;i<ac_Pool.Count;i++) {
            ac_Pool[i]();
        }
        ac_Pool.Clear();
        mutex.ReleaseMutex();
    }
}
