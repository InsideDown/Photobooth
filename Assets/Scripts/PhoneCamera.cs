using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {


    public RawImage Background;
    public AspectRatioFitter Fit;

    private bool _CamAvailable;
    private WebCamTexture _BackCamera;
    private Texture _DefaultBackground;



    private void Start()
    {
        _DefaultBackground = Background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("no camera detected");
            _CamAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                _BackCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(_BackCamera == null)
        {
            Debug.Log("no back facing camera detected");
            return;
        }

        _BackCamera.Play();
        Background.texture = _BackCamera;

        _CamAvailable = true;
    }

    private void Update()
    {
        if (!_CamAvailable)
            return;

        float ratio = (float)_BackCamera.width / (float)_BackCamera.height;
        Fit.aspectRatio = ratio;

        float scaleY = _BackCamera.videoVerticallyMirrored ? -1f : 1f;
        Background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -_BackCamera.videoRotationAngle;
        Background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }


    //   public GameObject Cube;
    //   public Camera CameraToRecord;


    //   private ProGifManager _GifManager;


    //   private void Awake()
    //   {
    //       _GifManager = ProGifManager.Instance;

    //   }

    //   // Use this for initialization
    //   void Start () {
    //       Cube.transform.DOMoveX(4, 1).SetLoops(3, LoopType.Yoyo);
    //}

    //// Update is called once per frame
    //void Update () {

    //}

    //public void OnStartRecord()
    //{
    //    ProGifManager.Instance.StartRecord();
    //}
}
