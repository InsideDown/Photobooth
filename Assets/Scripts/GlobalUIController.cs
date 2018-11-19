using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIController : MonoBehaviour {


    [Serializable]
    public struct VisibleItem
    {
        public GameObject Item;
        public List<GlobalVars.SceneList> Visible;
    }

    public VisibleItem[] VisibleItems;

    private void OnEnable()
    {
        EventManager.OnUpdateSceneComplete += EventManager_OnUpdateSceneComplete;
    }

    private void OnDisable()
    {
        EventManager.OnUpdateSceneComplete -= EventManager_OnUpdateSceneComplete;
    }

    void EventManager_OnUpdateSceneComplete(GlobalVars.SceneList sceneEnum)
    {
        SetItemVisibility(sceneEnum);
    }

    void SetItemVisibility(GlobalVars.SceneList sceneEnum)
    {
        for (int i = 0; i < VisibleItems.Length; i++)
        {
            GameObject curObject = VisibleItems[i].Item;
            curObject.SetActive(false);
            //loop through our visibility array - if our enum exists, we should show
            for (int j = 0; j < VisibleItems[i].Visible.Count;j++)
            {
                GlobalVars.SceneList curEnum = VisibleItems[i].Visible[j];
                if(curEnum == sceneEnum)
                {
                    curObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void OnResetClick(int sceneInt)
    {
        GlobalVars.SceneList sceneEnum = (GlobalVars.SceneList)sceneInt;
        EventManager.Instance.UpdateScene(sceneEnum);
    }

}
