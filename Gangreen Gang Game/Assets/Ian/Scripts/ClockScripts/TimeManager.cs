using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int day;
    public int hour;
    public float minute;

    public float deltaMinute;

    public bool skipping;
    private Vector2 storedDir;

    private float skipAccumulator;

    public GameObject directionalLight;

    private void Awake()
    {
        Services.timeManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        directionalLight.transform.eulerAngles = new Vector3(hour * 15 + minute * 0.25f - 110, -30, 0);
        if (skipping) return;

        //deltaMinute += Time.deltaTime;
        //if (deltaMinute > 1 || deltaMinute < 0) // <0 for when time gets rewinded
        {
            // do this so that when hour hand gets changed, we can just add to the deltaMinute variable to change time
            //int minutePassed = (int)deltaMinute;
            minute += Time.deltaTime;

            // do the conversion between day, hour, and minute
            int hourPassed = (int)(minute / 60);
            if (minute < 0)
            {
                minute = 60 + (minute % 60);
                hourPassed--;
            }
            else
            {
                minute %= 60;
            }
            hour += hourPassed;

            int dayPassed = (int)(hour / 24);
            if (hour < 0)
            {
                hour = 24 + (hour % 24);
                dayPassed--;
            }
            else
            {
                hour %= 24;
            }
            day += dayPassed;


            // reset the deltaMinute variable
            deltaMinute %= 1;
            if (deltaMinute < 0) deltaMinute = 1 + deltaMinute;
        }
    }

    public void Skip(Vector2 newDir)
    {
        newDir.x = ((int)(newDir.x * 100)) / 100f;
        newDir.y = ((int)(newDir.y * 100)) / 100f;
        //Debug.Log(newDir);
        if (storedDir == Vector2.zero)
        {
            storedDir = newDir;
            return;
        }

        if (newDir == Vector2.zero)
        {
            storedDir = Vector2.zero;
            skipping = false;
            return;
        }
        else
        {
            skipping = true;
        }

        float joystickAngle = Vector2.SignedAngle(newDir, storedDir);
        Debug.Log("angle: " + joystickAngle);
        //if (joystickAngle < 0) joystickAngle *= -1;
        //else if (joystickAngle > 0)
        //{

        //}
        int minutePassed = 0;
        skipAccumulator += joystickAngle / 6;
        if (Mathf.Abs(skipAccumulator) > 1)
        {
            minutePassed = (int)skipAccumulator;
            skipAccumulator -= minutePassed;
        }

        //int minutePassed = (int)(joystickAngle / 6);
        minute += minutePassed;

        int hourPassed = (int)(minute / 60);
        if (minute < 0)
        {
            minute = 60 + (minute % 60);
            hourPassed--;
        }
        else
        {
            minute %= 60;
        }
        hour += hourPassed;

        int dayPassed = (int)(hour / 24);
        if (hour < 0)
        {
            hour = 24 + (hour % 24);
            dayPassed--;
        }
        else
        {
            hour %= 24;
        }
        day += dayPassed;

        storedDir = newDir;
    }
}
