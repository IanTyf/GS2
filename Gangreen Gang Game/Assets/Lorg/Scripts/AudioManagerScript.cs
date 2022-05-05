using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{

    //public AudioSource ringing1;
    //public AudioSource ringing2;
    //public AudioClip ringingSound1;
    //public AudioClip ringingSound2;

    public List<GameObject> leftRingAudios = new List<GameObject>();
    public List<GameObject> rightRingAudios = new List<GameObject>();

    private void Awake()
    {
        Services.audioManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        updateRingAudios();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
         {
             //audioHandler1.Stop();
             ringing1.PlayOneShot(ringingSound1);
         }

         if (Input.GetKeyDown(KeyCode.E))
         {
             //audioHandler2.Stop();
             ringing2.PlayOneShot(ringingSound2);
         }
         */

    }

    public void updateRingAudios()
    {
        leftRingAudios.Clear();
        rightRingAudios.Clear();
        updateRingAudios("left");
        updateRingAudios("right");
    }

    void updateRingAudios(string side) {
        if (side != "left" && side != "right") {
            Debug.Log("Warning: updateAudios should only take left or right as parameters");
            return;
        }

        //Transform sounds = CameraScript.cam.transform.parent.Find(side+"Sounds");
        Transform sounds = Services.clockManager.currentClock.transform.Find(side + "Sounds");
        for (int i = 0; i<sounds.childCount; i++) {
            GameObject audio = sounds.GetChild(i).gameObject;
            if (side == "left") leftRingAudios.Add(audio);
            else if (side == "right") rightRingAudios.Add(audio);
        }
    }

    public void playLeftAudio()
    {
        int randomNum = Random.Range(0, leftRingAudios.Count);
        AudioSource randomAudioSource = leftRingAudios[randomNum].GetComponent<AudioSource>();
        randomAudioSource.PlayOneShot(randomAudioSource.clip, randomAudioSource.volume);
    }

    public void playRightAudio()
    {
        int randomNum = Random.Range(0, rightRingAudios.Count);
        AudioSource randomAudioSource = rightRingAudios[randomNum].GetComponent<AudioSource>();
        randomAudioSource.PlayOneShot(randomAudioSource.clip, randomAudioSource.volume);
    }
}
