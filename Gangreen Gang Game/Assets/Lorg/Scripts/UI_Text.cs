using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoBehaviour
{
    private static Text speechText;

    private void Awake()
    {
        speechText = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        //speechText.text = "гавно";
        //TypeWriter.AddWriter_Static(speechText,"Hello World!", 0.1f, true);
    }

    public static void Write(string txt, float timePerChar, bool invisibleChars)
    {
        if (Services.timeManager.fastForwarding)
            TypeWriter.AddWriter_Static(UI_Text.speechText, txt, timePerChar / Services.timeManager.fastForwardSpeed, invisibleChars);
        else
        {
            TypeWriter.AddWriter_Static(UI_Text.speechText, txt, timePerChar, invisibleChars);
        }
    }
}
