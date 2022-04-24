using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public GameObject mainPanel;

    

    public bool Expanded;

    private void Awake()
    {
        Services.taskUIManager = this;
        Expanded = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUI()
    {
        if (Expanded) {
            LeanTween.moveLocalY(mainPanel, 536f, 0.4f);
            Expanded = false;
        }
        else
        {
            LeanTween.moveLocalY(mainPanel, 200f, 0.4f);
            Expanded = true;
        }
    }

    public void GreyOut(GameObject button)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, Color.grey, 0.4f);
    }

    public void Appear(GameObject button, string txt)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, Color.black, new Color(1f,1f,1f,0f), 0.2f).setOnComplete(
            () =>
            {
                text.text = txt;
                LeanTween.value(text.gameObject, a => text.color = a, new Color(1f, 1f, 1f, 0f), Color.green, 0.4f);
            }
            );
    }

    public void TurnGreen(GameObject button)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, Color.green, 0.4f);
    }

    public void TurnGreen(GameObject button, float delay)
    {
        TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
        LeanTween.value(text.gameObject, a => text.color = a, text.color, Color.green, 0.4f).setDelay(delay);
    }

}
