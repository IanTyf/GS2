using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public List<GameObject> clocks = new List<GameObject>();
    public GameObject currentClock;

    // Start is called before the first frame update
    void Awake()
    {
        Services.clockManager = this;
    }

    private void Start()
    {
        currentClock.GetComponent<Clock>().hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextClock()
    {
        int currentInd = clocks.FindIndex((c)=> currentClock.name == c.name);
        switchToClock((currentInd + 1) % clocks.Count);
    }

    public void prevClock()
    {
        int currentInd = clocks.FindIndex((c) => currentClock.name == c.name);
        switchToClock((currentInd - 1 + clocks.Count) % clocks.Count);
    }

    public void switchToClock(int i)
    {
        if (i >= clocks.Count) return;
        GameObject newClock = clocks[i];
        currentClock.GetComponent<Clock>().possessed = false;
        currentClock.GetComponent<Clock>().showHidden();
        currentClock.transform.GetChild(0).gameObject.SetActive(false);

        currentClock = newClock;
        currentClock.GetComponent<Clock>().possessed = true;
        currentClock.GetComponent<Clock>().hide();
        currentClock.transform.GetChild(0).gameObject.SetActive(true);

    }
}
