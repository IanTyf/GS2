using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSetup : MonoBehaviour
{
     AudioSource audioHandler1;
     AudioSource audioHandler2; 
    public AudioManagerScript AuidioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioHandler1 = AuidioManager.ringing1;
        audioHandler2 = AuidioManager.ringing2;

    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey(KeyCode.Q))
         {
             //audioHandler1.Stop();
             audioHandler1.Play();
         }

         if (Input.GetKey(KeyCode.E))
         {
             //audioHandler2.Stop();
             audioHandler2.Play();
         }
    }
}
