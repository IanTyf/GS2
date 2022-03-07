using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{

    public AudioSource ringing1;
    public AudioSource ringing2;

    // Start is called before the first frame update
    void Awake()
    {
         ringing1 = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
         ringing2 = gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
