using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventCustomer : MonoBehaviour
{
    public List<TimeAction> allActions = new List<TimeAction>();

    // actions that this gameobject can potentially take in the future
    public List<TimeAction> listeningActions = new List<TimeAction>();

    // actions that this object has taken, act like a stack
    public List<TimeAction> actionHistory = new List<TimeAction>();
    public List<string> listeningHistory = new List<string>();

    public ActionState actionState;



    private Animator anim;
    private ActionConditionsManager acm;
    private TimeAction currentTimeAction;
    private TimeManager tm;
    private Animator[] allChildAnimators;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        acm = GameObject.Find("ActionConditionsManager").GetComponent<ActionConditionsManager>();
        actionState = ActionState.Listening;
        tm = Services.timeManager;
        allChildAnimators = GetComponentsInChildren<Animator>();


        // add all the action events of this object, one action event per anim clip
        // initial idle state
        TimeAction initialTA = new TimeAction("init", "CustomerOutsideIdle");

        // puzzle #2
        ActionCondition[] walkInConds = { acm.P2customerWalksIn };
        TimeAction walkIn = new TimeAction("walkIn", "WalkIn", walkInConds);
        allActions.Add(walkIn);

        ActionCondition[] askForClockConds = { acm.P2customerAskForClock };
        TimeAction askForClock = new TimeAction("askForClock", "Talk", askForClockConds);
        allActions.Add(askForClock);

        ActionCondition[] wrongClockConds = { acm.P2wrongClock };
        TimeAction wrongClock = new TimeAction("wrongClock", "NotWhatIWant", wrongClockConds);
        allActions.Add(wrongClock);

        ActionCondition[] grandfatherClockConds = { acm.P2grandfatherClock };
        TimeAction grandfatherClock = new TimeAction("grandfatherClock", "GrandfatherClockReaction", grandfatherClockConds);
        allActions.Add(grandfatherClock);

        ActionCondition[] mantelClockConds = { acm.P2mantelClock };
        TimeAction mantelClock = new TimeAction("mantelClock", "MantelClockReaction", mantelClockConds);
        allActions.Add(mantelClock);

        ActionCondition[] cornerClockConds = { acm.P2cornerClock };
        TimeAction cornerClock = new TimeAction("cornerClock", "CornerClockReaction", cornerClockConds);
        allActions.Add(cornerClock);

        ActionCondition[] deskClockConds = { acm.P2deskClock };
        TimeAction deskClock = new TimeAction("deskClock", "DeskClockReaction", deskClockConds);
        allActions.Add(deskClock);

        ActionCondition[] triangleClockConds = { acm.P2triangleClock };
        TimeAction triangleClock = new TimeAction("triangleClock", "TriangleClockReaction", triangleClockConds);
        allActions.Add(triangleClock);

        ActionCondition[] failedToBuyConds = { acm.P2failed };
        TimeAction failedToBuy = new TimeAction("failedToBuy", "FailPuzzle", failedToBuyConds);
        allActions.Add(failedToBuy);

        ActionCondition[] goUpstairsConds = { acm.P2ringClocksUpstair };
        TimeAction goUpstairs = new TimeAction("goUpstairs", "GoUpstairs", goUpstairsConds);
        allActions.Add(goUpstairs);

        ActionCondition[] leaveShopConds = { acm.P2customerLeave };
        TimeAction leaveShop = new TimeAction("leaveShop", "Leave", leaveShopConds);
        allActions.Add(leaveShop);


        // initial listening actions
        currentTimeAction = initialTA;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // when rewinding
        if (tm.skipping)
        {
            // set all animator (including all children's)'s speedMult float to -1
            anim.SetFloat("speedMult", -1 * tm.rewindSpeed);
            setChildrenAnimatorSpeed(-1 * tm.rewindSpeed);

            if (actionState == ActionState.Listening)
            {
                anim.SetFloat("speedMult", 0);
                //setChildrenAnimatorSpeed(0);

                // look at the end of the history stack, when time hits its completionTime, play it and pop it from the stack
                if (actionHistory.Count != 0)
                {
                    TimeAction endOfStack = actionHistory[actionHistory.Count - 1];
                    float currentM = minutes(new Vector3(tm.day, tm.hour, tm.minute));
                    float completionM = minutes(endOfStack.completionTime);
                    if (currentM < completionM)
                    {
                        Debug.Log("replaying history: " + endOfStack.name);
                        playAnim(endOfStack, 1);
                        actionState = ActionState.Playing;
                        currentTimeAction = endOfStack;
                        actionHistory.RemoveAt(actionHistory.Count - 1);
                    }
                }
            }
        }



        // when going forward
        if (!tm.skipping)
        {
            // set all animator including children's speedMult float to 1
            if (tm.fastForwarding)
            {
                anim.SetFloat("speedMult", tm.fastForwardSpeed);
                setChildrenAnimatorSpeed(tm.fastForwardSpeed);
            }
            else
            {
                anim.SetFloat("speedMult", 1);
                setChildrenAnimatorSpeed(1);
            }

            // when we're at the end of a clip, action state will be set to listening by the setListeningActions function, and we start listening for the next action
            if (actionState == ActionState.Listening)
            {
                anim.SetFloat("speedMult", 0);
                //setChildrenAnimatorSpeed(0);

                // check if the currently listening time action has all the conditions met, if so, set the parameters and also clear the listeningActions so that looping master animation can work
                foreach (TimeAction ta in listeningActions)
                {
                    if (ta.checkConditions())
                    {
                        // playing the next animation
                        playAnim(ta, 0);
                        actionState = ActionState.Playing;
                        currentTimeAction = ta;
                        Debug.Log("playing animation clip: " + ta.name);
                    }
                }
            }

            /*
            bool playedNew = false;
            foreach (TimeAction ta in listeningActions)
            {
                if (ta.checkConditions())
                {
                    // playing the next animation
                    playAnim(ta, 0);
                    actionState = ActionState.Playing;
                    currentTimeAction = ta;
                    Debug.Log("playing animation clip: " + ta.name);
                    playedNew = true;
                }
            }
            if (playedNew) listeningActions.Clear();
            */
        }
    }

    // play a time action's animation clip, norm == 1 means playing from the back, norm == 0 means playing from the beginning
    private void playAnim(TimeAction ta, int norm)
    {
        string clipName = ta.clipName;
        anim.Play(clipName, 0, norm);
    }

    // called at the end of each animation
    // update the currently listening actions list, only do so when going forward
    // push the current action to the history list
    public void setListeningActions(string actions)
    {
        if (tm.skipping)
        {
            actionState = ActionState.Playing;

            //restore what it was listening
            if (listeningHistory.Count > 0)
            {
                string endOfStack = listeningHistory[listeningHistory.Count - 1];
                string[] st = endOfStack.Split(',');

                listeningActions.Clear();
                Debug.Log("cleared listening actions in setActionState");
                //Debug.Log(listeningHistory[0]);
                foreach (TimeAction ta in allActions)
                {
                    foreach (string a in st)
                    {
                        if (ta.name == a.Trim())
                        {
                            listeningActions.Add(ta);
                            Debug.Log("added new listening action from history: " + ta.name);
                        }
                    }
                }
                listeningHistory.RemoveAt(listeningHistory.Count - 1);
            }
            return;
        }

        string oldActions = "";
        foreach (TimeAction ta in listeningActions)
        {
            oldActions += ta.name;
            oldActions += ", ";
        }
        if (!oldActions.Equals(""))
        {
            if (listeningHistory.Count == 0 || (listeningHistory.Count > 0 && !oldActions.Equals(listeningHistory[listeningHistory.Count - 1])))
            {
                listeningHistory.Add(oldActions);
                Debug.Log("added " + oldActions + " to listening history");
            }
        }


        string[] strs = actions.Split(',');

        listeningActions.Clear();
        foreach (TimeAction ta in allActions)
        {
            foreach (string a in strs)
            {
                if (ta.name == a.Trim())
                {
                    listeningActions.Add(ta);
                    Debug.Log("added new listening action: " + ta.name);
                }
            }
        }

        // reset params
        actionState = ActionState.Listening;
        //Debug.Log(currentTimeAction.name);
        actionHistory.Add(currentTimeAction);
        Debug.Log("added " + currentTimeAction.name + " to history");


        currentTimeAction.completionTime = new Vector3(tm.day, tm.hour, tm.minute);
        currentTimeAction = null;
    }

    public void resetActionState()
    {
        if (tm.skipping)
        {
            actionState = ActionState.Playing;
        }
        else actionState = ActionState.Listening;
    }

    public void setActionState()
    {
        if (tm.skipping)
        {
            actionState = ActionState.Listening;

        }
        else actionState = ActionState.Playing;
    }

    public void showSubtitle(int id)
    {
        if (tm.skipping) return;

        Subtitles.playSubtitles(id);
    }

    public void PushPlayerOutSecondFloor()
    {
        if (Services.timeManager.skipping)
        {
            //Services.actionConditionsManager.beauWatch.cannotPossess = false;
            return;
        }

        if (Services.clockManager.currentClock.name == "wristWatch")
        {
            Services.clockManager.returnToPreviousClock();
        }

        //Services.actionConditionsManager.beauWatch.cannotPossess = true;
    }

    public void PushPlayerOutFirstFloor()
    {
        if (Services.timeManager.skipping)
        {
            //Services.actionConditionsManager.beauWatch.cannotPossess = false;
            return;
        }

        if (Services.clockManager.currentClock.name == "pocketWatch")
        {
            Services.clockManager.returnToPreviousClock();
        }

        //Services.actionConditionsManager.beauWatch.cannotPossess = true;
    }

    /*
    public void playChildAnim(string str)
    {
        string[] p = str.Split(',');
        string objName = p[0].Trim();
        string boolName = p[1].Trim();
        bool value = p[2].Trim().Equals("1");

        //Debug.Log(p[2].Trim());
        if (tm.skipping) value = !value;
        GameObject child = findChild(transform, objName);
        if (child == null) Debug.Log("Error: no child named " + objName + " is found");
        else
        {
            child.GetComponent<Animator>().SetBool(boolName, value);
        }

    }

    private GameObject findChild(Transform t, string childName)
    {
        if (t.gameObject.name == childName) return t.gameObject;

        if (t.childCount == 0) return null;
        else
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GameObject child = findChild(t.GetChild(i), childName);
                if (child != null) return child;
            }
            return null;
        }
    }
    */

    private float minutes(Vector3 t)
    {
        return t.x * 24 * 60 + t.y * 60 + t.z;
    }

    private void setChildrenAnimatorSpeed(float i)
    {
        foreach (Animator a in allChildAnimators)
        {
            a.SetFloat("speedMult", i);
        }
    }
}
