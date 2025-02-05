using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    private Text message;

    private void Start()
    {
        message = GetComponent<Text>();
    }

    public void ShowMessageBox(string msg)
    {
        message.text = msg;
        message.enabled = true;
        Invoke("CloseMessageBox",1.0f);
    }

    void CloseMessageBox()
    {
        message.enabled = false;
    }
}
