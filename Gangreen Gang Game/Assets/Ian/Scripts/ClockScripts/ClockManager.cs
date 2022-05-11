using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public List<GameObject> clocks = new List<GameObject>();
    public GameObject currentClock;

    public GameObject previousClock;

    public Vector2 inputDir;
    public GameObject highlightedClock;

    public bool highlight;
    public float highlightTimer;
    private float timer;

    private bool zooming;
    private float zoomSpeed;
    private Vector3 tempLocalPos;

    public Material normalMat;
    public Material highlightMat;

    public Color savedColor;

    //public LayerMask switchRayLM;

    // Start is called before the first frame update
    void Awake()
    {
        Services.clockManager = this;
    }

    private void Start()
    {
        currentClock.GetComponent<Clock>().hide();
        highlight = false;
        timer = 0;

        zooming = false;
        zoomSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (highlightedClock != null)
        {
            if (highlight && !zooming)
            {
                // highlight the highlightedClock here
                //highlightAClock(highlightedClock);

                timer += Time.deltaTime;
                if (timer > highlightTimer)
                {
                    //highlight = false;
                    timer = 0;
                    //revertHighlight(highlightedClock);
                }
            }
        }

        if (zooming)
        {
            GameObject currentCam = currentClock.transform.GetChild(0).gameObject;

            Vector3 zoomDir = (highlightedClock.transform.position - currentCam.transform.position).normalized;
            
            currentCam.transform.position = currentCam.transform.position + zoomDir * zoomSpeed * Time.deltaTime;
            zoomSpeed += 20f * Time.deltaTime;
            
            if (Vector3.Distance(currentCam.transform.position, highlightedClock.transform.position) < 1.7f)
            {
                zooming = false;
                switchToClock(highlightedClock);
            }
        }
    }

    /*
    public void nextClock()
    {
        int currentInd = clocks.FindIndex((c)=> currentClock.name == c.name);
        switchToClock((currentInd + 1) % clocks.Count);
    }

    public void prevClock()
    {
        int currentInd = clocks.FindIndex((c) => currentClock.name == c.name);
        switchToClock((currentInd - 1 + clocks.Count) % clocks.Count);
    }
    */

    public void GoToHighlightedClock()
    {
        if (highlightedClock == null) return;
        if (!highlightedClock.activeSelf) return;
        if (!highlight) return;
        if (zooming) return;
        if (highlightedClock.GetComponent<Clock>().cannotPossess) return;

        float zoomDist = (highlightedClock.transform.position - currentClock.transform.position).magnitude;
        //Debug.Log(zoomDist);
        zoomSpeed = 10f * zoomDist / 5f;

        zooming = true;
        tempLocalPos = currentClock.transform.GetChild(0).localPosition;
        //switchToClock(highlightedClock);
    }

    private void switchToClock(GameObject newClock)
    {
        //if (i >= clocks.Count) return;
        //GameObject newClock = clocks[i];
        previousClock = currentClock;
        currentClock.GetComponent<Clock>().possessed = false;
        currentClock.GetComponent<Clock>().showHidden();
        currentClock.GetComponent<Clock>().SetHandsTransparent(1.0f);
        currentClock.transform.GetChild(0).localPosition = tempLocalPos;
        currentClock.transform.GetChild(0).gameObject.SetActive(false);
        revertHighlight(newClock);

        currentClock = newClock;
        currentClock.GetComponent<Clock>().possessed = true;
        currentClock.GetComponent<Clock>().hide();
        currentClock.GetComponent<Clock>().SetHandsTransparent(0.3f);
        currentClock.transform.GetChild(0).gameObject.SetActive(true);

        // update the ring sounds in audio manager
        Services.audioManager.updateRingAudios();

        highlightedClock = null;
        highlight = false;
        timer = 0;
        zooming = false;
        zoomSpeed = 10;
        tempLocalPos = Vector3.zero;

    }

    public void returnToPreviousClock()
    {
        if (previousClock == null) return;
        if (zooming) return;

        highlightedClock = previousClock;
        highlight = true;
        GoToHighlightedClock();
    }

    // takes a vec2 and shoot a ray from the currently hightlighted clock and move to another one
    // based on similar concept with ray-marching
    public void handleInput(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            resetInputDir();
            return;
        }
        if (inputDir != Vector2.zero) return;

        inputDir = dir.normalized;
        //Debug.Log("Input direction: " + inputDir.x + ", " + inputDir.y);

        List<GameObject> visibleClocks = new List<GameObject>();
        Camera currentCam = currentClock.transform.GetChild(0).GetComponent<Camera>();
        //get all the clock in viewport
        foreach (GameObject clock in clocks)
        {
            if (IsInView(clock, currentCam))
            {
                if (clock != currentClock)
                {
                    visibleClocks.Add(clock);
                    //Debug.Log("visible clock: " + clock.name);
                }
            }
        }

        Vector2 origin = (highlightedClock == null) ? new Vector2(Screen.width / 2f, Screen.height / 2f) 
            : getScreenPos(currentCam, highlightedClock.transform.position);

        //sort them by distance to the highlighted clock on screenspace
        visibleClocks.Sort((a, b) => {
            float aDist = Vector2.Distance(origin, getScreenPos(currentCam, a.transform.position));
            float bDist = Vector2.Distance(origin, getScreenPos(currentCam, b.transform.position));
            if (aDist == bDist) return 0;
            else return (aDist > bDist) ? 1 : -1;
        });
            // debug result
        string temp = "";
        for (int i = 0; i < visibleClocks.Count; i++) {
            temp += visibleClocks[i].name + ", ";
        }
        //Debug.Log("Sorted list :" + temp);

        //do the raymarching-ish algorithm
        Vector2 newPoint = origin;
        GameObject targetClock = null;
        for (int i=0; i<visibleClocks.Count; i++)
        {
            newPoint = origin + inputDir * Vector2.Distance(origin, getScreenPos(currentCam, visibleClocks[i].transform.position));
            for (int j=0; j<visibleClocks.Count; j++)
            {
                if (Vector2.Distance(newPoint, origin) > Vector2.Distance(newPoint, getScreenPos(currentCam, visibleClocks[j].transform.position))) 
                {
                    if (targetClock == null) targetClock = visibleClocks[j];
                    else
                    {
                        float oldDist = Vector2.Distance(newPoint, getScreenPos(currentCam, targetClock.transform.position));
                        float newDist = Vector2.Distance(newPoint, getScreenPos(currentCam, visibleClocks[j].transform.position));
                        if (newDist < oldDist) targetClock = visibleClocks[j];
                    }
                }
            }
            if (targetClock != null) break;
        }

        // if we find a new clock to switch to
        if (targetClock != null)
        {
            //Debug.Log("can't find a valid clock");
            revertHighlight(highlightedClock);
            highlightedClock = targetClock;
            highlightAClock(highlightedClock);
            timer = 0;
            highlight = true;
        }
    }

    public void resetInputDir()
    {
        inputDir = Vector2.zero;
        //Debug.Log("reset");
    }

    private bool IsInView(GameObject toCheck, Camera cam)
    {
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            //Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            //Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        //Vector3 heading = toCheck.transform.position - origin.transform.position;
        //Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(cam.transform.position, toCheck.transform.position, out hit))
        {
            //Debug.Log("hit: "+hit.transform.name);
            //if (hit.transform.name != toCheck.name)
            if (!checkAncestorName(hit.transform, toCheck.name))
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                //Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }

        if (!toCheck.activeSelf) return false;
        return true;
    }

    private bool checkAncestorName(Transform t, string name)
    {
        if (t == null) return false;
        if (t.name == name) return true;
        return checkAncestorName(t.parent, name);
    }

    private Vector2 getScreenPos(Camera cam, Vector3 worldPos)
    {
        return new Vector2(cam.WorldToScreenPoint(worldPos).x, 
            cam.WorldToScreenPoint(worldPos).y);
    }

    private void highlightAClock(GameObject clock)
    {
        if (clock == null) return;

        //clock.transform.localScale = Vector3.one * 2;
        //clock.transform.localScale = clock.transform.localScale * 2;

        //clock.transform.GetChild(clock.transform.childCount - 1).gameObject.GetComponent<MeshRenderer>().material = highlightMat;

        Material mat = clock.transform.GetChild(clock.transform.childCount - 1).gameObject.GetComponent<MeshRenderer>().material;
        savedColor = mat.color;
        Color newCol = new Color(mat.color.r + 0.4f, mat.color.g + 0.4f, mat.color.b + 0.4f);
        clock.transform.GetChild(clock.transform.childCount - 1).gameObject.GetComponent<MeshRenderer>().material.color = newCol;
    }

    private void revertHighlight(GameObject clock)
    {
        if (clock == null) return;

        //clock.transform.localScale = Vector3.one * 1;
        //clock.transform.localScale = clock.transform.localScale / 2;

        //clock.transform.GetChild(clock.transform.childCount - 1).gameObject.GetComponent<MeshRenderer>().material = normalMat;
        clock.transform.GetChild(clock.transform.childCount - 1).gameObject.GetComponent<MeshRenderer>().material.color = savedColor;
    }
}
