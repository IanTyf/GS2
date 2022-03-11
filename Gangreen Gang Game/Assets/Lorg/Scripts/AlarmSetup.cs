using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSetup : MonoBehaviour
{
     AudioSource audioHandler1;
     AudioSource audioHandler2; 
    public AudioClip ringingSound1;
    public AudioClip ringingSound2;
    public AudioManagerScript AuidioManager;

    // Start is called before the first frame update
    void Start()
    {
        //audioHandler1 = AuidioManager.ringing1;
        //audioHandler2 = AuidioManager.ringing2;

    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey(KeyCode.Q))
         {
             //audioHandler1.Stop();
             audioHandler1.PlayOneShot(ringingSound1);
         }

         if (Input.GetKey(KeyCode.E))
         {
             //audioHandler2.Stop();
             audioHandler2.PlayOneShot(ringingSound2);
         }
    }
}
