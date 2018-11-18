using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoboothController : MonoBehaviour {


    public List<GameObject> StepList;
    public List<BackgroundTexture> BackgroundTextures;
    public Camera GreenScreenCamera;


    private int _CurStepIndex = 0;
    private int _BackgroundIndex = 0;


    void Awake()
    {
        ResetScene();
    }


    void ResetScene()
    {
        _CurStepIndex = 0;
        _BackgroundIndex = 0;
        //SetActiveStep(_CurStepIndex);
    }

    void SetActiveStep(int stepIndex = 0)
    {
        _CurStepIndex = stepIndex;
        for (int i = 0; i < StepList.Count;i++) {
            GameObject curStep = StepList[i];
            curStep.SetActive(false);
        }
    }



    public void TakePhoto()
    {
        Debug.Log("take photo called");
        //Texture2D cameraImage = RTImage(GreenScreenCamera);
        //ScreenCapture.CaptureScreenshot(ScreenshotName(), 2);
    }

    void TakeHighRes(Camera camera)
    {
        int resWidth = 1920;
        int resHeight = 1080;
        RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
    }

    string ScreenshotName()
    {
        return string.Format("{0}/screenshots/hamburgerscreen_{1}.png",
                             Application.persistentDataPath,
                             System.DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss"));
    }

    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RTImage(Camera camera)
    {
        int resWidth = camera.targetTexture.width;
        int resHeight = camera.targetTexture.height;
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(resWidth, resHeight);
        image.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToJPG();
        //string filename = ScreenshotName();

        // Replace the original active Render Texture.
        //RenderTexture.active = currentRT;
        return image;
    }


}
