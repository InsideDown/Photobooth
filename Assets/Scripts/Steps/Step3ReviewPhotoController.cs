using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Step3ReviewPhotoController : StepBase {


    public GameObject BtnSavePhoto;
    public GameObject BtnRetakePhoto;
    public GameObject TxtPhotoSaved;
    public GameObject PhotoHolder;
    public PhotoReview PhotoReviewController;

    private CaptureImage _CaptureImageController;
    private CanvasGroup _BtnSaveCanvasGroup;
    private CanvasGroup _BtnRetakeCanvasGroup;

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
        //TODO: Messaging about screenshot being saved should display here
        EventManager.Instance.SaveScreenshotStart();
    }

    void AnimIn()
    {
        float tweenSpeed = 0.4f;
        _BtnSaveCanvasGroup.DOFade(1, tweenSpeed);
        _BtnRetakeCanvasGroup.DOFade(1, tweenSpeed).SetDelay(0.4f);

    }

    public void OnSavePhoto()
    {
        Debug.Log("saving photo");
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
    }

}
