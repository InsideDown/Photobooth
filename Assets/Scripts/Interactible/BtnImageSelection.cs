using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BtnImageSelection : MonoBehaviour {


    public Image ImageContainer;

    private Button ButtonContainer;
    private Texture _Texture;
    private CanvasGroup _CanvasGroup;

    private void Awake()
    {
        ButtonContainer = this.gameObject.GetComponent<Button>();
        _CanvasGroup = this.gameObject.GetComponent<CanvasGroup>();

        Utils.Instance.CheckRequired(ImageContainer, "ImageContainer");
        Utils.Instance.CheckRequired(ButtonContainer, "ButtonContainer");
        Utils.Instance.CheckRequired(_CanvasGroup, "CanvasGroup");

        _CanvasGroup.alpha = 0;

    }

    public void SetButton(Sprite image, Texture texture)
    {
        ImageContainer.sprite = image;
        _Texture = texture;
        float ranDelay = Random.Range(0f, 0.6f);
        _CanvasGroup.DOFade(1, 0.5f).SetDelay(ranDelay);

        ButtonContainer.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    private void OnClick()
    {
        //set our global texture to our texture and dispatch our selection event
        GlobalVars.Instance.BackgroundTexture = _Texture;
        EventManager.Instance.ScreenSelected();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
