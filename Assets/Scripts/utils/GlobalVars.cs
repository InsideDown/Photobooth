using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : Singleton<GlobalVars>
{

    protected GlobalVars() { }

    public enum SceneList {
        Step1,
        Step2,
        Step3,
        Step4
    }

    public SceneList CurScene;
    public int BackgroundIndex = 0;
    //hold a reference to the background texture we should be using
    public Texture BackgroundTexture;

}
