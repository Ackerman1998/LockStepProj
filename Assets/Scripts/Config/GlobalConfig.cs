using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig : MonoSingleton<GlobalConfig>
{
    public GameType gameType;
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