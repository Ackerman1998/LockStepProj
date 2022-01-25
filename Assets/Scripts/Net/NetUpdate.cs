using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetUpdate : MonoSingleton<NetUpdate>
{
    public override void Awake()
    {
        base.Awake();
    }
    

    // Update is called once per frame
    void Update()
    {
        NetGlobal.Instance.RunAction();
    }

    private void OnDestroy()
    {
        TcpManager.Instance.Close();
    }
}
