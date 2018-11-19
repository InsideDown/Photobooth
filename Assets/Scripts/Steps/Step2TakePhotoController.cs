using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step2TakePhotoController : StepBase {


    public GameObject Instructions;
    public GameObject BtnTakePhoto;
    public List<GameObject> CountdownList;
    public float WaitLength = 1.0f;

    private int _CurCountdownItem = 0;

    private void Awake()
    {
        if (Instructions == null)
            throw new System.Exception("An Instructions object must be defined in " + this.gameObject.name);

        if (CountdownList == null || CountdownList.Count == 0)
            throw new System.Exception("A CountdownList object must be defined in " + this.gameObject.name);
    }

    void Init()
    {
        _CurCountdownItem = 0;
        ShowInstructions();
        ShowCountdownListItem();
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

    void ShowInstructions()
    {
        Instructions.SetActive(true);
        BtnTakePhoto.SetActive(true);
    }

    void HideInstructions()
    {
        Instructions.SetActive(false);
        BtnTakePhoto.SetActive(false);
    }

    void StartCountdown()
    {
        HideInstructions();
        StartCoroutine(ShowCountdownNumber());
    }

    void ShowCountdownListItem(int index = -1)
    {
        foreach (GameObject countdownItem in CountdownList)
        {
            countdownItem.SetActive(false);
        }
        if (index > -1)
            CountdownList[index].SetActive(true);
    }

    IEnumerator ShowCountdownNumber()
    {
        ShowCountdownListItem(_CurCountdownItem);
        yield return new WaitForSeconds(WaitLength);
        _CurCountdownItem++;
        if(_CurCountdownItem < CountdownList.Count)
        {
            StartCoroutine(ShowCountdownNumber());
        }else{
            ShowCountdownListItem();
        }
    }

    public void OnTakePhotoClick()
    {
        StartCountdown();
    }
}
