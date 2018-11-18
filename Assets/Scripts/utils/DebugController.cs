using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour {

    public bool IsDebug = false;
    public Text LogTxt;

    private void Awake()
    {
        if(IsDebug){
            this.gameObject.SetActive(true);
        }else{
            this.gameObject.SetActive(false);
        }

        if (LogTxt == null)
            LogTxt = this.gameObject.GetComponentInChildren<Text>();

        if (LogTxt == null)
            throw new System.Exception("A debug txt object must exist");
    }

    private void OnEnable()
    {
        if(IsDebug)
           EventManager.OnLogEvent += EventManager_OnLogEvent;
    }

    private void OnDisable()
    {
        if(IsDebug)
            EventManager.OnLogEvent -= EventManager_OnLogEvent;
    }

    void EventManager_OnLogEvent(string txtString)
    {
        if (!string.IsNullOrEmpty(txtString))
        {
            Debug.Log("txtString: " + txtString);
            LogTxt.text = txtString;
        }else{
            Debug.Log("nothing in txt string");
        }
    }
}
