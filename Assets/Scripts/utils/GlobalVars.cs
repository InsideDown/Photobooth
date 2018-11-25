using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : Singleton<GlobalVars>
{

    protected GlobalVars() { }

    public enum SceneList {
        Step1,
        Step2,
        Step3
    }

    public SceneList CurScene;
    public int BackgroundIndex = 0;
    public List<Texture2D> ScreenshotReviewList = new List<Texture2D>();

}
