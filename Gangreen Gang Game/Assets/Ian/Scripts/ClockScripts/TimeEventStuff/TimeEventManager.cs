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

    //public TimeEventDB timeEventDB;

    //public GameObject testObj;
    //public GameObject testObj2;
    public GameObject figure;

    private TimeManager tm;
    private bool endOfSkipping;

    // listener conditionals that get reset each loop so that it only triggers once
    [HideInInspector]
    public bool isRinging;
    // list of conditionals that trigger different events
    public List<TimedConditional> timeConditionals = new List<TimedConditional>();

    //public TimedConditional testEvent2 = new TimedConditional();
    public TimedConditional wakeupOnTime = new TimedConditional();
    public TimedConditional wakeupEarly = new TimedConditional();
    public TimedConditional wakeupLate = new TimedConditional();
    public TimedConditional wakeupNaturally = new TimedConditional();

    private void Awake()
    {
        Services.timeEventManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tm = Services.timeManager;
        /*
        if (timeEventDB != null)
        {
            InitialTimeEventSetup[] initialEvents = timeEventDB.initialEvents;
            TimeEventSetup[] dynamicEvents = timeEventDB.dynamicEvents;

            foreach (InitialTimeEventSetup initialEvent in initialEvents)
            {
                addInitialEvent(initialEvent.nameOfEvent, new Vector3(initialEvent.day, initialEvent.hour, initialEvent.minute),
                    initialEvent.actor, initialEvent.nameOfAnimationClip);
            }
        }
        */
        // VERY IMPORTANT STEPS BELOW

        // add all the initial events that will play no matter what
        //addInitialEvent("Test event", new Vector3(0, 8, 10), testObj, "Test");

        // add all the time conditionals to the list so that when rewind, can look through the list and remove ones that should be removed
        //timeConditionals.Add(testEvent2);
        timeConditionals.Add(wakeupOnTime);
        timeConditionals.Add(wakeupEarly);
        timeConditionals.Add(wakeupLate);
        timeConditionals.Add(wakeupNaturally);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentTime = new Vector3(tm.day, tm.hour, tm.minute);

        #region dynamicEventCreation
        //if (!tm.skipping)
        {
            // test2 event - if ring is pressed during 0,8,10 and 0,8,20, trigger test2 actor anim
            // step one: if statements that set each timeconditional given certain requirements
            /*
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
            */

            #region wakeupEarly Event
            if (wakeupEarly.cond == TimeCond.NotSet && isRinging && Services.clockManager.currentClock.name == "alarmClock"
                && minutes(currentTime) < minutes(new Vector3(0, 7, 27)))
            {
                wakeupEarly.cond = TimeCond.Ready;
                wakeupEarly.creationTime = currentTime;
            }

            if (wakeupEarly.cond == TimeCond.Ready)
            {
                TimeEvent evt = addDynamicEvent("WakeupEarly", new Vector3(tm.day, tm.hour, tm.minute + 0.5f), figure, "WakeUp00");
                if (evt != null) evt.addSubtitle("I still got some time..", new Vector3(tm.day, tm.hour, tm.minute + 0.7f), new Vector3(tm.day, tm.hour, tm.minute + 4.7f));
            }

            #endregion

            #region wakeupOnTime Event
            if (wakeupOnTime.cond == TimeCond.NotSet && isRinging && Services.clockManager.currentClock.name == "alarmClock" 
                && minutes(currentTime) < minutes(new Vector3(0,8,0)) && minutes(currentTime) > minutes(new Vector3(0,7,33)) )
            {
                wakeupOnTime.cond = TimeCond.Ready;
                wakeupOnTime.creationTime = currentTime;
            }

            if (wakeupOnTime.cond == TimeCond.Ready)
            {
                TimeEvent evt = addDynamicEvent("WakeupOnTime", new Vector3(tm.day, tm.hour, tm.minute + 0.5f), figure, "WakeUp01");
                if (evt != null) evt.addSubtitle("Alright, Wakey Wakey.", new Vector3(tm.day, tm.hour, tm.minute + 2.5f), new Vector3(tm.day, tm.hour, tm.minute + 6.5f));
            }
            #endregion

            #region wakeupLate Event
            if (wakeupLate.cond == TimeCond.NotSet && wakeupOnTime.cond == TimeCond.NotSet && isRinging && Services.clockManager.currentClock.name == "alarmClock"
                && minutes(currentTime) > minutes(new Vector3(0, 8, 0)) && minutes(currentTime) < minutes(new Vector3(0, 8, 30)))
            {
                wakeupLate.cond = TimeCond.Ready;
                wakeupLate.creationTime = currentTime;
            }

            if (wakeupLate.cond == TimeCond.Ready)
            {
                TimeEvent evt = addDynamicEvent("WakeupLate", new Vector3(tm.day, tm.hour, tm.minute + 0.5f), figure, "WakeUp01");
                if (evt != null) evt.addSubtitle("Oh No! I'm late!", new Vector3(tm.day, tm.hour, tm.minute + 2.5f), new Vector3(tm.day, tm.hour, tm.minute + 6.5f));
            }

            #endregion
            
            #region wakeupNaturally Event
            if (wakeupNaturally.cond == TimeCond.NotSet && wakeupOnTime.cond == TimeCond.NotSet && wakeupLate.cond == TimeCond.NotSet
                && minutes(currentTime) > minutes(new Vector3(0, 8, 30)))
            {
                wakeupNaturally.cond = TimeCond.Ready;
                wakeupNaturally.creationTime = currentTime;
            }

            if (wakeupNaturally.cond == TimeCond.Ready)
            {
                TimeEvent evt = addDynamicEvent("wakeupNaturally", new Vector3(tm.day, tm.hour, tm.minute + 0.5f), figure, "WakeUp01");
                if (evt != null) evt.addSubtitle("Oh No! I'm late!", new Vector3(tm.day, tm.hour, tm.minute + 2.5f), new Vector3(tm.day, tm.hour, tm.minute + 6.5f));
            }

            #endregion


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
            for (int i = 0; i < allTimeEvents.Count; i++)
            {
                TimeEvent evt = allTimeEvents[i];
                float currentM = minutes(currentTime);
                float startM = minutes(evt.startTime);
                float endM = minutes(evt.endTime);
                float createM = minutes(evt.createTime);
                if (currentM > endM)
                {
                    UI_Text.Write(" ", 0.001f, true);
                    evt.setAnimToFrame(2);
                }
            }


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
                    UI_Text.Write(" ", 0.001f, true);
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
            
            

            endOfSkipping = true;
        }
        #endregion
    }

    public TimeEvent addInitialEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        TimeEvent newEvt = new TimeEvent(eventName, startTime, actor, animState);
        initialTimeEvents.Add(newEvt);
        // sort the time events by their startTime
        initialTimeEvents.Sort(compareStartTime);

        allTimeEvents.Add(newEvt);
        allTimeEvents.Sort(compareStartTime);

        return newEvt;
    }

    public TimeEvent addDynamicEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        if (dynamicTimeEvents.Find(x => x.eventName == eventName) != null)
        {
            Debug.Log("event already exist in list, failed to add");
            return null;
        }

        // only add if it doesn't already exist in the list
        /*
        if (dynamicTimeEvents.Contains(newEvt))
        {
            return;
        }
        */
        TimeEvent newEvt = new TimeEvent(eventName, startTime, actor, animState, new Vector3(tm.day, tm.hour, tm.minute));

        dynamicTimeEvents.Add(newEvt);
        // sort the time events by their startTime
        dynamicTimeEvents.Sort(compareStartTime);

        allTimeEvents.Add(new TimeEvent(eventName, startTime, actor, animState, new Vector3(tm.day, tm.hour, tm.minute)));
        allTimeEvents.Sort(compareStartTime);

        Debug.Log("new dynamic event <"+eventName+"> created");
        return newEvt;
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

    public List<Subtitle> subtitles;

    public Vector3 createTime; //time when this event is created. during rewind, if time < createTime, remove this time event

    public TimeEvent(string eventName, Vector3 startTime, GameObject actor, string animState)
    {
        this.eventName = eventName;
        this.startTime = startTime;
        this.isPlaying = false;
        this.actor = actor;
        this.animState = animState;
        this.createTime = Vector3.zero;
        this.subtitles = new List<Subtitle>();

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
        this.subtitles = new List<Subtitle>();

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

        printInfo();
    }

    public void addSubtitle(string text, Vector3 startTime, Vector3 endTime)
    {
        Subtitle newSub = new Subtitle(text, startTime, endTime);
        subtitles.Add(newSub);
        subtitles.Sort(compareSubtitle);
    }

    public int compareSubtitle(Subtitle s1, Subtitle s2)
    {
        float s1M = minutes(s1.startTime);
        float s2M = minutes(s2.startTime);
        if (s1M == s2M) return 0;
        else return (s1M < s2M) ? -1 : 1;
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

        for (int i = 0; i < subtitles.Count; i++)
        {
            Subtitle sub = subtitles[i];
            Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
            float currentM = minutes(currentTime);
            float startTimeM = minutes(sub.startTime);
            float endTimeM = minutes(sub.endTime);
            Debug.Log("currentM: " + currentM + ", endM: " + endTimeM);
            if (currentM > endTimeM)
            {
                Debug.Log("RESET TO EMPTY");
                UI_Text.Write(" ", 0.001f, true);
            }
        }

        for (int i = subtitles.Count-1; i>=0; i--)
        {
            Subtitle sub = subtitles[i];
            Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
            float currentM = minutes(currentTime);
            float startTimeM = minutes(sub.startTime);
            float endTimeM = minutes(sub.endTime);
            if (currentM >= startTimeM && currentM <= startTimeM + 0.1)
            {
                UI_Text.Write(sub.text, 0.05f, true);
            }
            else if (currentM >= endTimeM - 0.1 && currentM <= endTimeM)
            {
                UI_Text.Write(" ", 0.001f, true);
            }
            else if (currentM < startTimeM)
            {
                UI_Text.Write(" ", 0.001f, true);
            }
        }

        this.isPlaying = true;
    }

    public void setAnimToFrame(float normalizedTimeScale)
    {
        this.actor.GetComponent<Actor>().SetAnimToFrame(this.animState, normalizedTimeScale);

        for (int i = 0; i<subtitles.Count; i++)
        {
            Subtitle sub = subtitles[i];
            Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
            float currentM = minutes(currentTime);
            float startTimeM = minutes(sub.startTime);
            float endTimeM = minutes(sub.endTime);
            if (currentM > endTimeM)
            {
                UI_Text.Write(" ", 0.001f, true);
            }
        }

        for (int i = subtitles.Count - 1; i >= 0; i--)
        {
            Subtitle sub = subtitles[i];
            Vector3 currentTime = new Vector3(Services.timeManager.day, Services.timeManager.hour, Services.timeManager.minute);
            float currentM = minutes(currentTime);
            float startTimeM = minutes(sub.startTime);
            float endTimeM = minutes(sub.endTime);
            if (currentM >= startTimeM && currentM <= startTimeM + 0.1)
            {
                UI_Text.Write(sub.text, 0.05f, true);
            }
            else if (currentM >= endTimeM - 0.1 && currentM <= endTimeM)
            {
                UI_Text.Write(" ", 0.001f, true);
            }
            else if (currentM < startTimeM)
            {
                Debug.Log("before start time when skipping");
                UI_Text.Write(" ", 0.001f, true);
            }
        }

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

public class Subtitle
{
    public string text;
    public Vector3 startTime;
    public Vector3 endTime;
    public float length;

    public Subtitle(string text, Vector3 startTime, Vector3 endTime)
    {
        this.text = text;
        this.startTime = startTime;
        this.endTime = endTime;
        this.length = minutes(endTime) - minutes(startTime);
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
