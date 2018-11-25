using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBase : MonoBehaviour {

    public GlobalVars.SceneList CurScene;
    public GlobalVars.SceneList NextScene;

    private ChristmasPhotoBoothController _ChristmasPhotoBoothController;

    private void Awake()
    {
        _ChristmasPhotoBoothController = FindObjectOfType<ChristmasPhotoBoothController>();
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void GoToScene(GlobalVars.SceneList scene)
    {
        EventManager.Instance.UpdateScene(scene);
    }

    //public void SetScene()
    //{
    //    EventManager.Instance.UpdateScene(NextScene);
    //    //EventManager.Instance.SetScene((int)NextScene);
    //}
}
