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


    void Awake()
    {
        Services.actionConditionsManager = this;

        // initialize all the action conditions
        // puzzle #1
        P1goToAlarmClock = new ActionCondition("P1goToAlarmClock", new Vector3(0, 6, 0), new Vector3(0, 8, 30));
        P1ringAlarmClockEarly = new ActionCondition("P1ringAlarmClockEarly", new Vector3(0, 6, 0), new Vector3(0, 7, 29));
        P1ringAlarmClockOnTime = new ActionCondition("P1ringAlarmClockOnTime", new Vector3(0, 7, 33), new Vector3(0, 8, 0));
        P1ringAlarmClockLate = new ActionCondition("P1ringAlarmClockLate", new Vector3(0, 8, 0), new Vector3(0, 8, 30));

        allActionConditions.Add(P1goToAlarmClock);
        allActionConditions.Add(P1ringAlarmClockEarly);
        allActionConditions.Add(P1ringAlarmClockOnTime);
        allActionConditions.Add(P1ringAlarmClockLate);

    }

    void Start()
    {
        tm = Services.timeManager;
        cm = Services.clockManager;
    }

    void Update()
    {
        float currentM = minutes(new Vector3(tm.day, tm.hour, tm.minute));

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
            if (
                (cm.currentClock.name == "alarmClock" && isRinging && P1ringAlarmClockLate.withinTimeWindow(currentM) && P1ringAlarmClockLate.state != CondState.Ready) ||
                (P1ringAlarmClockLate.afterTimeWindow(currentM) && P1ringAlarmClockLate.state != CondState.Ready)
                )
            {
                setReady(P1ringAlarmClockLate);
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
        P1ringAlarmClockEarly.state = CondState.NotSet;
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