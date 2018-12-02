using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour {

    public GameObject ScrollContainer;
	public BtnImageSelection BtnPrefab;
    public ScrollRect ScrollRect;

    [Serializable]
    public struct ImagesList
    {
        public Sprite ImageSprite;
        public Texture LargeTexture;
    }

    public List<ImagesList> ImageSelectionList;


    private void Awake()
    {
        Utils.Instance.CheckRequired(ScrollContainer, "ScrollContainer");
        Utils.Instance.CheckRequired(BtnPrefab, "BtnPrefab");
        Utils.Instance.CheckRequired(ImageSelectionList, "ImageSelectionList");

    }

    public void Init()
    {
        ScrollRect.horizontalNormalizedPosition = 0.5f;
        InitList();
    }

    private void InitList()
    {
        ClearList();
        for (int i = 0; i < ImageSelectionList.Count; i++)
        {
            BtnImageSelection btnImage = Instantiate(BtnPrefab, ScrollContainer.transform, false) as BtnImageSelection;
            Sprite curSprite = ImageSelectionList[i].ImageSprite;
            Texture curTexture = ImageSelectionList[i].LargeTexture;
            btnImage.SetButton(curSprite, curTexture);
        }
    }

    private void ClearList()
    {
        foreach (Transform child in ScrollContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
