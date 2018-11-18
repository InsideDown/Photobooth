using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step2TakePhotoController : StepBase {


    public GameObject Instructions;
    public List<GameObject> CountdownList;

    private int _CurCountdownItem = 0;

    private void Awake()
    {
        if (Instructions == null)
            throw new System.Exception("An Instructions object must be defined in " + this.gameObject.name);

        if (CountdownList == null)
            throw new System.Exception("A CountdownList object must be defined in " + this.gameObject.name);
    }

    void Init()
    {
        _CurCountdownItem = 0;
        Instructions.SetActive(true);
        foreach(GameObject countdownItem in CountdownList)
        {
            countdownItem.SetActive(false);
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        Init();
        base.Show();
    }

    void StartCountdown()
    {
        
    }

    public void OnTakePhotoClick()
    {
        StartCountdown();
    }
}
