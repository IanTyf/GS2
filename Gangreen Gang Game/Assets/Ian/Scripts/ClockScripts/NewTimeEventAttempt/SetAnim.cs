using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnim : MonoBehaviour
{
    public string lastPlayedChildClip;
    public AudioSource phoneRing;

    public List<GameObject> disabledObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playBackCheck(string clipName)
    {
        /*
        string[] p = str.Split(',');
        string objName = p[0].Trim();
        string clipName = p[1].Trim();
        */
        clipName = clipName.Trim();

        if (!Services.timeManager.skipping)
        {
            lastPlayedChildClip = clipName;
            return;
        }

        if (lastPlayedChildClip == clipName) return;
        else
        {
            lastPlayedChildClip = clipName;
            /*
            GameObject child = findChild(transform, objName);
            if (child == null) Debug.Log("Error: no child named " + objName + " is found");
            else
            {
            */
            GetComponent<Animator>().Play(clipName, 0, 1);
            //}
        }
    }

    public void playBackReset(string str)
    {
        if (!Services.timeManager.skipping) return;

        string[] p = str.Split(',');
        string objName = p[0].Trim();
        string boolName = p[1].Trim();

        GameObject child = findChild(transform, objName);
        if (child == null) Debug.Log("Error: no child named " + objName + " is found");
        else
        {
            child.GetComponent<Animator>().SetBool(boolName, false);
        }
    }

    public void disableGameObject(string str)
    {
        if (!Services.timeManager.skipping)
        {
            GameObject go = GameObject.Find(str);
            disabledObjects.Add(go);
            go.SetActive(false);
        }
        else
        {
            foreach (GameObject go in disabledObjects)
            {
                go.SetActive(true);
                disabledObjects.Remove(go);
                break;
            }
        }
    }

    public void setAC(string name)
    {
        if (Services.timeManager.skipping) Services.actionConditionsManager.setActionConditionByName(name.Trim(), false);
        else Services.actionConditionsManager.setActionConditionByName(name.Trim());
    }

    public void playPhoneRing()
    {
        if (phoneRing != null)
        {
            if (Services.timeManager.skipping || Services.timeManager.fastForwarding) return;
            phoneRing.PlayOneShot(phoneRing.clip);
        }
    }

    public void playChildAnim(string str)
    {
        string[] p = str.Split(',');
        string objName = p[0].Trim();
        string boolName = p[1].Trim();
        bool value = p[2].Trim().Equals("1");

        //Debug.Log(p[2].Trim());
        if (Services.timeManager != null && Services.timeManager.skipping) value = !value;
        GameObject child = findChild(transform, objName);
        if (child == null) Debug.Log("Error: no child named " + objName + " is found");
        else
        {
            child.GetComponent<Animator>().SetBool(boolName, value);
        }

    }

    private GameObject findChild(Transform t, string childName)
    {
        if (t.gameObject.name == childName) return t.gameObject;

        if (t.childCount == 0) return null;
        else
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GameObject child = findChild(t.GetChild(i), childName);
                if (child != null) return child;
            }
            return null;
        }
    }
}
