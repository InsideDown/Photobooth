using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step3ReviewPhotoController : StepBase {

    public GameObject PhotoHolder;
    public PhotoReview PhotoReviewController;

    private CaptureImage _CaptureImageController;

    private void Awake()
    {
        _CaptureImageController = FindObjectOfType<CaptureImage>();
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
        
        for (int i = 0; i < _CaptureImageController.PhotoTextures.Count;i++)
        {
            Texture2D textureToReview = _CaptureImageController.PhotoTextures[i];
            PhotoReview photoReview = Instantiate(PhotoReviewController, PhotoHolder.transform, false) as PhotoReview;
            photoReview.SetImage(textureToReview);
        }
    }

    void SavePhoto()
    {
        //TODO: Messaging about screenshot being saved should display here
        EventManager.Instance.SaveScreenshotStart();
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
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        ReviewPhotos();
    }

    void EventManager_OnSaveScreenshotComplete()
    {
        Debug.Log("screenshot is done saving");
    }

}
