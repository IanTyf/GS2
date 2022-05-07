using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextDup : MonoBehaviour
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
        TypeWriterDup.AddWriter_Static(UI_TextDup.speechText, txt, timePerChar, invisibleChars);
    }

    public static void Write(string txt, float timePerChar, bool invisibleChars, bool notDestroy)
    {
        TypeWriterDup.AddWriter_Static(UI_TextDup.speechText, txt, timePerChar, invisibleChars, notDestroy);
    }
}
