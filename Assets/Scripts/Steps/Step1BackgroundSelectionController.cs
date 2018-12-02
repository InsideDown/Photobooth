using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step1BackgroundSelectionController : StepBase {

    public SelectionController SelectionController;

    private void Awake()
    {
        Utils.Instance.CheckRequired(SelectionController);
    }


    private void OnEnable()
    {
        EventManager.OnScreenSelected += EventManager_OnScreenSelected;
    }

    private void OnDisable()
    {
        EventManager.OnScreenSelected -= EventManager_OnScreenSelected;
    }


    public void BtnPressed(int index)
    {
        base.GoToScene(NextScene);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();

        SelectionController.Init();
    }

    void EventManager_OnScreenSelected()
    {
        base.GoToScene(NextScene);
    }

}
