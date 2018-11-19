using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CaptureImage : MonoBehaviour {

    [Tooltip("How we should be naming our screenshots")]
    public string FileName = "superscreen_";

    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int CaptureWidth = 3840;
    public int CaptureHeight = 2160;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format FileFormat = Format.JPG;

    [Tooltip("Where to store the photos inside /Documents")]
    public string DirectoryName = "christmas-screenshots";

    [Tooltip("The camera to use to render from")]
    public Camera CameraToUse;

    private string _DocumentPath;
    //keep track of how many files are currently in our directory for naming
    private int _FileCount;
    private bool _IsCapturingScreenshot = false;

    // private vars for screenshot
    private Rect Rect;
    private RenderTexture RenderTextureToUse;
    private Texture2D ScreenShot;

    private void Awake()
    {
        if (CameraToUse == null)
            throw new System.Exception("A camera must be set in order to capture images.");

        CreateDirectory();
        _FileCount = GetFileCount();
    }

    private void OnEnable()
    {
        EventManager.OnCaptureScreenshot += EventManager_OnCaptureScreenshot;
    }


    /// <summary>
    /// Creates the directory that will store where we want the photos
    /// </summary>
    void CreateDirectory()
    {
        _DocumentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //TODO: need to check on PC
        _DocumentPath += "/Documents/" + DirectoryName;
        _DocumentPath = Path.GetFullPath(_DocumentPath);
        //create directory if it does not yet exist (nothing if exists)
        System.IO.Directory.CreateDirectory(_DocumentPath);
        EventManager.Instance.LogEvent("created: " + _DocumentPath);

    }

    int GetFileCount()
    {
        string mask = string.Format("{0}{1}x{2}*.{3}", FileName, CaptureWidth, CaptureHeight, FileFormat.ToString().ToLower());
        int fileCount = Directory.GetFiles(_DocumentPath, mask, SearchOption.TopDirectoryOnly).Length;
        return fileCount;
    }

    private void Update()
    {
        if(_IsCapturingScreenshot)
        {
            _IsCapturingScreenshot = false;
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot()
    {
        // create screenshot objects if needed
        if (RenderTextureToUse == null)
        {
            // creates off-screen render texture that can rendered into
            Rect = new Rect(0, 0, CaptureWidth, CaptureHeight);
            RenderTextureToUse = new RenderTexture(CaptureWidth, CaptureHeight, 24);
            ScreenShot = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.RGB24, false);
        }
        //set our camera to render scene to
        CameraToUse.targetTexture = RenderTextureToUse;
        CameraToUse.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = RenderTextureToUse;
        ScreenShot.ReadPixels(Rect, 0, 0);

        // reset active camera texture and render texture
        CameraToUse.targetTexture = null;
        RenderTexture.active = null;

        //get a unique filename to store 
        string fileName = GetUniqueFilename();

        // pull in our file header/data bytes for the specified image format (has to be done from main thread)
        byte[] fileHeader = null;
        byte[] fileData = null;
        if (FileFormat == Format.RAW)
        {
            fileData = ScreenShot.GetRawTextureData();
        }
        else if (FileFormat == Format.PNG)
        {
            fileData = ScreenShot.EncodeToPNG();
        }
        else if (FileFormat == Format.JPG)
        {
            fileData = ScreenShot.EncodeToJPG();
        }
        else // ppm
        {
            // create a file header for ppm formatted file
            // create a file header for ppm formatted file
            string headerStr = string.Format("P6\n{0} {1}\n255\n", CaptureWidth, CaptureHeight);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            ScreenShot.wrapMode = TextureWrapMode.Clamp;

            fileData = ScreenShot.GetRawTextureData();

        }

    }

    string GetUniqueFilename()
    {
        _FileCount = GetFileCount();
        string fileName = string.Format("{0}/{1}{2}x{3}_{4}.{5}", 
                                     _DocumentPath,
                                     FileName,
                                     CaptureWidth, 
                                     CaptureHeight, 
                                     _FileCount, 
                                     FileFormat.ToString().ToLower());

        return fileName;
    }


    void SetScreenCapturing()
    {
        _IsCapturingScreenshot = true;
    }

    void EventManager_OnCaptureScreenshot()
    {
        SetScreenCapturing();
    }

}
