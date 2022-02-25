using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanCamSetup : MonoBehaviour
{
    public bool IsMainCamera = false;
    // Start is called before the first frame update
    void Start()
    {
        IanCamScript.GetCamera(gameObject);

        if (IsMainCamera == false)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
