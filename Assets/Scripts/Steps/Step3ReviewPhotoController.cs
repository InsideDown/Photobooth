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

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        ReviewPhotos();
    }
}
