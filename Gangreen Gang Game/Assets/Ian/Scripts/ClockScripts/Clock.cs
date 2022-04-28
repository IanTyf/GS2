using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public bool broken;
    private float brokenTimer;
    public float brokenMinute;
    public int brokenHour;
    public bool altControl;

    public GameObject minuteHand;
    public GameObject hourHand;

    public int myMinute;
    public int myHour;

    public float minuteOffset;
    public int hourOffset;

    private TimeManager tm;

    public bool possessed;

    public bool stopped;

    public List<AudioSource> tickSounds = new List<AudioSource>();

    public List<GameObject> hiddenObjs = new List<GameObject>();

    private void Start()
    {
        Services.clockManager.clocks.Add(this.gameObject);

        if (!possessed) transform.GetChild(0).gameObject.SetActive(false);

        tm = Services.timeManager;
        myMinute = (int)tm.minute;
        myHour = tm.hour;

        Transform ts = transform.Find("tickSounds");
        for (int i=0; i<ts.childCount; i++)
        {
            tickSounds.Add(ts.GetChild(i).gameObject.GetComponent<AudioSource>());
        }

        brokenTimer = -0.02f;
    }

    private void Update()
    {
        if (!Services.timeManager.skipping)
        {
            Vector3 tempRot = minuteHand.transform.localEulerAngles;
            if (broken)
            {
                myMinute = (int)(brokenMinute + minuteOffset);
                myHour = brokenHour + hourOffset;
                // update the minute and hour hands according to current time and offsets
                float minuteAngle = myMinute * -6;
                minuteHand.transform.localEulerAngles = new Vector3(0, 0, -minuteAngle * (possessed ? -1 : 1));

                float hourAngle = myHour * -30 + myMinute * -0.5f;
                hourHand.transform.localEulerAngles = new Vector3(0, 0, -hourAngle * (possessed ? -1 : 1));

                brokenMinute += Time.deltaTime;
                brokenTimer += Time.deltaTime;
                if (brokenTimer >= 1f)
                {
                    brokenTimer = 0;
                    minuteOffset -= 1f;
                }
            }
            else
            {
                if (!possessed)
                {
                    myMinute = (int)(tm.minute + minuteOffset);
                    myHour = tm.hour + hourOffset;
                    // update the minute and hour hands according to current time and offsets
                    float minuteAngle = myMinute * -6;
                    minuteHand.transform.localEulerAngles = new Vector3(0, 0, -minuteAngle);

                    float hourAngle = myHour * -30 + myMinute * -0.5f;
                    hourHand.transform.localEulerAngles = new Vector3(0, 0, -hourAngle);
                }
                else // possessing
                {
                    if (altControl)
                    {
                        myMinute = (int)(tm.minute + minuteOffset);
                        myHour = tm.hour + hourOffset;
                        // update the minute and hour hands according to current time and offsets
                        float minuteAngle = myMinute * -6;
                        minuteHand.transform.localEulerAngles = new Vector3(0, 0, -minuteAngle);

                        float hourAngle = myHour * -30 + myMinute * -0.5f;
                        hourHand.transform.localEulerAngles = new Vector3(0, 0, -hourAngle);

                        if (stopped)
                        {
                            minuteOffset -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        minuteOffset = myMinute - tm.minute;
                        hourOffset = myHour - tm.hour;

                        float minuteAngle = myMinute * -6;
                        minuteHand.transform.localEulerAngles = new Vector3(0, 0, minuteAngle);

                        float hourAngle = myHour * -30 + myMinute * -0.5f;
                        hourHand.transform.localEulerAngles = new Vector3(0, 0, hourAngle);
                    }
                }
            }


            // if the rotation of the minute hand changed, play a sound
            if (tempRot.z != minuteHand.transform.localEulerAngles.z && (minuteHand.transform.localEulerAngles.z + tempRot.z != 360f))
            {
                //Debug.Log("tempRot: " + tempRot.z + ", new rot: " + minuteHand.transform.localEulerAngles.z);
                PlayTickSound();
            }
        }
        else
        {
            if (!broken)
            {
                myMinute = (int)(tm.minute + minuteOffset);
                myHour = tm.hour + hourOffset;
                // update the minute and hour hands according to current time and offsets
                float minuteAngle = myMinute * -6;
               // if (possessed) minuteAngle *= -1;
                minuteHand.transform.localEulerAngles = new Vector3(0, 0, -minuteAngle);

                float hourAngle = myHour * -30 + myMinute * -0.5f;
               // if (possessed) hourAngle *= -1;
                hourHand.transform.localEulerAngles = new Vector3(0, 0, -hourAngle);
            }
        }

        //if (Services.inputManager.currentClock == this) possessed = true;
        //Debug.DrawRay(transform.GetChild(0).position, transform.forward, Color.red, 5f);
    }

    /*
    public void RotateHourHand(float degree)
    {
        RotateHand(hourHand, degree);
        RotateHand(minuteHand, degree * 12);
    }
    */
    public void RotateMinuteHand(int degree)
    {
        //RotateHand(minuteHand, degree);
        //RotateHand(hourHand, degree / 12f);
        if (altControl) minuteOffset += degree / 6;
        else myMinute += degree / 6;

        if (myMinute >= 60)
        {
            myHour += myMinute / 60;
            myMinute %= 60;
        }
        if (myHour >= 24)
        {
            myHour %= 24;
        }
    }

    public void StopMinuteHand()
    {
        stopped = true;
    }

    public void ResumeMinuteHand()
    {
        stopped = false;
        //minuteOffset = (float)(int)(minuteOffset);
        minuteOffset = (float)(int)(minuteOffset + tm.minute + 1) - tm.minute;
    }

    /*
    private void RotateHand(GameObject hand, float degree)
    {
        hand.transform.Rotate(0, 0, -degree, Space.Self);
    }
    */
    private void PlayTickSound()
    {
        int randNum = Random.Range(0, tickSounds.Count);
        AudioSource randSource = tickSounds[randNum];
        randSource.PlayOneShot(randSource.clip, randSource.volume);
        //randSource.Play();
    }


    public void hide()
    {
        Transform cam = transform.GetChild(0);
        RaycastHit[] hits = Physics.RaycastAll(cam.position, transform.forward, 1f);
        
        //Debug.Log("hit.count: " + hits.Length);
        //Debug.Log(cam.position);
        //Debug.Log(transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag != "Hand" && hit.transform.gameObject.tag != "Room")
            {
                hiddenObjs.Add(hit.transform.gameObject);
                hit.transform.gameObject.SetActive(false);
            }
        }
    }

    public void showHidden()
    {
        foreach (GameObject obj in hiddenObjs)
        {
            obj.SetActive(true);
        }
        hiddenObjs.Clear();
    }
}
