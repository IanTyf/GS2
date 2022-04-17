using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelecter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Vector3 selectPosition;
    RectTransform buttonRect;
    float YAxis;
    RectTransform selectorRect;
    float Speed = 0.1f;
    public GameObject QuitButton;
    RectTransform QuitRect;
    public RectTransform optionsMenu;
    public GameObject minuteHand;

    void Start()
    {
        selectPosition = new Vector3(0, +30, 0);
        buttonRect = gameObject.GetComponent<RectTransform>();
        selectorRect = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        QuitRect = QuitButton.GetComponent<RectTransform>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        YAxis = buttonRect.anchoredPosition.y;
        if (eventData.selectedObject.name == "MusicSlider" || eventData.selectedObject.name == "SoundSlider")
        {
            selectorRect.gameObject.SetActive(true);
            LeanTween.move(buttonRect, new Vector3(+120f,YAxis,0f), Speed).setEaseOutQuad();
            Debug.Log(selectorRect);
            LeanTween.alpha(selectorRect, 1f, Speed);
        } else
        {
            LeanTween.move(buttonRect, new Vector3(+30f,YAxis,0f), Speed).setEaseOutQuad();
            LeanTween.alpha(selectorRect, 1f, Speed);           
        }

        if (eventData.selectedObject.name == "NewGame")
        {
            minuteHand.transform.localEulerAngles = new Vector3 (-180f,0f,58.155f);
        }
        if (eventData.selectedObject.name == "Options")
        {
            minuteHand.transform.localEulerAngles = new Vector3 (-180f,0f,90f);
        }
        if (eventData.selectedObject.name == "Quit")
        {
            minuteHand.transform.localEulerAngles = new Vector3 (-180f,0f,120f);
        }   
    }
    
    public void OnDeselect(BaseEventData eventData)
    {

        if (eventData.selectedObject.name == "MusicSlider" || eventData.selectedObject.name == "SoundSlider")
        {
            YAxis = buttonRect.anchoredPosition.y;
            LeanTween.move(buttonRect, new Vector3(60f,YAxis,0f), Speed);
            selectorRect.gameObject.SetActive(false);            
        } else
        {
            YAxis = buttonRect.anchoredPosition.y;
            LeanTween.move(buttonRect, new Vector3(0f,YAxis,0f), Speed);
        }
        
        LeanTween.alpha(selectorRect, 0f, Speed);
    }

    public Slider MusicSlider;
    public Button Quit;
    public bool menuOpened = false;
    Navigation startNav;

    public void OptionsClicked()
    {
        if (menuOpened == false) 
        {
            menuOpened = true;
            optionsMenu.gameObject.SetActive(true);
            Debug.Log("options got clicked");
            float xAxis = optionsMenu.anchoredPosition.x;
            LeanTween.move(optionsMenu, new Vector3(xAxis, -82f, 0f), Speed);
            LeanTween.alpha(optionsMenu, 1f, Speed);
            LeanTween.move(QuitRect, new Vector3(0f,-125f, 0f), Speed);

            startNav = GetComponent<Button>().navigation;
            Navigation nav = startNav;
            nav.selectOnDown = MusicSlider;
            GetComponent<Button>().navigation = nav;
        } else 
        {
            menuOpened = false;
            float xAxis = optionsMenu.anchoredPosition.x;
            LeanTween.move(optionsMenu, new Vector3 (xAxis, -55f, 0f), Speed);
            LeanTween.alpha(optionsMenu, 0f, Speed);
            LeanTween.move(QuitRect, new Vector3(0f, -66.6f, 0f), Speed);

            Navigation nav = GetComponent<Button>().navigation;
            nav.selectOnDown = startNav.selectOnDown;
            Debug.Log(nav.selectOnDown);
            GetComponent<Button>().navigation = nav;
        }

    }

}
