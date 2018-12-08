using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Step3ReviewPhotoController : StepBase {


    public GameObject BtnSavePhoto;
    public GameObject BtnRetakePhoto;
    public GameObject TxtPhotoSaved;
    public GameObject PhotoHolder;
    public PhotoReview PhotoReviewController;
    public Text TxtInfo;

    private CaptureImage _CaptureImageController;
    private CanvasGroup _BtnSaveCanvasGroup;
    private CanvasGroup _BtnRetakeCanvasGroup;
    private float _DelayHomeTime = 2.0f;
    private Dictionary<string, string> _StrMessages = new Dictionary<string, string>
        {
            { "saving", "Saving photo, please wait..." },
            { "error", "Oops, there was an error - returning you home..." },
            { "complete", "Photo saved! Returning you to home..." }
        };

    private void Awake()
    {
        _CaptureImageController = FindObjectOfType<CaptureImage>();
        _BtnSaveCanvasGroup = BtnSavePhoto.GetComponent<CanvasGroup>();
        _BtnRetakeCanvasGroup = BtnRetakePhoto.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EventManager.OnSaveScreenshotComplete += EventManager_OnSaveScreenshotComplete;
    }

    private void OnDisable()
    {
        EventManager.OnSaveScreenshotComplete -= EventManager_OnSaveScreenshotComplete;
    }

    /// <summary>
    /// Loop through our photo array to review photos to decide if we want to keep them
    /// </summary>
    void ReviewPhotos()
    {
        ClearOldPhotos();
        for (int i = 0; i < _CaptureImageController.PhotoTextures.Count;i++)
        {
            Texture2D textureToReview = _CaptureImageController.PhotoTextures[i];
            PhotoReview photoReview = Instantiate(PhotoReviewController, PhotoHolder.transform, false) as PhotoReview;
            photoReview.SetImage(textureToReview);
        }
    }

    void ClearOldPhotos()
    {
        foreach (Transform child in PhotoHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void SavePhoto()
    {
        //hide our buttons and show messaging
        HideUI();
        EventManager.Instance.SaveScreenshotStart();
    }

    void HideUI() 
    {
        BtnSavePhoto.SetActive(false);
        BtnRetakePhoto.SetActive(false);
    }

    void AnimIn()
    {
        float tweenSpeed = 0.4f;
        _BtnSaveCanvasGroup.DOFade(1, tweenSpeed);
        _BtnRetakeCanvasGroup.DOFade(1, tweenSpeed).SetDelay(0.4f);

    }

    /// <summary>
    /// Sets the text of our messaging
    /// </summary>
    /// <param name="key">Key. - must match a key value from the dictionary</param>
    void SetText(string key)
    {
        string msg = _StrMessages[key];
        TxtInfo.text = msg;
        TxtInfo.gameObject.SetActive(true);
    }

    public void OnSavePhoto()
    {
        Debug.Log("saving photo");
        SetText("saving");
        SavePhoto();
    }

    public void OnRetakePhoto()
    {
        Debug.Log("retaking photo");

        base.GoToScene(PrevScene);
    }

    public override void Hide()
    {
        _BtnSaveCanvasGroup.alpha = 0;
        _BtnSaveCanvasGroup.alpha = 0;
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        AnimIn();
        ReviewPhotos();
    }

    void EventManager_OnSaveScreenshotComplete()
    {
        Debug.Log("screenshot is done saving");
        SetText("complete");
        StartCoroutine(SendHome());
    }

    IEnumerator SendHome()
    {
        yield return new WaitForSeconds(_DelayHomeTime);
        Debug.Log("go home");
        base.GoToScene(NextScene);
    }

}
