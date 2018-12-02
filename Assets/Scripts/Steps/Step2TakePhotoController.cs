﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step2TakePhotoController : StepBase {


    public GameObject Instructions;
    public GameObject BtnTakePhoto;
    public List<GameObject> CountdownList;
    public float WaitLength = 1.0f;
    public Material BackgroundMaterial;

    private int _CurCountdownItem = 0;

    private void Awake()
    {
        Utils.Instance.CheckRequired(Instructions, "ScrollContainer");
        Utils.Instance.CheckRequired(CountdownList);
        Utils.Instance.CheckRequired(BackgroundMaterial);
    }

    void OnEnable()
    {
        EventManager.OnCaptureScreenshotComplete += EventManager_OnCaptureScreenshotComplete;    
    }

    void onDisable()
    {
        EventManager.OnCaptureScreenshotComplete -= EventManager_OnCaptureScreenshotComplete;  
    }

    void Init()
    {
        //replace our background 
        BackgroundMaterial.mainTexture = GlobalVars.Instance.BackgroundTexture;
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
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        //hide our list
        ShowCountdownListItem();
        EventManager.Instance.CaptureScreenshotStart();
    }

    void InitTakePhoto()
    {
        StartCountdown();
    }

    public void OnTakePhotoClick()
    {
        InitTakePhoto();
    }

    void EventManager_OnCaptureScreenshotComplete()
    {
        base.GoToScene(NextScene);
    }

}
