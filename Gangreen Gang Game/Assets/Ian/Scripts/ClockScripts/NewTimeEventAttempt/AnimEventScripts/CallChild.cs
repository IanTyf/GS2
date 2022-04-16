using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallChild : MonoBehaviour
{
    //float timer = 0;
    private void Update()
    {
        /*
        timer += Time.deltaTime;
        if (timer > 2)
        {
            transform.GetComponent<Animator>().SetFloat("speedMult", -1);
        }

        if (timer > 3)
        {
            //Debug.Log("reversing");
            //transform.GetComponent<Animator>().SetFloat("speedMult", -1);
            transform.GetComponent<Animator>().Play("testCubeMove", 0 ,1);
            timer = -100;
        }
        */
    }

    public void playAnim(string str)
    {
        string[] p = str.Split(',');
        string objName = p[0].Trim();
        string boolName = p[1].Trim();
        bool value = p[2].Trim().Equals("1");

        Debug.Log(p[2].Trim());
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
