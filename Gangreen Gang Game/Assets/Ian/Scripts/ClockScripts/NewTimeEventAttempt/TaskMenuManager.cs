using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubTaskState { Hidden, Completed, Greyed }
public enum MainObjectiveState { Hidden, Visible, Completed, Greyed }

public class TaskMenuManager : MonoBehaviour
{
    public List<MainObjective> allMainObjectives = new List<MainObjective>();
    public List<SubTask> allSubTasks = new List<SubTask>();

    // declare all main objectives and their subtasks
    // puzzle #1
    public MainObjective p1;
    public SubTask p1GoToAlarmClock;
    public SubTask p1RingAlarmClockOnTime;
    public SubTask p1RingAlarmClockLate;

    //puzzle #2
    public MainObjective p2;
    public SubTask p2SellRightClock;
    public SubTask p2SellWrongClock;
    public SubTask p2KeepPocketWatch;

    public GameObject Puzzle1Main;
    public GameObject Puzzle1Sub1;
    public GameObject Puzzle1Sub2;

    public GameObject Puzzle2Main;
    public GameObject Puzzle2Sub1;
    public GameObject Puzzle2Sub2;

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
        p1GoToAlarmClock.displayBtn = Puzzle1Sub1;
        allSubTasks.Add(p1GoToAlarmClock);

        p1RingAlarmClockOnTime = new SubTask("p1RingAlarmClock", "Make sure she's on time");
        p1RingAlarmClockOnTime.displayBtn = Puzzle1Sub2;
        //p1RingAlarmClock.addSelectiveActionCondition(acm.P1ringAlarmClockEarly);
        p1RingAlarmClockOnTime.addRequiredActionCondition(acm.P1ringAlarmClockOnTime);
        //p1RingAlarmClock.addSelectiveActionCondition(acm.P1ringAlarmClockLate);
        allSubTasks.Add(p1RingAlarmClockOnTime);

        p1RingAlarmClockLate = new SubTask("p1RingAlarmClockLate", "not showing");
        p1RingAlarmClockLate.addRequiredActionCondition(acm.P1ringAlarmClockLate);
        allSubTasks.Add(p1RingAlarmClockLate);

        p1 = new MainObjective("p1", "WAKE UP NIECE");
        p1.addRequiredSubTask(p1GoToAlarmClock);
        p1.addSelectiveSubTask(p1RingAlarmClockOnTime);
        p1.addSelectiveSubTask(p1RingAlarmClockLate);
        p1.displayBtn = Puzzle1Main;
        allMainObjectives.Add(p1);


        // puzzle #2
        p2SellRightClock = new SubTask("p2SellRightClock", "Sell the right clock");
        p2SellRightClock.displayBtn = Puzzle2Sub1;
        p2SellRightClock.addRequiredActionCondition(acm.P2soldRightClock);
        allSubTasks.Add(p2SellRightClock);

        p2SellWrongClock = new SubTask("p2SellWrongClock", "not showing");
        p2SellWrongClock.addRequiredActionCondition(acm.P2soldWrongClock);
        allSubTasks.Add(p2SellWrongClock);

        p2KeepPocketWatch = new SubTask("p2KeepPocketWatch", "Keep Pocket Watch in the shop");
        p2KeepPocketWatch.displayBtn = Puzzle2Sub2;
        p2KeepPocketWatch.addRequiredActionCondition(acm.P2soldRightClock);
        p2KeepPocketWatch.addRequiredActionCondition(acm.P2customerLeave);
        allSubTasks.Add(p2KeepPocketWatch);

        p2 = new MainObjective("p2", "SELL A CLOCK TO CUSTOMER");
        p2.addSelectiveSubTask(p2SellRightClock);
        p2.addSelectiveSubTask(p2SellWrongClock);
        p2.addOptionalSubTask(p2KeepPocketWatch);
        p2.displayBtn = Puzzle2Main;
        allMainObjectives.Add(p2);


        p1.state = MainObjectiveState.Visible;
        p2.state = MainObjectiveState.Visible;
    }

    // Update is called once per frame
    void Update()
    {
        string debugMsg = "Objective progress: \n";
        foreach (MainObjective m in allMainObjectives)
        {
            m.updateState();
            debugMsg += "    " + m.displayText + " -- " + m.state + "\n";
            foreach (SubTask st in m.requiredSubTasks)
            {
                debugMsg += "        " + st.displayText + " -- " + st.state + "\n";
            }
            foreach (SubTask st in m.optionalSubTasks)
            {
                debugMsg += "        " + st.displayText + " -- " + st.state + "\n";
            }
            foreach (SubTask st in m.selectiveSubTasks)
            {
                debugMsg += "        " + st.displayText + " -- " + st.state + "\n";
            }
        }

        Debug.Log(debugMsg);
    }

    public SubTask getSubTaskByName(string name)
    {
        foreach (SubTask st in allSubTasks)
        {
            if (st.name == name) return st;
        }
        return null;
    }
}

public class MainObjective
{
    public List<SubTask> requiredSubTasks; // need to complete all
    public List<SubTask> optionalSubTasks; // need to complete NONE
    public List<SubTask> selectiveSubTasks; // need to complete at least one

    public string name;
    public string displayText;
    public MainObjectiveState state;
    public GameObject displayBtn;

    public string ActiveSelectiveSubTask;

    public MainObjective(string name, string displayText)
    {
        this.name = name;
        this.displayText = displayText;
        this.state = MainObjectiveState.Hidden;

        requiredSubTasks = new List<SubTask>();
        optionalSubTasks = new List<SubTask>();
        selectiveSubTasks = new List<SubTask>();
    }

    public void addRequiredSubTask(SubTask st)
    {
        requiredSubTasks.Add(st);
    }

    public void addOptionalSubTask(SubTask st)
    {
        optionalSubTasks.Add(st);
    }

    public void addSelectiveSubTask(SubTask st)
    {
        selectiveSubTasks.Add(st);
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
        foreach (SubTask st in selectiveSubTasks)
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

            bool bb = false;
            foreach (SubTask st in selectiveSubTasks)
            {
                if (st.state == SubTaskState.Completed)
                {
                    bb = true;
                    ActiveSelectiveSubTask = st.name;
                }
            }
            if (selectiveSubTasks.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = MainObjectiveState.Completed;
                if (this.displayBtn != null)
                    Services.taskUIManager.TurnGreen(this.displayBtn, 0.8f);
            }
        }
        else if (this.state == MainObjectiveState.Completed)
        {
            bool b = false;
            foreach (SubTask st in requiredSubTasks)
            {
                if (st.state == SubTaskState.Greyed) b = true;
            }

            bool bb = false;
            SubTask activeST = Services.taskMenuManager.getSubTaskByName(ActiveSelectiveSubTask);
            if (activeST != null)
            {
                if (activeST.state == SubTaskState.Greyed)
                {
                    bb = true;
                }
            }

            if (b || bb)
            {
                this.state = MainObjectiveState.Greyed;
                if (this.displayBtn != null)
                    Services.taskUIManager.GreyOut(this.displayBtn);
            }
        }
        else if (this.state == MainObjectiveState.Greyed)
        {
            bool b = true;
            foreach (SubTask st in requiredSubTasks)
            {
                if (st.state != SubTaskState.Completed) b = false;
            }

            bool bb = false;
            foreach (SubTask st in selectiveSubTasks)
            {
                if (st.state == SubTaskState.Completed)
                {
                    bb = true;
                    ActiveSelectiveSubTask = st.name;
                }
            }
            if (selectiveSubTasks.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = MainObjectiveState.Completed;
                if (this.displayBtn != null)
                    Services.taskUIManager.TurnGreen(this.displayBtn, 0.8f);
            }
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
    public string ActiveSelectiveCondition;
    public GameObject displayBtn;

    public SubTask(string name, string displayText)
    {
        this.name = name;
        this.displayText = displayText;
        this.state = SubTaskState.Hidden;
        this.ActiveSelectiveCondition = null;

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
                    ActiveSelectiveCondition = ac.name;
                }
            }
            if (selectiveActionConditions.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = SubTaskState.Completed;
                if (this.displayBtn != null)
                    Services.taskUIManager.Appear(this.displayBtn, this.displayText);
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
            ActionCondition activeAC = Services.actionConditionsManager.getActionConditionByName(this.ActiveSelectiveCondition);
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
                if (this.displayBtn != null)
                    Services.taskUIManager.GreyOut(this.displayBtn);
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
                    ActiveSelectiveCondition = ac.name;
                }
            }
            if (selectiveActionConditions.Count == 0) bb = true;

            if (b && bb)
            {
                this.state = SubTaskState.Completed;
                if (this.displayBtn != null)
                    Services.taskUIManager.TurnGreen(this.displayBtn);
            }
        }
    }
}
