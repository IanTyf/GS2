using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubTaskState { Hidden, Completed, Greyed }
public enum MainObjectiveState { Hidden, Visible, Completed, Greyed }

public class TaskMenuManager : MonoBehaviour
{
    public List<MainObjective> allMainObjectives = new List<MainObjective>();

    // declare all main objectives and their subtasks
    // puzzle #1
    public MainObjective p1;
    public SubTask p1GoToAlarmClock;
    public SubTask p1RingAlarmClock;

    private ActionConditionsManager acm;

    private void Awake()
    {
        Services.taskMenuManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        acm = Services.actionConditionsManager;

        // initialize all main objectives and their subtasks
        // puzzle #1
        p1GoToAlarmClock = new SubTask("p1GoToAlarmClock", "Enter Alarm Clock");
        p1GoToAlarmClock.addRequiredActionCondition(acm.P1goToAlarmClock);
        p1RingAlarmClock = new SubTask("p1RingAlarmClock", "Ring Alarm Clock At Proper Time");
        //p1RingAlarmClock.addSelectiveActionCondition(acm.P1ringAlarmClockEarly);
        p1RingAlarmClock.addSelectiveActionCondition(acm.P1ringAlarmClockOnTime);
        p1RingAlarmClock.addSelectiveActionCondition(acm.P1ringAlarmClockLate);

        p1 = new MainObjective("p1", "WAKE UP NIECE");
        p1.addRequiredSubTask(p1GoToAlarmClock);
        p1.addRequiredSubTask(p1RingAlarmClock);
        allMainObjectives.Add(p1);






        p1.state = MainObjectiveState.Visible;
    }

    // Update is called once per frame
    void Update()
    {
        string debugMsg = "Objective progress: \n";
        foreach (MainObjective m in allMainObjectives)
        {
            m.updateState();
            debugMsg += "    " + m.displayText + " -- " + m.state + "\n";
            foreach (SubTask st in m.requiredSubTasks) {
                debugMsg += "        " + st.displayText + " -- " + st.state + "\n";
            }
            foreach (SubTask st in m.optionalSubTasks)
            {
                debugMsg += "        " + st.displayText + " -- " + st.state + "\n";
            }
        }

        Debug.Log(debugMsg);
    }
}

public class MainObjective
{
    public List<SubTask> requiredSubTasks; // need to complete all
    public List<SubTask> optionalSubTasks; // need to complete NONE

    public string name;
    public string displayText;
    public MainObjectiveState state;

    public MainObjective(string name, string displayText)
    {
        this.name = name;
        this.displayText = displayText;
        this.state = MainObjectiveState.Hidden;

        requiredSubTasks = new List<SubTask>();
        optionalSubTasks = new List<SubTask>();
    }

    public void addRequiredSubTask(SubTask st)
    {
        requiredSubTasks.Add(st);
    }

    public void addOptionalSubTask(SubTask st)
    {
        optionalSubTasks.Add(st);
    }

    public void updateState()
    {
        foreach (SubTask st in requiredSubTasks)
        {
            st.updateState();
        }
        foreach (SubTask st in optionalSubTasks)
        {
            st.updateState();
        }

        if (this.state == MainObjectiveState.Visible) 
        {
            bool b = true;
            foreach (SubTask st in requiredSubTasks)
            {
                if (st.state != SubTaskState.Completed) b = false;
            }

            if (b) this.state = MainObjectiveState.Completed;
        }
        else if (this.state == MainObjectiveState.Completed)
        {
            bool b = false;
            foreach (SubTask st in requiredSubTasks)
            {
                if (st.state == SubTaskState.Greyed) b = true;
            }

            if (b) this.state = MainObjectiveState.Greyed;
        }
        else if (this.state == MainObjectiveState.Greyed)
        {
            bool b = true;
            foreach (SubTask st in requiredSubTasks)
            {
                if (st.state != SubTaskState.Completed) b = false;
            }

            if (b) this.state = MainObjectiveState.Completed;
        }
    }
}

public class SubTask
{
    public List<ActionCondition> requiredActionConditions; // all of these need to be ready
    public List<ActionCondition> selectiveActionConditions; // at least one of these is ready

    public string name;
    public string displayText;
    public SubTaskState state;
    public string ActiveOptionalCondition;

    public SubTask(string name, string displayText)
    {
        this.name = name;
        this.displayText = displayText;
        this.state = SubTaskState.Hidden;
        this.ActiveOptionalCondition = null;

        requiredActionConditions = new List<ActionCondition>();
        selectiveActionConditions = new List<ActionCondition>();
    }

    public void addRequiredActionCondition(ActionCondition ac)
    {
        requiredActionConditions.Add(ac);
    }

    public void addSelectiveActionCondition(ActionCondition ac)
    {
        selectiveActionConditions.Add(ac);
    }

    public void updateState()
    {
        if (this.state == SubTaskState.Hidden)
        {
            bool b = true;
            foreach (ActionCondition ac in requiredActionConditions)
            {
                if (ac.state != CondState.Ready) b = false;
            }

            bool bb = false;
            foreach (ActionCondition ac in selectiveActionConditions)
            {
                if (ac.state == CondState.Ready)
                {
                    bb = true;
                    ActiveOptionalCondition = ac.name;
                }
            }
            if (selectiveActionConditions.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = SubTaskState.Completed;
            }
        }
        else if (this.state == SubTaskState.Completed)
        {
            bool b = false;
            foreach (ActionCondition ac in requiredActionConditions)
            {
                if (ac.state == CondState.Greyed) b = true;
            }

            bool bb = false;
            ActionCondition activeAC = Services.actionConditionsManager.getActionConditionByName(this.ActiveOptionalCondition);
            if (activeAC != null)
            {
                if (activeAC.state == CondState.Greyed)
                {
                    bb = true;
                }
            }

            if (b || bb)
            {
                this.state = SubTaskState.Greyed;
            }
        }
        else if (this.state == SubTaskState.Greyed)
        {
            bool b = true;
            foreach (ActionCondition ac in requiredActionConditions)
            {
                if (ac.state != CondState.Ready) b = false;
            }

            bool bb = false;
            foreach (ActionCondition ac in selectiveActionConditions)
            {
                if (ac.state == CondState.Ready)
                {
                    bb = true;
                    ActiveOptionalCondition = ac.name;
                }
            }
            if (selectiveActionConditions.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = SubTaskState.Completed;
            }
        }
    }
}
