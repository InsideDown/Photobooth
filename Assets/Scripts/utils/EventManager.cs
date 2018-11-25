using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager> {

    protected EventManager() {}

    public delegate void StringMessageAction(string txtString);
    public static event StringMessageAction OnLogEvent;

    public delegate void SceneAction(int index);
    public static event SceneAction OnSetScene;

    public delegate void SceneUpdateAction(GlobalVars.SceneList sceneEnum);
    public static event SceneUpdateAction OnUpdateScene;
    public static event SceneUpdateAction OnUpdateSceneComplete;

    public delegate void ScreenCaptureAction();
    public static event ScreenCaptureAction OnCaptureScreenshotStart;
    public static event ScreenCaptureAction OnCaptureScreenshotComplete;

    public void SetScene(int index)
    {
        if (OnSetScene != null)
            OnSetScene(index);
    }

    public void UpdateScene(GlobalVars.SceneList sceneEnum)
    {
        if (OnUpdateScene != null)
            OnUpdateScene(sceneEnum);
    }

    public void UpdateSceneComplete(GlobalVars.SceneList sceneEnum)
    {
        if (OnUpdateSceneComplete != null)
            OnUpdateSceneComplete(sceneEnum);
    }

    public void CaptureScreenshotStart()
    {
        if (OnCaptureScreenshotStart != null)
            OnCaptureScreenshotStart();
    }

    public void CaptureScreenshotComplete()
    {
        if (OnCaptureScreenshotComplete != null)
            OnCaptureScreenshotComplete();
    }

    public void LogEvent(string txtString)
    {
        if (OnLogEvent != null)
            OnLogEvent(txtString);
    }
}
