using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public GameObject ScrollContainer;
	public BtnImageSelection BtnPrefab;

    public List<Sprite> ImageList;


    private void Awake()
    {
        Utils.Instance.CheckRequired(ScrollContainer, "ScrollContainer");
        Utils.Instance.CheckRequired(BtnPrefab, "BtnPrefab");
        Utils.Instance.CheckRequired(ImageList, "ImageList");

        InitList();
    }

    private void InitList()
    {
        ClearList();
        for (int i = 0; i < ImageList.Count; i++)
        {
            BtnImageSelection btnImage = Instantiate(BtnPrefab, ScrollContainer.transform, false) as BtnImageSelection;
            Sprite curSprite = ImageList[i];
            btnImage.SetButton(curSprite);
        }
    }

    private void ClearList()
    {
        foreach (Transform child in ScrollContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
