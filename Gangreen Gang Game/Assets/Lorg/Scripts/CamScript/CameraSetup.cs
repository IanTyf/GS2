using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSetup : MonoBehaviour
{
    public bool IsMainCamera = false;
    public Image tlImage;
    // Start is called before the first frame update
    void Start()
    {
        CameraScript.GetCamera(gameObject);
        
        if (IsMainCamera == false)
        {
            gameObject.SetActive(false);
        }
    }

}
