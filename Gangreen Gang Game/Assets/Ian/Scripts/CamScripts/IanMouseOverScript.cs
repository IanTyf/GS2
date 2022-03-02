using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanMouseOverScript : MonoBehaviour
{
    public static GameObject cam;
    Color BaseColor = Color.white;
    public Color MouseOverColor = Color.magenta;

    private IanCamTransition icm;

    private void Start()
    {
        icm = GameObject.Find("CameraManager").GetComponent<IanCamTransition>();
    }

    // Start is called before the first frame update
    private void OnMouseOver()
    {
        Debug.Log("hover");
        cam = transform.GetChild(0).gameObject; //the camera always needs to be the first child
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);
        }
        //Debug.Log(cam);
        //IanCamScript.Click();
        checkClick();
    }
    private void OnMouseExit()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Renderer>().material.SetColor("_Color", BaseColor);
        }
        cam = null;
    }

    private void checkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            icm.zoomIn(IanMouseOverScript.cam);
        }
    }
}
