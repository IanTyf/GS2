using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, ISelectHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(transform.name);
    }


    public void test()
    {
        Navigation nav = GetComponent<Button>().navigation;
        GetComponent<Button>().navigation = nav;
    }
}
