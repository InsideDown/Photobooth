using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Step2TakePhotoController : StepBase {


    public GameObject Instructions;
    public GameObject BtnTakePhoto;
    public List<GameObject> CountdownList;
    public float WaitLength = 1.0f;
    public Material BackgroundMaterial;

    private int _CurCountdownItem = 0;
    private Vector3 _InstructionPos;
    private Vector3 _BtnStartPos;
    private CanvasGroup _InstructionCanvasGroup;
    private CanvasGroup _BtnStartCanvasGroup;
    private float _DelayBeforeInstructions = 1.0f;

    private void Awake()
    {
        Utils.Instance.CheckRequired(Instructions, "ScrollContainer");
        Utils.Instance.CheckRequired(CountdownList);
        Utils.Instance.CheckRequired(BackgroundMaterial);

        _InstructionPos = Instructions.transform.localPosition;
        _BtnStartPos = BtnTakePhoto.transform.localPosition;

        _InstructionCanvasGroup = Instructions.GetComponent<CanvasGroup>();
        _BtnStartCanvasGroup = BtnTakePhoto.GetComponent<CanvasGroup>();

        Instructions.SetActive(false);
        BtnTakePhoto.SetActive(false);
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
        ShowCountdownListItem();
        StartCoroutine(WaitShowInstructions());
    }

    IEnumerator WaitShowInstructions()
    {
        yield return new WaitForSeconds(_DelayBeforeInstructions);
        ShowInstructions();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        Init();
    }

    void ShowInstructions()
    {
        float tweenSpeed = 0.5f;
        float delay = 0.3f;
        float distance = 50.0f;
    
        _InstructionCanvasGroup.alpha = 0;
        _BtnStartCanvasGroup.alpha = 0;
        Instructions.transform.localPosition = new Vector3(_InstructionPos.x, _InstructionPos.y - distance, _InstructionPos.z);
        BtnTakePhoto.transform.localPosition = new Vector3(_BtnStartPos.x, _BtnStartPos.y - distance, _BtnStartPos.z);
        Instructions.SetActive(true);
        BtnTakePhoto.SetActive(true);

        _InstructionCanvasGroup.DOFade(1, tweenSpeed);
        Instructions.transform.DOLocalMoveY(_InstructionPos.y, tweenSpeed).SetEase(Ease.OutQuad);

        _BtnStartCanvasGroup.DOFade(1, tweenSpeed).SetDelay(delay);
        BtnTakePhoto.transform.DOLocalMoveY(_BtnStartPos.y, tweenSpeed).SetDelay(delay).SetEase(Ease.OutQuad);
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
