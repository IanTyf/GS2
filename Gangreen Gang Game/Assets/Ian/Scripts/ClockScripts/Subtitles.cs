using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    public static float textSpeed = 0.06f;

    public static string p1Early = "Niece: *Yawn* Stupid clock, I still have some time to sleep.";
    public static string p1OnTime = "Niece: Time to get to Work.";
    public static string p1Late = "Niece: Oh shoot I'm late! I gotta get down stairs!";

    public static string p2NieceGreet = "Hi there! how can I help you?";
    public static string p2CustomerAsk = "Yes! I am looking for a clock!";

    public static string p2NotTheRightClock = "That's.. not what I want.";

    public static string p2MoreClockUpstairs = "Niece: There are also clocks upstairs.";
    public static string p2decideToBuy = "This one! I love it!";
    public static string p2failedBuy = "I think I'm just going to go with this one.";
    public static string p2NieceSayBye = "Alright sounds good. Thank you for coming!";


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
            case 4:
                UI_Text.Write(p2NieceGreet, textSpeed, true);
                break;
            case 5:
                UI_Text.Write(p2CustomerAsk, textSpeed, true);
                break;
            case 6:
                UI_Text.Write(p2NotTheRightClock, textSpeed, true);
                break;
            case 7:
                UI_Text.Write(p2MoreClockUpstairs, textSpeed, true);
                break;
            case 8:
                UI_Text.Write(p2decideToBuy, textSpeed, true);
                break;
            case 9:
                UI_Text.Write(p2failedBuy, textSpeed, true);
                break;
            case 10:
                UI_Text.Write(p2NieceSayBye, textSpeed, true);
                break;
        }
    }
}
