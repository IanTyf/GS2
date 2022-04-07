using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeCond { Ready, NotSet };

// Takes care of storing, managing, and executing all the time events
public class TimeEventManager : MonoBehaviour
{

    public List<TimeEvent> initialTimeEvents = new List<TimeEvent>();
    public List<TimeEvent> dynamicTimeEvents = new List<TimeEvent>(); // events that are added in real-time based on player decisions; should be removed when rewind past the start time
    public List<TimeEvent> allTimeEvents = new List<TimeEvent>();

    public GameObject testObj;
    public GameObject testObj2;

    private TimeManager tm;
    private bool endOfSkipping;

    // listener conditionals that get reset each loop so that it only triggers once
    [HideInInspector]
    public bool isRinging;
    // list of conditionals that trigger different events
    public List<TimedConditional> timeConditionals = new List<TimedConditional>();

    public TimedConditional testEvent2 = new TimedConditional();

    private void Awake()
    {
        Services.timeEventManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tm = Services.timeManager;

        // VERY IMPORTANT STEPS BELOW

        // add all the initial events that will play no matter what
        addInitialEvent("Test event", new Vector3(0, 8, 10), testObj, "Test");

        // add all the time conditionals to the list so that when rewind, can look through the list and remove ones that should be removed
        timeConditionals.Add(testEvent2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentTime = new Vector3(tm.day, tm.hour, tm.minute);

        #region dynamicEventCreation
        if (!tm.skipping)
        {
            // test2 event - if ring is pressed during 0,8,10 and 0,8,20, trigger test2 actor anim
            // step one: if statements that set each timeconditional given certain requirements
            if (testEvent2.cond == TimeCond.NotSet && isRinging && minutes(currentTime) < minutes(new Vector3(0, 8, 20)) && minutes(currentTime) > minutes(new Vector3(0, 8, 10)))
            {
                testEvent2.cond = TimeCond.Ready;
                testEvent2.creationTime = currentTime;
                //testEvent2 = new TimedConditional(TimeCond.Ready, currentTime);
                //timeConditionals.Add(testEvent2); //IMPORTANT: add to a list so that when rewind, can look through the list and remove ones that should be removed
            }
            // step two: if statements that checks all related timeconditionals at a time range
            // here this event is triggered immediately after the conditions are met, so start time is set to minute+0.5. sometimes it might be set to a particular start time
            if (testEvent2.cond == TimeCond.Ready)
            {
                addDynamicEvent("Test2 event", new Vector3(tm.day, tm.hour, tm.minute + 0.5f), testObj2, "Test2");
                //Debug.Log("new dynamic event <Test2 event> created");
                //testEvent2.cond = TimeCond.Exausted;
            }


            // reset listener variables
            isRinging = false;
        }
        #endregion


        #region eventHandling
        List<TimeEvent> eventsShouldPlay = new List<TimeEvent>();

        if (!tm.skipping)
        {
            // first handle deletion and reseting if we just finished skipping
            if (endOfSkipping)
            {
                handleDeletion(currentTime);
                endOfSkipping = false;
            }


            eventsShouldPlay = getEvents(currentTime);
            if (eventsShouldPlay.Count == 0) Debug.Log("no event should be played now");
            else
            {
                // loop through the current events and play them if they haven't been played
                foreach (TimeEvent evt in eventsShouldPlay)
                {
                    //if (!evt.isPlaying)
                    {
                        float normalizedTimeScale = (minutes(currentTime) - minutes(evt.startTime)) / evt.length;
                        Debug.Log("start playing event <" + evt.animState + "> on object <" + evt.actor.name + ">");
                        evt.playAnim(normalizedTimeScale);
                    }
                }
            }
        }
        // Rewinding/Forwording
        else
        {
            for (int i=allTimeEvents.Count-1; i >= 0; i--)
            {
                TimeEvent evt = allTimeEvents[i];
                float currentM = minutes(currentTime);
                float startM = minutes(evt.startTime);
                float endM = minutes(evt.endTime);
                float createM = minutes(evt.createTime);
                if (currentM < createM)
                {
                    //allTimeEvents.Remove(evt);
                    //dynamicTimeEvents.Remove(evt); //handled at the end of skipping in handleDeletion
                }
                else if (currentM < startM)
                {
                    evt.setAnimToFrame(0);
                }
                else if (currentM < endM)
                {
                    evt.setAnimToFrame((currentM - startM) / (evt.length));
                }
                else
                {
                    // do nothing
                }
            }
            
            for (int i = 0; i < allTimeEvents.Count; i++)
            {
                TimeEvent evt = allTimeEvents[i];
                float currentM = minutes(currentTime);
                float startM = minutes(evt.startTime);
                float endM = minutes(evt.endTime);
                float createM = minutes(evt.createTime);
                if (currentM > endM)
                {
                    evt.setAnimToFrame(2);
                }
            }

            endOfSkipping = true;
        }
        #endregion
    }

    public void addInitialEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        initialTimeEvents.Add(new TimeEvent(eventName, startTime, actor, animState));
        // sort the time events by their startTime
        initialTimeEvents.Sort(compareStartTime);

        allTimeEvents.Add(new TimeEvent(eventName, startTime, actor, animState));
        allTimeEvents.Sort(compareStartTime);
    }

    public void addDynamicEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        if (dynamicTimeEvents.Find(x => x.eventName == eventName) != null)
        {
            Debug.Log("event already exist in list, failed to add");
            return;
        }

        // only add if it doesn't already exist in the list
        TimeEvent newEvt = new TimeEvent(eventName, startTime, actor, animState, new Vector3(tm.day, tm.hour, tm.minute));
        if (dynamicTimeEvents.Contains(newEvt))
        {
            return;
        }

        dynamicTimeEvents.Add(newEvt);
        // sort the time events by their startTime
        dynamicTimeEvents.Sort(compareStartTime);

        allTimeEvents.Add(new TimeEvent(eventName, startTime, actor, animState, new Vector3(tm.day, tm.hour, tm.minute)));
        allTimeEvents.Sort(compareStartTime);

        Debug.Log("new dynamic event <"+eventName+"> created");
    }

    private int compareStartTime(TimeEvent evt1, TimeEvent evt2)
    {
        float evt1M = minutes(evt1.startTime);
        float evt2M = minutes(evt2.startTime);
        if (evt1M == evt2M) return 0;
        else return (evt1M < evt2M) ? -1 : 1;
    }

    // given a vec3 time, return the event that should be happening
    public List<TimeEvent> getEvents(Vector3 t, bool removePastDynamic = false)
    {
        List<TimeEvent> events = new List<TimeEvent>();

        foreach (TimeEvent evt in initialTimeEvents)
        {
            int startComp = compareTime(evt.startTime, t);
            int endComp = compareTime(evt.endTime, t);
            if ((startComp == -1 || startComp == 0) && (endComp == 1 || endComp == 0))
            {
                events.Add(evt);
            }
            else
            {
                //set the isPlaying flag to false so that it can be played
                evt.isPlaying = false;
            }
        }
        foreach (TimeEvent evt in dynamicTimeEvents)
        {
            int startComp = compareTime(evt.startTime, t);
            int endComp = compareTime(evt.endTime, t);
            if ((startComp == -1 || startComp == 0) && (endComp == 1 || endComp == 0))
            {
                events.Add(evt);
            }
            else
            {
                //set the isPlaying flag to false so that it can be played
                evt.isPlaying = false;

                //remove the dynamic ones if we are before its creation time
                if (removePastDynamic && startComp == 1)
                {
                    int createComp = compareTime(evt.createTime, t);
                    if (createComp == 1)
                    {
                        dynamicTimeEvents.Remove(evt);
                        allTimeEvents.Remove(evt);
                    }
                }
            }
        }

        return events;
    }

    private void handleDeletion(Vector3 currentTime)
    {
        for (int i = allTimeEvents.Count - 1; i >= 0; i--)
        {
            TimeEvent evt = allTimeEvents[i];
            float currentM = minutes(currentTime);
            float startM = minutes(evt.startTime);
            float endM = minutes(evt.endTime);
            float createM = minutes(evt.createTime);
            if (currentM < createM)
            {
                allTimeEvents.Remove(evt);
                dynamicTimeEvents.Remove(evt);
            }
        }
        for (int i = 0; i < timeConditionals.Count; i++)
        {
            TimedConditional tc = timeConditionals[i];
            if (tc == null) continue;

            float currentM = minutes(currentTime);
            float tcM = minutes(tc.creationTime);
            if (currentM < tcM)
            {
                Debug.Log("reset to NotSet");
                timeConditionals[i].cond = TimeCond.NotSet;
            }
        }
    }

    // if time1 less than time2, return -1. if time1 larger than time2, return 1. if same return 0
    private int compareTime(Vector3 time1, Vector3 time2)
    {
        float time1Minutes = minutes(time1);
        float time2Minutes = minutes(time2);
        if (time1Minutes == time2Minutes) return 0;
        else return (time1Minutes < time2Minutes) ? -1 : 1;
    }

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }
}

public class TimeEvent
{
    public string eventName;
    public Vector3 startTime; // day, hour, minute
    public Vector3 endTime; // day, hour, minute
    public float length; // in minutes
    public bool isPlaying;
    public GameObject actor;
    public string animState;

    public Vector3 createTime; //time when this event is created. during rewind, if time < createTime, remove this time event

    public TimeEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        this.eventName = eventName;
        this.startTime = startTime;
        this.isPlaying = false;
        this.actor = actor;
        this.animState = animState;
        this.createTime = Vector3.zero;

        AnimationClip[] clips = actor.GetComponent<Actor>().anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animState)
            {
                this.length = clip.length;
            }
        }
        Vector3 unformatedEndTime = new Vector3(startTime.x, startTime.y, startTime.z + this.length);
        this.endTime = reformatTime(unformatedEndTime);

        //printInfo();
    }

    public TimeEvent(string eventName, Vector3 startTime, GameObject actor, string animState, Vector3 createTime)
    {
        this.eventName = eventName;
        this.startTime = startTime;
        this.isPlaying = false;
        this.actor = actor;
        this.animState = animState;
        this.createTime = createTime;

        AnimationClip[] clips = actor.GetComponent<Actor>().anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animState)
            {
                this.length = clip.length;
            }
        }
        Vector3 unformatedEndTime = new Vector3(startTime.x, startTime.y, startTime.z + this.length);
        this.endTime = reformatTime(unformatedEndTime);

        //printInfo();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        TimeEvent evt = (TimeEvent)obj;
        return this.eventName.Equals(evt.eventName);
    }

    public override int GetHashCode()
    {
        return (int)minutes(this.startTime);
    }

    // this should only be called when not rewinding, since when rewinding we want to directly set to particular animation frames
    public void playAnim(float normalizedTimeScale)
    {
        this.actor.GetComponent<Actor>().PlayAnim(this.animState, normalizedTimeScale);
        this.isPlaying = true;
    }

    public void setAnimToFrame(float normalizedTimeScale)
    {
        this.actor.GetComponent<Actor>().SetAnimToFrame(this.animState, normalizedTimeScale);
        this.isPlaying = false;
    }

    public Vector3 reformatTime(Vector3 time)
    {
        float minute = time.z;
        float hour = time.y;
        float day = time.x;

        while (minute >= 60f)
        {
            minute -= 60;
            hour++;
        }

        while (hour >= 24)
        {
            hour -= 24;
            day++;
        }

        return new Vector3(day, hour, minute);
    }

    private void printInfo()
    {
        Debug.Log("Event info:\n    name: "+this.eventName+", startTime: "+this.startTime+", endTime: "+this.endTime+", length: "+this.length+ "\n  isPlaying: "+this.isPlaying+", actor name: " + this.actor.name + ", animState: " + this.animState + ", createTime: " + this.createTime);
    }

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }
}

public class TimedConditional
{
    public TimeCond cond;
    public Vector3 creationTime;

    public TimedConditional()
    {
        cond = TimeCond.NotSet;
        creationTime = Vector3.zero;
    }

    public TimedConditional(TimeCond c, Vector3 creationTime)
    {
        this.cond = c;
        this.creationTime = creationTime;
    }
}
