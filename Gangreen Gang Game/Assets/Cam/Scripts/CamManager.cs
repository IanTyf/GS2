using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject cameraOne;
    public GameObject cameraTwo;
    public GameObject cameraThree;
    public GameObject cameraFour;
    
    // Start is called before the first frame update
    void Start()
    {
        #region Assign Cameras
        mainCamera = GameObject.Find("Main Camera");
        cameraOne = GameObject.Find("Camera 1");
        cameraTwo = GameObject.Find("Camera 2");
        cameraThree = GameObject.Find("Camera 3");
        cameraFour = GameObject.Find("Camera 4");
        #endregion

        #region Set Cameras
        mainCamera.SetActive(true);
        cameraOne.SetActive(false);
        cameraTwo.SetActive(false);
        cameraThree.SetActive(false);
        cameraFour.SetActive(false);
        #endregion
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(MouseOver.boxTag == 0)
            {
                mainCamera.SetActive(true);
                cameraOne.SetActive(false);
                cameraTwo.SetActive(false);
                cameraThree.SetActive(false);
                cameraFour.SetActive(false);
            }
            if(MouseOver.boxTag == 1)
            {
                mainCamera.SetActive(false);
                cameraOne.SetActive(true);
                cameraTwo.SetActive(false);
                cameraThree.SetActive(false);
                cameraFour.SetActive(false);
            }
            if(MouseOver.boxTag == 2)
            {
                mainCamera.SetActive(false);
                cameraOne.SetActive(false);
                cameraTwo.SetActive(true);
                cameraThree.SetActive(false);
                cameraFour.SetActive(false);
            }
            if(MouseOver.boxTag == 3)
            {
                mainCamera.SetActive(false);
                cameraOne.SetActive(false);
                cameraTwo.SetActive(false);
                cameraThree.SetActive(true);
                cameraFour.SetActive(false);
            }
            if(MouseOver.boxTag == 4)
            {
                mainCamera.SetActive(false);
                cameraOne.SetActive(false);
                cameraTwo.SetActive(false);
                cameraThree.SetActive(false);
                cameraFour.SetActive(true);
            }
        }
    }
}
