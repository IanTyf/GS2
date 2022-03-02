using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanCamTransition : MonoBehaviour
{
    public GameObject currentCam;
    public GameObject targetCam;
    public float zoomSpeed;
    public float zoomAccel;
    public Animator anim;

    private bool zooming;
    public Vector3 oldPos;
    private float tempZoomSpeed;

    //temporary hard code thingy for presentation
    public GameObject firstVase;
    public GameObject secondVase;
    public int counter;

    // Start is called before the first frame update
    void Start()
    {
        zooming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCam == null && Camera.current != null)
        {
            currentCam = Camera.current.gameObject;
            //oldPos = currentCam.transform.localPosition;
        }
        if (zooming)
        {
            currentCam.transform.Translate((targetCam.transform.position - currentCam.transform.position).normalized * tempZoomSpeed * Time.deltaTime, Space.World);
            tempZoomSpeed += Time.deltaTime * zoomAccel;

            if (Vector3.Distance(targetCam.transform.position, currentCam.transform.position) < 1f)
            {
                IanCamScript.SwitchToCam(targetCam);
                currentCam.transform.localPosition = oldPos;
                zooming = false;
                currentCam = null;
                targetCam = null;
                
                //temp stuff
                firstVase.SetActive(true);
                counter++;
                if (counter == 2) secondVase.SetActive(true);
            }
        }
    }

    public void zoomIn(GameObject to)
    {
        targetCam = to;
        oldPos = currentCam.transform.localPosition;
        //zoomSpeed = 28f / (80f / (Vector3.Distance(targetCam.transform.position, currentCam.transform.position)));
        tempZoomSpeed = zoomSpeed;
        zooming = true;
        if (anim != null)
        {
            anim.SetTrigger("StartFade");
            anim.speed = 65f / (Vector3.Distance(targetCam.transform.position, currentCam.transform.position));
        }

        Debug.Log(Vector3.Distance(targetCam.transform.position, currentCam.transform.position));
    }
}
