using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoBehaviour
{
    private Text speechText;

    private void Awake()
    {
        speechText = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        //speechText.text = "гавно";
        TypeWriter.AddWriter_Static(speechText,"Hello World!", 0.1f, true);
    }
}
