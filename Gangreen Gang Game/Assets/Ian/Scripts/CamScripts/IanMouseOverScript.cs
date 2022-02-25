using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanMouseOverScript : MonoBehaviour
{
    public static GameObject cam;
    Color BaseColor = Color.white;
    public Color MouseOverColor = Color.magenta;

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
        IanCamScript.Click();
    }
    private void OnMouseExit()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Renderer>().material.SetColor("_Color", BaseColor);
        }
        cam = null;
    }
}
