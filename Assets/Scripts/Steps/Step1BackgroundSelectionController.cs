using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step1BackgroundSelectionController : StepBase {


    public void BtnPressed(int index)
    {
        GlobalVars.Instance.BackgroundIndex = index;
        base.SetScene();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }
}
