using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CondState { NotSet, Ready, Greyed };

public class ActionConditionsManager : MonoBehaviour
{
    public Clock beauWatch;

    private TimeManager tm;
    private ClockManager cm;

    [HideInInspector]
    public bool isRinging;

    // all the action conditions in the game
    private List<ActionCondition> allActionConditions = new List<ActionCondition>();

    // puzzle #1
    public ActionCondition P1goToAlarmClock;
    public ActionCondition P1ringAlarmClockEarly;
    public ActionCondition P1ringAlarmClockOnTime;
    public ActionCondition P1ringAlarmClockLate;
    public ActionCondition P1wakeUpNaturally;

    // puzzle #2
    public ActionCondition P2customerWalksIn;
    public ActionCondition P2customerAskForClock;
    public ActionCondition P2nieceReady;
    public ActionCondition P2customerReady;
    public ActionCondition P2nieceGreeted;
    public ActionCondition P2customerAsked;

    public ActionCondition P2wrongClock;
    public ActionCondition P2ringClocksUpstair;
    
    public ActionCondition P2failed;
    public ActionCondition P2customerDecided;
    public ActionCondition P2customerLeave;

    public ActionCondition P2soldRightClock;
    public ActionCondition P2soldWrongClock;

    public ActionCondition P2grandfatherClock;
    public ActionCondition P2mantelClock;
    public ActionCondition P2cornerClock;
    public ActionCondition P2deskClock;
    public ActionCondition P2triangleClock;

    // puzzle #3
    public ActionCondition P3beauWalksIn;
    public ActionCondition P3beauStartTalking;
    public ActionCondition P3NieceStartTalking;
    public ActionCondition P3beauCheckWatchNormal;
    public ActionCondition P3beauCheckWatchLate;
    public ActionCondition P3beauCheckWatchWrong;
    public ActionCondition P3beauLeave;
    public ActionCondition P3nieceSelfTalk;

    // puzzle #4
    public ActionCondition P4beauCallNormal;
    public ActionCondition P4beauCallMad;

    public ActionCondition P4beauWalksIn;
    public ActionCondition P4BuyerOneWalksIn;
    public ActionCondition P4BuyerTwoWalksIn;

    public ActionCondition P4BuyerOneFinished;
    public ActionCondition P4BuyerTwoFinished;
    public ActionCondition P4BeauGreets;

    public ActionCondition P4BeauFinishes;
    public ActionCondition P4BuyerOneWants;
    public ActionCondition P4BuyerTwoWants;

    public ActionCondition P4BuyerBuy;
    public ActionCondition P4BuyerNotBuy;
    public ActionCondition P4BeauShakeHand;

    // intro and ending stuff
    public ActionCondition Intro;
    public ActionCondition GoodEnding;
    public ActionCondition BadEnding;


    void Awake()
    {
        Services.actionConditionsManager = this;

        // initialize all the action conditions
        // puzzle #1
        P1goToAlarmClock = new ActionCondition("P1goToAlarmClock", new Vector3(0, 6, 0), new Vector3(0, 8, 30));
        P1ringAlarmClockEarly = new ActionCondition("P1ringAlarmClockEarly", new Vector3(0, 6, 0), new Vector3(0, 7, 29));
        P1ringAlarmClockOnTime = new ActionCondition("P1ringAlarmClockOnTime", new Vector3(0, 7, 33), new Vector3(0, 8, 0));
        P1ringAlarmClockLate = new ActionCondition("P1ringAlarmClockLate", new Vector3(0, 8, 0), new Vector3(0, 8, 30));
        P1wakeUpNaturally = new ActionCondition("P1wakeUpNaturally", new Vector3(0, 8, 30), new Vector3(1, 0, 0));

        // puzzle #2
        P2customerWalksIn = new ActionCondition("P2customerWalksIn", new Vector3(0, 8, 0), new Vector3(1, 0, 0));
        P2customerAskForClock = new ActionCondition("P2customerAskForClock", new Vector3(0, 8, 0), new Vector3(1, 0, 0));
        P2nieceReady = new ActionCondition("P2nieceReady", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2customerReady = new ActionCondition("P2customerReady", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2nieceGreeted = new ActionCondition("P2nieceGreeted", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2customerAsked = new ActionCondition("P2customerAsked", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2wrongClock = new ActionCondition("P2wrongClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2ringClocksUpstair = new ActionCondition("P2ringClocksUpstair", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2failed = new ActionCondition("P2failed", new Vector3(0, 10, 0), new Vector3(1, 0, 0));
        P2customerDecided = new ActionCondition("P2customerDecided", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2customerLeave = new ActionCondition("P2customerLeave", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2soldRightClock = new ActionCondition("P2soldRightClock", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P2soldWrongClock = new ActionCondition("P2soldWrongClock", new Vector3(0, 7, 0), new Vector3(1, 0, 0));

        P2grandfatherClock = new ActionCondition("P2grandfatherClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2mantelClock = new ActionCondition("P2mantelClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2cornerClock = new ActionCondition("P2cornerClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2deskClock = new ActionCondition("P2deskClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));
        P2triangleClock = new ActionCondition("P2triangleClock", new Vector3(0, 8, 15), new Vector3(0, 9, 59));

        // puzzle #3
        P3beauWalksIn = new ActionCondition("P3beauWalksIn", new Vector3(0, 10, 30), new Vector3(1, 0, 0));
        P3beauStartTalking = new ActionCondition("P3beauStartTalking", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P3NieceStartTalking = new ActionCondition("P3nieceStartTalking", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P3beauCheckWatchNormal = new ActionCondition("P3beauCheckWatchNormal", new Vector3(0, 12, 0), new Vector3(0, 12, 5));
        P3beauCheckWatchLate = new ActionCondition("P3beauCheckWatchLate", new Vector3(0, 12, 0), new Vector3(0, 12, 5));
        P3beauCheckWatchWrong = new ActionCondition("P3beauCheckWatchWrong", new Vector3(0, 12, 0), new Vector3(0, 12, 5));
        P3beauLeave = new ActionCondition("P3beauLeave", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P3nieceSelfTalk = new ActionCondition("P3nieceSelfTalk", new Vector3(0, 7, 0), new Vector3(1, 0, 0));

        // puzzle #4
        P4beauCallNormal = new ActionCondition("P4beauCallNormal", new Vector3(0, 13, 00), new Vector3(0, 13, 05));
        P4beauCallMad = new ActionCondition("P4beauCallMad", new Vector3(0, 13, 00), new Vector3(0, 13, 05));
        P4beauWalksIn = new ActionCondition("P4beauWalksIn", new Vector3(0, 14, 00), new Vector3(0, 14, 05));
        P4BuyerOneWalksIn = new ActionCondition("P4buyerOneWalksIn", new Vector3(0, 14, 01), new Vector3(0, 14, 06));
        P4BuyerTwoWalksIn = new ActionCondition("P4buyerTwoWalksIn", new Vector3(0, 14, 02), new Vector3(0, 14, 07));

        P4BuyerOneFinished = new ActionCondition("P4buyerOneFinished", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P4BuyerTwoFinished = new ActionCondition("P4buyerTwoFinished", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P4BeauGreets = new ActionCondition("P4beauGreets", new Vector3(0, 7, 0), new Vector3(1, 0, 0));

        P4BeauFinishes = new ActionCondition("P4beauFinishes", new Vector3(0, 7, 0), new Vector3(1, 0, 0));
        P4BuyerOneWants = new ActionCondition("P4buyerOneWants", new Vector3(0, 14, 25), new Vector3(0, 15, 09));
        P4BuyerTwoWants = new ActionCondition("P4buyerTwoWants", new Vector3(0, 14, 25), new Vector3(0, 15, 09));

        P4BuyerBuy = new ActionCondition("P4buyerBuy", new Vector3(0, 15, 10), new Vector3(1, 0, 0));
        P4BuyerNotBuy = new ActionCondition("P4buyerNotBuy", new Vector3(0, 15, 10), new Vector3(1, 0, 0));
        P4BeauShakeHand = new ActionCondition("P4beauShakeHand", new Vector3(0, 7, 0), new Vector3(1, 0, 0));

        Intro = new ActionCondition("Intro", new Vector3(0, 6, 0), new Vector3(0, 6, 5));
        GoodEnding = new ActionCondition("GoodEnding", new Vector3(0, 6, 0), new Vector3(1, 0, 0));
        BadEnding = new ActionCondition("BadEnding", new Vector3(0, 6, 0), new Vector3(1, 0, 0));


        allActionConditions.Add(P1goToAlarmClock);
        allActionConditions.Add(P1ringAlarmClockEarly);
        allActionConditions.Add(P1ringAlarmClockOnTime);
        allActionConditions.Add(P1ringAlarmClockLate);
        allActionConditions.Add(P1wakeUpNaturally);

        allActionConditions.Add(P2customerWalksIn);
        allActionConditions.Add(P2customerAskForClock);
        allActionConditions.Add(P2nieceReady);
        allActionConditions.Add(P2customerReady);
        allActionConditions.Add(P2nieceGreeted);
        allActionConditions.Add(P2customerAsked);
        allActionConditions.Add(P2wrongClock);
        allActionConditions.Add(P2ringClocksUpstair);
        allActionConditions.Add(P2failed);
        allActionConditions.Add(P2customerDecided);
        allActionConditions.Add(P2customerLeave);
        allActionConditions.Add(P2soldRightClock);
        allActionConditions.Add(P2soldWrongClock);
        allActionConditions.Add(P2grandfatherClock);
        allActionConditions.Add(P2mantelClock);
        allActionConditions.Add(P2cornerClock);
        allActionConditions.Add(P2deskClock);
        allActionConditions.Add(P2triangleClock);


        allActionConditions.Add(P3beauWalksIn);
        allActionConditions.Add(P3beauStartTalking);
        allActionConditions.Add(P3NieceStartTalking);
        allActionConditions.Add(P3beauCheckWatchNormal);
        allActionConditions.Add(P3beauCheckWatchLate);
        allActionConditions.Add(P3beauCheckWatchWrong);
        allActionConditions.Add(P3beauLeave);
        allActionConditions.Add(P3nieceSelfTalk);

        allActionConditions.Add(P4beauCallNormal);
        allActionConditions.Add(P4beauCallMad);
        allActionConditions.Add(P4beauWalksIn);
        allActionConditions.Add(P4BuyerOneWalksIn);
        allActionConditions.Add(P4BuyerTwoWalksIn);
        allActionConditions.Add(P4BuyerOneFinished);
        allActionConditions.Add(P4BuyerTwoFinished);
        allActionConditions.Add(P4BeauGreets);
        allActionConditions.Add(P4BeauFinishes);
        allActionConditions.Add(P4BuyerOneWants);
        allActionConditions.Add(P4BuyerTwoWants);
        allActionConditions.Add(P4BuyerBuy);
        allActionConditions.Add(P4BuyerNotBuy);
        allActionConditions.Add(P4BeauShakeHand);

        allActionConditions.Add(Intro);
        allActionConditions.Add(GoodEnding);
        allActionConditions.Add(BadEnding);
    }

    void Start()
    {
        tm = Services.timeManager;
        cm = Services.clockManager;
    }

    void Update()
    {
        float currentM = minutes(new Vector3(tm.day, tm.hour, tm.minute));

        // reset some repeatable action conditions
        P1ringAlarmClockEarly.state = CondState.NotSet;
        P2wrongClock.state = CondState.NotSet;
        P2grandfatherClock.state = CondState.NotSet;
        P2mantelClock.state = CondState.NotSet;
        P2cornerClock.state = CondState.NotSet;
        P2deskClock.state = CondState.NotSet;
        P2triangleClock.state = CondState.NotSet;


        // when forwarding, check and set all the action conditions
        if (!tm.skipping)
        {
            // puzzle #1
            // P1goToAlarmClock
            if (cm.currentClock.name == "alarmClock" && P1goToAlarmClock.withinTimeWindow(currentM)
                && P1goToAlarmClock.state != CondState.Ready)
            {
                setReady(P1goToAlarmClock);
            }

            // P1ringAlarmClockEarly
            if (cm.currentClock.name == "alarmClock" && isRinging && P1ringAlarmClockEarly.withinTimeWindow(currentM)
                && P1ringAlarmClockEarly.state != CondState.Ready)
            {
                setReady(P1ringAlarmClockEarly);
            }

            // P1ringAlarmClockOnTime
            if (cm.currentClock.name == "alarmClock" && isRinging && P1ringAlarmClockOnTime.withinTimeWindow(currentM)
                && P1ringAlarmClockOnTime.state != CondState.Ready)
            {
                setReady(P1ringAlarmClockOnTime);
            }

            // P1ringAlarmClockLate
            if (cm.currentClock.name == "alarmClock" && isRinging && P1ringAlarmClockLate.withinTimeWindow(currentM) && P1ringAlarmClockLate.state != CondState.Ready)
            {
                setReady(P1ringAlarmClockLate);
            }

            // P1wakeUpNaturally
            if (P1wakeUpNaturally.withinTimeWindow(currentM) && P1wakeUpNaturally.state != CondState.Ready 
                && P1ringAlarmClockOnTime.state != CondState.Ready && P1ringAlarmClockLate.state != CondState.Ready)
            {
                setReady(P1wakeUpNaturally);
            }

            // Puzzle #2
            // P2customerWalksIn
            if (P2customerWalksIn.withinTimeWindow(currentM) && P2customerWalksIn.state != CondState.Ready)
            {
                setReady(P2customerWalksIn);
            }

            // P2customerAskForClock
            if (P2customerAskForClock.withinTimeWindow(currentM) && P2customerAskForClock.state != CondState.Ready 
                && P2nieceGreeted.state == CondState.Ready && P2customerReady.state == CondState.Ready)
            {
                setReady(P2customerAskForClock);
            }

            // P2wrongClock
            if (P2wrongClock.withinTimeWindow(currentM) && P2wrongClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "pocketWatch")
            {
                setReady(P2wrongClock);
            }

            // P2ringClocksUpstair
            if (P2ringClocksUpstair.withinTimeWindow(currentM) && P2ringClocksUpstair.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && (cm.currentClock.name == "wristWatch" || cm.currentClock.name == "wallClock" || cm.currentClock.name == "alarmClock"))
            {
                setReady(P2ringClocksUpstair);
            }

            // P2failed
            if (P2failed.withinTimeWindow(currentM) && P2failed.state != CondState.Ready && P2ringClocksUpstair.state != CondState.Ready)
            {
                setReady(P2failed);
            }

            // P2grandfatherClock
            if (P2grandfatherClock.withinTimeWindow(currentM) && P2grandfatherClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "grandfatherClock")
            {
                setReady(P2grandfatherClock);
            }

            // P2mantelClock
            if (P2mantelClock.withinTimeWindow(currentM) && P2mantelClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "mantelClock")
            {
                setReady(P2mantelClock);
            }

            // P2cornerClock
            if (P2cornerClock.withinTimeWindow(currentM) && P2cornerClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "cornerClock")
            {
                setReady(P2cornerClock);
            }

            // P2deskClock
            if (P2deskClock.withinTimeWindow(currentM) && P2deskClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "deskClock")
            {
                setReady(P2deskClock);
            }

            // P2triangleClock
            if (P2triangleClock.withinTimeWindow(currentM) && P2triangleClock.state != CondState.Ready
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name == "startClock")
            {
                setReady(P2triangleClock);
            }

            // P3beauWalksIn
            if (P3beauWalksIn.withinTimeWindow(currentM) && P3beauWalksIn.state != CondState.Ready)
            {
                setReady(P3beauWalksIn);
            }

            // P3beauCheckWatchNormal
            if (P3beauCheckWatchNormal.withinTimeWindow(currentM) && P3beauCheckWatchNormal.state != CondState.Ready)
            {
                int mDiff = minutesFromRealTime();
                if (mDiff > -31 && mDiff < 2) 
                {
                    setReady(P3beauCheckWatchNormal);
                }
            }

            // P3beauCheckWatchLate
            if (P3beauCheckWatchLate.withinTimeWindow(currentM) && P3beauCheckWatchLate.state != CondState.Ready)
            {
                int mDiff = minutesFromRealTime();
                if (mDiff > 1 && mDiff < 31)
                {
                    setReady(P3beauCheckWatchLate);
                }
            }

            // P3beauCheckWatchWrong
            if (P3beauCheckWatchWrong.withinTimeWindow(currentM) && P3beauCheckWatchWrong.state != CondState.Ready)
            {
                int mDiff = minutesFromRealTime();
                if (mDiff > 30 || mDiff < -30)
                {
                    setReady(P3beauCheckWatchWrong);
                }
            }

            // P4beauCallNormal
            if (P4beauCallNormal.withinTimeWindow(currentM) && P4beauCallNormal.state != CondState.Ready && P3beauCheckWatchLate.state != CondState.Ready)
            {
                setReady(P4beauCallNormal);
            }

            // P4beauCallMad
            if (P4beauCallMad.withinTimeWindow(currentM) && P4beauCallMad.state != CondState.Ready && P3beauCheckWatchLate.state == CondState.Ready)
            {
                setReady(P4beauCallMad);
            }

            // P4beauWalksIn
            if (P4beauWalksIn.withinTimeWindow(currentM) && P4beauWalksIn.state != CondState.Ready)
            {
                setReady(P4beauWalksIn);
            }

            // P4buyerOneWalksIn
            if (P4BuyerOneWalksIn.withinTimeWindow(currentM) && P4BuyerOneWalksIn.state != CondState.Ready)
            {
                setReady(P4BuyerOneWalksIn);
            }

            // P4buyerTwoWalksIn
            if (P4BuyerTwoWalksIn.withinTimeWindow(currentM) && P4BuyerTwoWalksIn.state != CondState.Ready)
            {
                if (P3beauCheckWatchLate.state != CondState.Ready)
                {
                    setReady(P4BuyerTwoWalksIn);
                }
            }

            // P4beauGreets
            if (P4BeauGreets.withinTimeWindow(currentM) && P4BeauGreets.state != CondState.Ready)
            {
                if (P4BuyerOneFinished.state == CondState.Ready)
                {
                    if (P3beauCheckWatchLate.state == CondState.Ready || P4BuyerTwoFinished.state == CondState.Ready)
                    {
                        setReady(P4BeauGreets);
                    }
                }
            }

            // P4buyerOneWants
            if (P4BuyerOneWants.withinTimeWindow(currentM) && P4BuyerOneWants.state != CondState.Ready)
            {
                if (isRinging && Services.clockManager.currentClock.name == "grandfatherClock")
                {
                    setReady(P4BuyerOneWants);
                }
            }

            // P4buyerTwoWants
            if (P4BuyerTwoWants.withinTimeWindow(currentM) && P4BuyerTwoWants.state != CondState.Ready && P4BuyerTwoWalksIn.state == CondState.Ready)
            {
                if (isRinging && Services.clockManager.currentClock.name == "deskClock")
                {
                    setReady(P4BuyerTwoWants);
                }
            }

            // P4BuyerNotBuy
            if (P4BuyerNotBuy.withinTimeWindow(currentM) && P4BuyerNotBuy.state != CondState.Ready)
            {
                if (P4BuyerOneWants.state == CondState.Ready)
                {
                    if (P3beauCheckWatchLate.state == CondState.Ready || P4BuyerTwoWants.state == CondState.Ready)
                    {
                        setReady(P4BuyerNotBuy);
                    }
                }
            }

            // P4BuyerBuy
            if (P4BuyerBuy.withinTimeWindow(currentM) && P4BuyerBuy.state != CondState.Ready)
            {
                if (P4BuyerOneWants.state != CondState.Ready)
                {
                    setReady(P4BuyerBuy);
                }
                else
                {
                    if (P3beauCheckWatchLate.state != CondState.Ready && P4BuyerTwoWants.state != CondState.Ready)
                    {
                        setReady(P4BuyerBuy);
                    }
                }
            }

            // Intro
            if (Intro.withinTimeWindow(currentM) && Intro.state != CondState.Ready)
            {
                setReady(Intro);
            }


            isRinging = false;
        }


        // when rewinding, loop through all the action conditions, if current time is less than create time, set the state to greyed
        if (tm.skipping)
        {
            foreach (ActionCondition ac in allActionConditions)
            {
                if (ac.state == CondState.Ready)
                {
                    float createM = minutes(ac.createTime);
                    if (currentM < createM)
                    {
                        ac.state = CondState.Greyed;
                        Debug.Log("Action cond: " + ac.name + " set to grey");
                    }
                }
            }
        }
    }

    public void LateUpdate()
    {
        
    }

    public ActionCondition getActionConditionByName(string name)
    {
        ActionCondition ret = null;
        foreach (ActionCondition ac in allActionConditions)
        {
            if (ac.name.Equals(name))
            {
                ret = ac;
                break;
            }
        }
        return ret;
    }

    public void setActionConditionByName(string name, bool b = true)
    {
        foreach(ActionCondition ac in allActionConditions)
        {
            if (ac.name.Equals(name))
            {
                if (b) setReady(ac);
                else setGreyed(ac);
                return;
            }
        }
        Debug.LogWarning("Action condition " + name + " not found");
    }

    private void setGreyed(ActionCondition ac)
    {
        ac.state = CondState.Greyed;
    }

    private void setReady(ActionCondition ac)
    {
        float currentM = minutes(new Vector3(tm.day, tm.hour, tm.minute));
        ac.state = CondState.Ready;
        ac.SetCreateTime(tm.day, tm.hour, tm.minute);
        Debug.Log("Action cond: " + ac.name + " set to Ready");
    }

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }

    // if positive, watch is late; if negative, watch is early
    private int minutesFromRealTime()
    {
        if (beauWatch == null) return 0;
        int hr = beauWatch.myHour;
        int mi = beauWatch.myMinute;
        int realHr = Services.timeManager.hour;
        int realMinute = (int)Services.timeManager.minute;
        int mDiff = realMinute - mi;
        mDiff += (realHr - hr) * 60;
        Debug.Log("watch is " + mDiff + " minutes late");
        return mDiff;
    }
}

public class ActionCondition
{
    public string name;
    public CondState state;
    public Vector3 createTime;
    public Vector3 triggerWindowStart;
    public Vector3 triggerWindowEnd;

    public ActionCondition(string name, Vector3 triggerWindowStart, Vector3 triggerWindowEnd)
    {
        this.name = name;
        this.state = CondState.NotSet;
        this.createTime = Vector3.zero;
        this.triggerWindowStart = triggerWindowStart;
        this.triggerWindowEnd = triggerWindowEnd;
    }

    public void SetCreateTime(Vector3 t)
    {
        this.createTime = t;
    }

    public void SetCreateTime(int day, int hour, float minute)
    {
        this.createTime = new Vector3(day, hour, minute);
    }

    public bool withinTimeWindow(float m)
    {
        float sm = minutes(triggerWindowStart);
        float em = minutes(triggerWindowEnd);
        if (m > sm && m < em) return true;
        else return false;
    }

    public bool afterTimeWindow(float m)
    {
        float em = minutes(triggerWindowEnd);
        if (m > em) return true;
        else return false;
    }

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }
}