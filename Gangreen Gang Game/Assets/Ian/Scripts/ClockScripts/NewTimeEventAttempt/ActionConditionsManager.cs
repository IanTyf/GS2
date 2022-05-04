using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CondState { NotSet, Ready, Greyed };

public class ActionConditionsManager : MonoBehaviour
{
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
                && P2customerAsked.state == CondState.Ready && isRinging && cm.currentClock.name != "wristWatch" && cm.currentClock.name != "pocketWatch")
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