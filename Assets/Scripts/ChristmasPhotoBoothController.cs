using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasPhotoBoothController : MonoBehaviour {

    public List<StepBase> StepList;

	
    void Start()
    {
        ResetWorld();
    }

    private void OnEnable()
    {
        EventManager.OnUpdateScene += EventManager_OnUpdateScene;
    }

    private void OnDisable()
    {
        EventManager.OnUpdateScene -= EventManager_OnUpdateScene;
    }

    public void ResetWorld()
    {
        GlobalVars.SceneList item = (GlobalVars.SceneList)0;
        SetCurrentStep(item);
    }

    void SetCurrentStep(GlobalVars.SceneList sceneEnum)
    {
        for (int i = 0; i < StepList.Count; i++)
        {
            StepBase curStep = StepList[i];
            if (sceneEnum == curStep.CurScene)
            {
                curStep.Show();
            }
            else
            {
                curStep.Hide();
            }
        }
        GlobalVars.Instance.CurScene = sceneEnum;
        EventManager.Instance.UpdateSceneComplete(sceneEnum);
    }


    void EventManager_OnUpdateScene(GlobalVars.SceneList sceneEnum)
    {
        SetCurrentStep(sceneEnum);
    }

}
