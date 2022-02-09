using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameScript : MonoBehaviour
{

    public Sprite FrameToShow;
    Sprite SourceImage;
    public GameObject FrameObject;

    // Start is called before the first frame update
    void Start()
    {
        FrameObject.GetComponent<Image>().sprite = FrameToShow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
