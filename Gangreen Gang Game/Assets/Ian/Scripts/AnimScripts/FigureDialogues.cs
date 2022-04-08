using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureDialogues : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wakeupEarly()
    {

    }

    public void wakeupOnTime(string t)
    {
        Debug.Log("printing words, "+t);
        UI_Text.Write("Alright, Wakey Wakey.", 0.12f, true);
    }

    public void wakeupLate()
    {

    }
}
