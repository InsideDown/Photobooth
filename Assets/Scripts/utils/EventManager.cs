using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager> {

    protected EventManager() {}

    public delegate void VideoAction();
    public static event VideoAction OnVideoPlayEvent;
    public static event VideoAction OnVideoEndEvent;
    public static event VideoAction OnSauronVideoStartEvent;
    public static event VideoAction OnSauronVideoEndEvent;

    //public delegate void VideoStartAction(VideoItemModel curVideoItem);
    //public static event VideoStartAction OnVideoStartEvent;
    //public static event VideoStartAction OnRingPlacedEvent;

    public delegate void KeyboardAction();
    public static event KeyboardAction OnRedTeamWin;
    public static event KeyboardAction OnGreenTeamWin;

    public delegate void StringMessageAction(string txtString);
    public static event StringMessageAction OnLogEvent;

    public delegate void SceneAction(int index);
    public static event SceneAction OnSetScene;

    public delegate void SceneUpdateAction(GlobalVars.SceneList sceneEnum);
    public static event SceneUpdateAction OnUpdateScene;
    public static event SceneUpdateAction OnUpdateSceneComplete;

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

    public void LogEvent(string txtString)
    {
        if (OnLogEvent != null)
            OnLogEvent(txtString);
    }

    public void VideoPlayEvent()
    {
        if (OnVideoPlayEvent != null)
            OnVideoPlayEvent();
    }

    public void RedTeamWin()
    {
        if (OnRedTeamWin != null)
            OnRedTeamWin();
    }

    public void GreenTeamWin()
    {
        if (OnGreenTeamWin != null)
            OnGreenTeamWin();
    }

    //public void VideoStartEvent(VideoItemModel curVideoItem)
    //{
    //    if (OnVideoStartEvent != null)
    //        OnVideoStartEvent(curVideoItem);
    //}

    //public void RingPlacedEvent(VideoItemModel curVideoItem)
    //{
    //    if (OnRingPlacedEvent != null)
    //        OnRingPlacedEvent(curVideoItem);
    //}

    public void VideoEndEvent()
    {
        if (OnVideoEndEvent != null)
            OnVideoEndEvent();
    }

    public void SauronVideoStart() 
    {
        if (OnSauronVideoStartEvent != null)
            OnSauronVideoStartEvent();    
    }

    public void SauronVideoEnd()
    {
        if (OnSauronVideoEndEvent != null)
            OnSauronVideoEndEvent();
    }
}
