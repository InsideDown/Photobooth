using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnImageSelection : MonoBehaviour {

    private Image ImageContainer;
    private Button ButtonContainer;

    private void Awake()
    {
        ImageContainer = this.gameObject.GetComponent<Image>();
        ButtonContainer = this.gameObject.GetComponent<Button>();

        Utils.Instance.CheckRequired(ImageContainer, "ImageContainer");
        Utils.Instance.CheckRequired(ButtonContainer, "ButtonContainer");


    }

    public void SetButton(Sprite image)
    {
        ImageContainer.sprite = image;
        ButtonContainer.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    private void OnClick()
    {
        Debug.Log("On click action called");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
