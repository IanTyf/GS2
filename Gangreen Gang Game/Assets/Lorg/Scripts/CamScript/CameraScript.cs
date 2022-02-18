using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
   public static List<GameObject> AllCameras = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GetCamera(GameObject camera)
    {
        AllCameras.Add(camera);
        //Debug.Log(AllCameras);
    }
    public static void Click()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            foreach (var Camera in AllCameras)
            {
                if (Camera != MouseOverScript.cam)
                {
                    Camera.SetActive(false);
                } else 
                {
                    Camera.SetActive(true);
                 Image uiImage = Camera.GetComponent<CameraSetup>().tlImage;
                 uiImage.rectTransform.sizeDelta = new Vector2(150, 150); 
                } 
            }
        }
    }
}
