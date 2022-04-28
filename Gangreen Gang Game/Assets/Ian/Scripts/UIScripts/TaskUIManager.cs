using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public GameObject mainPanel;

    public int currentPuzzleIndex = 0;
    public int currentHighlightedPuzzle = 0;
    public List<GameObject> TaskGroups = new List<GameObject>();

    public bool Expanded;

    private bool switchingTask;

    public TMP_Text ClockTimeText;
    public TMP_Text RealTimeText;

    private void Awake()
    {
        Services.taskUIManager = this;
        Expanded = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPuzzleIndex = 0;
        currentHighlightedPuzzle = 0;

        //for (int i = 0; i < mainPanel.transform.childCount; i++)
        for (int i = 0; i < 4; i++)
        {
            TaskGroups.Add(mainPanel.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update the clock time
        ClockManager cm = Services.clockManager;
        Clock currentClock = cm.currentClock.GetComponent<Clock>();
        int hour = currentClock.myHour;
        int minute = currentClock.myMinute;
        ClockTimeText.text = "Clock time: " + hour.ToString() + ":" + ((minute > 9) ? minute.ToString() : ("0" + minute.ToString()));

        // update the real time
        TimeManager tm = Services.timeManager;
        int rHour = tm.hour;
        int rMinute = (int)tm.minute;
        RealTimeText.text = "Real time: " + rHour.ToString() + ":" + ((rMinute > 9) ? rMinute.ToString() : ("0" + rMinute.ToString()));
    }

    public void NextTask()
    {
        if (switchingTask) return;
        if (currentPuzzleIndex == TaskGroups.Count - 1) return;
        if (!Expanded)
        {
            ToggleUI();
            return;
        }
        Debug.Log("next task");
        currentPuzzleIndex++;

        ExpandTask(TaskGroups[currentPuzzleIndex]);
        CollapseTask(TaskGroups[currentPuzzleIndex - 1]);

        UpdateExpandedPos();

        switchingTask = true;
    }

    public void PrevTask()
    {
        if (switchingTask) return;
        if (currentPuzzleIndex == 0) return;
        if (!Expanded)
        {
            ToggleUI();
            return;
        }
        Debug.Log("previous task");
        currentPuzzleIndex--;

        ExpandTask(TaskGroups[currentPuzzleIndex]);
        CollapseTask(TaskGroups[currentPuzzleIndex + 1]);

        UpdateExpandedPos();

        switchingTask = true;
    }

    public void ExpandTask(GameObject Group)
    {
        Transform mainTask = Group.transform.GetChild(0);
        for (int i = 1; i < Group.transform.childCount; i++)
        {
            GameObject subTask = Group.transform.GetChild(i).gameObject;
            LeanTween.moveLocalY(subTask, subTask.transform.localPosition.y - 42 * i, 0.4f);
            Image img = subTask.GetComponent<Image>();
            LeanTween.value(subTask, a => img.color = a, img.color, new Color(img.color.r, img.color.g, img.color.b, 1), 0.4f).setOnComplete(SwitchReady);
            TMP_Text txt = subTask.transform.GetChild(0).GetComponent<TMP_Text>();
            LeanTween.value(txt.gameObject, a => txt.color = a, txt.color, new Color(txt.color.r, txt.color.g, txt.color.b, 1), 0.4f);
        }
    }

    public void CollapseTask(GameObject Group)
    {
        Transform mainTask = Group.transform.GetChild(0);
        for (int i = 1; i < Group.transform.childCount; i++)
        {
            GameObject subTask = Group.transform.GetChild(i).gameObject;
            LeanTween.moveLocalY(subTask, subTask.transform.localPosition.y + 42 * i, 0.4f);
            Image img = subTask.GetComponent<Image>();
            LeanTween.value(subTask, a => img.color = a, img.color, new Color(img.color.r, img.color.g, img.color.b, 0), 0.4f).setOnComplete(SwitchReady);
            TMP_Text txt = subTask.transform.GetChild(0).GetComponent<TMP_Text>();
            LeanTween.value(txt.gameObject, a => txt.color = a, txt.color, new Color(txt.color.r, txt.color.g, txt.color.b, 0), 0.4f);
        }
    }

    private void UpdateExpandedPos(float delay = 0f)
    {
        for (int i = 0; i < TaskGroups.Count; i++)
        {
            GameObject group = TaskGroups[i];
            float targetPos = 260;
            if (i <= currentPuzzleIndex) targetPos += -50f * i;
            else if (i > currentPuzzleIndex) targetPos += -50f * i - 16f - 42f * (TaskGroups[currentPuzzleIndex].transform.childCount - 1);
            LeanTween.moveLocalY(group, targetPos, 0.4f).setDelay(delay);
        }
    }

    private void SwitchReady()
    {
        switchingTask = false;
    }

    public void ToggleUI(float delay = 0f)
    {
        if (Expanded)
        {
            LeanTween.moveLocalY(mainPanel, 467f, 0.4f).setDelay(delay);
            LeanTween.moveLocalY(ClockTimeText.transform.parent.gameObject, 269.4f, 0.4f).setDelay(delay);
            LeanTween.moveLocalY(RealTimeText.transform.parent.gameObject, 269.4f, 0.4f).setDelay(delay);
            Expanded = false;

            for (int i = 0; i < TaskGroups.Count; i++)
            {
                GameObject group = TaskGroups[i];
                float targetPos = 72;
                if (i < currentPuzzleIndex) targetPos += 10 + 50f * (currentPuzzleIndex - i);
                else if (i > currentPuzzleIndex) targetPos += -100 - 50f * (i - currentPuzzleIndex);
                LeanTween.moveLocalY(group, targetPos, 0.4f).setDelay(delay);
            }
        }
        else
        {
            LeanTween.moveLocalY(mainPanel, 279f, 0.4f).setDelay(delay);
            LeanTween.moveLocalY(ClockTimeText.transform.parent.gameObject, 81.4f, 0.4f).setDelay(delay);
            LeanTween.moveLocalY(RealTimeText.transform.parent.gameObject, 81.4f, 0.4f).setDelay(delay);
            Expanded = true;

            UpdateExpandedPos(delay);
        }
    }

    public void GreyOut(GameObject button)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, Color.grey, 0.4f);
    }

    public void Appear(GameObject button, string txt)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, Color.black, new Color(1f, 1f, 1f, 0f), 0.2f).setOnComplete(
            () =>
            {
                text.text = txt;
                LeanTween.value(text.gameObject, a => text.color = a, new Color(1f, 1f, 1f, 0f), new Color(0.3f, 0.7f, 0.3f, 1f), 0.4f);
            }
            );
    }

    public void TurnGreen(GameObject button)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, new Color(0.3f, 0.7f, 0.3f, 1f), 0.4f);
    }

    public void TurnGreen(GameObject button, float delay)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, new Color(0.3f, 0.7f, 0.3f, 1f), 0.4f).setDelay(delay);
    }

}
