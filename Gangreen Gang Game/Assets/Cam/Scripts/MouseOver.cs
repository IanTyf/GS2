using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    public int boxId;

    public static int boxTag;

    public Color startColor = Color.white;
    public Color mouseOverColor = Color.magenta;

    public GameObject camera;

    //GameObject myManager;

    public void Start()
    {
        //myManager = GameObject.Find("CamManager");
    }

    private void OnMouseOver()
    {
        //Debug.Log("Wow look at me!");

        camera = GameObject.Find("Main Camera"); //fetches the Main Camera and assings it to the cube as a gameobject.

        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor); //changes the color of the cube.

        boxTag = boxId;

        
    }

    private void OnMouseExit()
    {
        Debug.Log("Wow don't look at me!");

        GetComponent<Renderer>().material.SetColor("_Color", startColor);   //returns the cube to default color. 
    }

}
