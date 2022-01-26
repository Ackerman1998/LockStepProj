using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig : MonoSingleton<GlobalConfig>
{
    public GameType gameType;
    public bool recording = false;
    public string currentRecordName = "";
    public override void Awake()
    {
        base.Awake();
    }

}
public enum GameType { 
    Standalone,
    Playback,
    Net
}