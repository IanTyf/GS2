using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScript : MonoBehaviour
{
    public static GameObject cam;
    Color BaseColor = Color.white;
    public Color MouseOverColor = Color.magenta;

    // Start is called before the first frame update
    private void OnMouseOver()
    {
        cam = transform.GetChild(0).gameObject; //the camera always needs to be the first child
        GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);
        //Debug.Log(cam);
        CameraScript.Click();
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.SetColor("_Color", BaseColor);
        cam = null;
    }
}
