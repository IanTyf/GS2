using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    public static float textSpeed = 0.06f;

    public static string p1Early = "I still have some time, back to sleep";
    public static string p1OnTime = "Wakey Wakey";
    public static string p1Late = "Oh No! I'm late!";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void playSubtitles( int id )
    {
        switch (id)
        {
            case 1:
                UI_Text.Write(p1Early, textSpeed, true);
                break;
            case 2:
                UI_Text.Write(p1OnTime, textSpeed, true);
                break;
            case 3:
                UI_Text.Write(p1Late, textSpeed, true);
                break;

        }
    }
}
