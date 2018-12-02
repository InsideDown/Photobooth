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

    public List<Texture2D> PhotoTextures = new List<Texture2D>();

    private string _DocumentPath;
    //keep track of how many files are currently in our directory for naming
    private int _FileCount;
    private bool _IsCapturingScreenshot = false;
    private bool _IsSavingScreenshot = false;
    private string _LatestFileName = null;

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
        EventManager.OnCaptureScreenshotStart += EventManager_OnCaptureScreenshotStart;
        EventManager.OnSaveScreenshotStart += EventManager_OnSaveScreenshotStart;
    }

    private void OnDisable()
    {
        EventManager.OnCaptureScreenshotStart -= EventManager_OnCaptureScreenshotStart;

    }


    /// <summary>
    /// Creates the directory that will store where we want the photos
    /// </summary>
    void CreateDirectory()
    {
        _DocumentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //TODO: need to check on PC - this doubles documents
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            _DocumentPath += "/" + DirectoryName;
        }else{
            //we're probably a mac in player or editor
            _DocumentPath += "/Documents/" + DirectoryName;
        }
        _DocumentPath = Path.GetFullPath(_DocumentPath);
        //create directory if it does not yet exist (nothing if exists)
        System.IO.Directory.CreateDirectory(_DocumentPath);
        EventManager.Instance.LogEvent("created: " + _DocumentPath);

    }

    /// <summary>
    /// Get the number of files in our directory
    /// </summary>
    /// <returns>The file count.</returns>
    int GetFileCount()
    {
        string mask = string.Format("{0}{1}x{2}*.{3}", FileName, CaptureWidth, CaptureHeight, FileFormat.ToString().ToLower());
        int fileCount = Directory.GetFiles(_DocumentPath, mask, SearchOption.TopDirectoryOnly).Length;
        return fileCount;
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(_LatestFileName))
        {
            EventManager.Instance.LogEvent("wrote: " + _LatestFileName);
            _LatestFileName = null;
            //as of right now, we're done capturing here
            EventManager.Instance.SaveScreenshotComplete();
        }

        if(_IsSavingScreenshot)
        {
            _IsSavingScreenshot = false;
            SaveScreenshot();
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

        //create a new texture to add to our list
        Texture2D screenshotCopy = new Texture2D(ScreenShot.width, ScreenShot.height);
        Color[] colors = ScreenShot.GetPixels(0, 0, ScreenShot.width, ScreenShot.height);
        screenshotCopy.SetPixels(colors);
        screenshotCopy.Apply();
        PhotoTextures.Add(screenshotCopy);

        // reset active camera texture and render texture
        CameraToUse.targetTexture = null;
        RenderTexture.active = null;
        EventManager.Instance.CaptureScreenshotComplete();
    }


    void SaveScreenshot()
    {
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
            string headerStr = string.Format("P6\n{0} {1}\n255\n", CaptureWidth, CaptureHeight);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            ScreenShot.wrapMode = TextureWrapMode.Clamp;

            fileData = ScreenShot.GetRawTextureData();
        }

        // create new thread to save the image to file (only operation that can be done in background)
        new System.Threading.Thread(() =>
        {
            // create file and write optional header with image bytes
            var f = System.IO.File.Create(fileName);
            if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);

            f.Write(fileData, 0, fileData.Length);
            f.Close();
            _LatestFileName = fileName;

            Debug.Log(string.Format("Wrote screenshot {0} of size {1}", fileName, fileData.Length));
        }).Start();
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


    /// <summary>
    /// Capture screens with a slight delay to allow other events time to happen
    /// </summary>
    /// <returns>The screen capturing.</returns>
    IEnumerator SetScreenCapturing()
    {
        yield return new WaitForSeconds(0.1f);
        CaptureScreenshot();
        //_IsCapturingScreenshot = true;
    }

    IEnumerator SetSaveScreenshot()
    {
        yield return new WaitForEndOfFrame();
        _IsSavingScreenshot = true;
    }

    void EventManager_OnCaptureScreenshotStart()
    {
        PhotoTextures.Clear();
        StartCoroutine(SetScreenCapturing());
    }

    void EventManager_OnSaveScreenshotStart()
    {
        StartCoroutine(SetSaveScreenshot());
    }

}
