using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoReview : MonoBehaviour {
    
    public Material MaterialToDuplicate;

    private Material _PhotoMaterial;
    private Renderer _Renderer;

    private void Awake()
    {

        if (MaterialToDuplicate == null)
            throw new System.Exception("A DuplicateMaterial must be set in PhotoReview");

        _Renderer = this.gameObject.GetComponent<Renderer>();
        _PhotoMaterial = new Material(MaterialToDuplicate);
        _Renderer.material = new Material(_PhotoMaterial);
    }

    public void SetImage(Texture2D photoTexture)
    {
        _Renderer.material.mainTexture = photoTexture;
    }


}
