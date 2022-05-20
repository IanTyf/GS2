using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    public static float textSpeed = 0.06f;

    public static string p1Early = "Claire: *Yawn* Stupid clock, I still have some time to sleep.";
    public static string p1OnTime = "Claire: Time to get to work.";
    public static string p1Late = "Claire: Oh shoot I'm late! I gotta get down stairs!";

    public static string p2NieceGreet = "Claire: Hi there! how can I help you?";
    public static string p2CustomerAsk = "Customer: Yes! I am looking for a clock!";

    public static string p2NotTheRightClock = "Customer: Hmm, this one is not bad.";

    public static string p2MoreClockUpstairs = "Claire: There are also clocks upstairs.";
    public static string p2decideToBuy = "Customer: This one! I love it!";
    public static string p2failedBuy = "Customer: I think I'm just going to go with this one.";
    public static string p2NieceSayBye = "Claire: Alright sounds good. Thank you for coming!";

    public static string p3Beau1 = "???: Hello Cousin.";
    public static string p3Niece1 = "Claire: Oh! Beauregard. It's been a while.";
    public static string p3Niece2 = "Claire: How have you been?";
    public static string p3Beau2 = "Beauregard: Good.";
    public static string p3Beau3 = "Beauregard: I won't be here too long, I have a meeting to get to.";
    public static string p3Beau4 = "Beauregard: I see you've livened up this relic.";
    public static string p3Niece3 = "Claire: Well Uncle Frank poured his heart and soul into this place.";
    public static string p3Niece4 = "Claire: Couldn't just let it die.";
    public static string p3Beau5 = "Beauregard: That's well and good, but Uncle Franklin is from my side of the family.";
    public static string p3Beau6 = "Beauregard: As such, ownership of the store should rightfully fall to me.";
    public static string p3Beau7 = "Beauregard: I've got plans to sell the store. My meeting is to secure buyers.";
    public static string p3Niece5 = "Claire: You didn't want the store after Uncle Franklin died.";
    public static string p3Beau8 = "Beauregard: There wasn't money to be made in the store after he died. No one wanted a crusty, old shop.";
    public static string p3Beau9 = "Beauregard: And now the value of the property has gone up recently.";
    public static string p3Niece6 = "Claire: You're sick. Doesn't matter though, The store belongs to me.";
    public static string p3Beau10 = "Beauregard: Don't worry I'll make sure you get an offer you can't refuse.";

    public static string p3Beau11 = "Beauregard: Oh! Looks like I have some time before my meeting. You'll have to excuse me.";
    public static string p3Beau12 = "Beauregard: You'll have to excuse me."; // not used
    
    public static string p3Beau13 = "Beauregard: Well, it's been swell I'm going out to my meeting.";

    public static string p3Beau15 = "Beauregard: Hmm, this can't be right. My watch seems to be broken.";
    public static string p3Beau16 = "Beauregard: Anyways, it's been swell I'm going out to my meeting.";

    public static string p3Beau14 = "Beauregard: I'll see you with the buyers next time.";

    public static string p3NieceSelf1 = "(I don't know what to do uncle.)";
    public static string p3NieceSelf2 = "(You worked your butt off for this place.)";
    public static string p3NieceSelf3 = "(I can't believe Beauregard would just assume that I would throw all this away.)";
    public static string p3NieceSelf4 = "(But selling the shop would mean I could start a newer shop.)";
    public static string p3NieceSelf5 = "(If I did that I feel like I could throw some new ideas into the mix.)";
    public static string p3NieceSelf6 = "(Ugh such a big decision.)";

    public static string p4NieceHi = "Claire: Hello?";
    public static string p4BeauBackWithBuyers = "Beauregard: I'll make this quick, I'm coming back with buyers around two.";
    public static string p4NieceBye = "Claire: Oh Alright, guess I'll see you soon.";
    public static string p4NieceExciting = "Claire: I guess I'm doing this. This is oddly exciting.";

    public static string p4Beau1 = "Beauregard: I hope you're happy.";
    public static string p4Niece1 = "Claire: Come again?";
    public static string p4Beau2 = "Beauregard: The time on my watch desynced. I was late to my meetings.";
    public static string p4Beau3 = "Beauregard: Probably from all the broken clocks in there.";
    public static string p4Beau4 = "Beauregard: I lost buyers because of you.";
    public static string p4Beau5 = "Beauregard: Anyway, I'll be back down with them soon.";

    public static string p4Buyer11 = "Buyer 1: I must say Mr. Clockenspiel, this lot has potential.";
    public static string p4Buyer21 = "Buyer 2: I agree. Lots of light and location is great.";
    public static string p4Buyer22 = "Buyer 2: All we need is to clear up all these old clocks and it's perfect.";

    public static string p4Buyer12 = "Buyer 1: My papa used to have a clock like this.";
    public static string p4Buyer23 = "Buyer 2: This is the type of clock I had in my room.";

    public static string p4Buyer13 = "Buyer 1: Mr. Clockenspiel, we all feel that it wouldn't be right tearing this place down.";
    public static string p4BeauSad = "Beauregard: But gentleman! Think of the potential!";
    public static string p4Buyer14 = "Buyer 1: Our mind is decided, Good day Mr. Clockenspiel.";
    
    public static string p4Buyer15 = "Buyer 1: Mr. Clockenspiel, this place definitely has what we're looking for. When do we sign?";
    public static string p4BeauHappy = "Beauregard: Absolutely, gentlemen! We can do that right now.";
    public static string p4NieceEnd = "Claire: Alright, Lets hear your price.";

    public static string p2NieceAdded = "Claire: Right, well feel free to take a look around. There are also clocks upstairs!";

    public static string grandfatherClockReaction = "Customer: This is a bit too big. Do you have something smaller?";
    public static string mantelClockReaction = "Customer: I think this is too fancy for me.";
    public static string cornerClockReaction = "Customer: I'm not really liking the angles. Do you have something more round?";
    public static string deskClockReaction = "Customer: I think I want something with more portability.";
    public static string triangleClockReaction = "Customer: I think this is too fancy for me.";

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
            case 11:
                UI_Text.Write(p3Beau1, textSpeed, true);
                break;
            case 12:
                UI_Text.Write(p3Niece1, textSpeed, true);
                break;
            case 13:
                UI_Text.Write(p3Niece2, textSpeed, true);
                break;
            case 14:
                UI_Text.Write(p3Beau2, textSpeed, true);
                break;
            case 15:
                UI_Text.Write(p3Beau3, textSpeed, true);
                break;
            case 16:
                UI_Text.Write(p3Beau4, textSpeed, true);
                break;
            case 17:
                UI_Text.Write(p3Niece3, textSpeed, true);
                break;
            case 18:
                UI_Text.Write(p3Niece4, textSpeed, true);
                break;
            case 19:
                UI_Text.Write(p3Beau5, textSpeed, true);
                break;
            case 20:
                UI_Text.Write(p3Beau6, textSpeed, true);
                break;
            case 21:
                UI_Text.Write(p3Beau7, textSpeed, true);
                break;
            case 22:
                UI_Text.Write(p3Niece5, textSpeed, true);
                break;
            case 23:
                UI_Text.Write(p3Beau8, textSpeed, true);
                break;
            case 24:
                UI_Text.Write(p3Beau9, textSpeed, true);
                break;
            case 25:
                UI_Text.Write(p3Niece6, textSpeed, true);
                break;
            case 26:
                UI_Text.Write(p3Beau10, textSpeed, true);
                break;
            case 27:
                UI_Text.Write(p3Beau11, textSpeed, true);
                break;
            case 28:
                UI_Text.Write(p3Beau12, textSpeed, true);
                break;
            case 29:
                UI_Text.Write(p3Beau13, textSpeed, true);
                break;
            case 30:
                UI_Text.Write(p3Beau14, textSpeed, true);
                break;
            case 31:
                UI_Text.Write(p3Beau15, textSpeed, true);
                break;
            case 32:
                UI_Text.Write(p3Beau16, textSpeed, true);
                break;
            case 33:
                UI_Text.Write(p3NieceSelf1, textSpeed, true);
                break;
            case 34:
                UI_Text.Write(p3NieceSelf2, textSpeed, true);
                break;
            case 35:
                UI_Text.Write(p3NieceSelf3, textSpeed, true);
                break;
            case 36:
                UI_Text.Write(p3NieceSelf4, textSpeed, true);
                break;
            case 37:
                UI_Text.Write(p3NieceSelf5, textSpeed, true);
                break;
            case 38:
                UI_Text.Write(p3NieceSelf6, textSpeed, true);
                break;
            case 39:
                UI_Text.Write(p4NieceHi, textSpeed, true);
                break;
            case 40:
                UI_Text.Write(p4BeauBackWithBuyers, textSpeed, true);
                break;
            case 41:
                UI_Text.Write(p4NieceBye, textSpeed, true);
                break;
            case 42:
                UI_Text.Write(p4NieceExciting, textSpeed, true);
                break;
            case 43:
                UI_Text.Write(p4Beau1, textSpeed, true);
                break;
            case 44:
                UI_Text.Write(p4Niece1, textSpeed, true);
                break;
            case 45:
                UI_Text.Write(p4Beau2, textSpeed, true);
                break;
            case 46:
                UI_Text.Write(p4Beau3, textSpeed, true);
                break;
            case 47:
                UI_Text.Write(p4Beau4, textSpeed, true);
                break;
            case 48:
                UI_Text.Write(p4Beau5, textSpeed, true);
                break;
            case 49:
                UI_Text.Write(p4Buyer11, textSpeed, true);
                break;
            case 50:
                UI_Text.Write(p4Buyer21, textSpeed, true);
                break;
            case 51:
                UI_Text.Write(p4Buyer22, textSpeed, true);
                break;
            case 52:
                UI_Text.Write(p4Buyer12, textSpeed, true);
                break;
            case 53:
                UI_Text.Write(p4Buyer23, textSpeed, true);
                break;
            case 54:
                UI_Text.Write(p4Buyer13, textSpeed, true);
                break;
            case 55:
                UI_Text.Write(p4BeauSad, textSpeed, true);
                break;
            case 56:
                UI_Text.Write(p4Buyer14, textSpeed, true);
                break;
            case 57:
                UI_Text.Write(p4Buyer15, textSpeed, true);
                break;
            case 58:
                UI_Text.Write(p4BeauHappy, textSpeed, true);
                break;
            case 59:
                UI_Text.Write(p4NieceEnd, textSpeed, true);
                break;
            case 60:
                UI_Text.Write(p2NieceAdded, textSpeed, true);
                break;
            case 61:
                UI_Text.Write(grandfatherClockReaction, textSpeed, true);
                break;
            case 62:
                UI_Text.Write(mantelClockReaction, textSpeed, true);
                break;
            case 63:
                UI_Text.Write(cornerClockReaction, textSpeed, true);
                break;
            case 64:
                UI_Text.Write(deskClockReaction, textSpeed, true);
                break;
            case 65:
                UI_Text.Write(triangleClockReaction, textSpeed, true);
                break;
        }
    }
}
