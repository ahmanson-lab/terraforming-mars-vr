using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Opertoon.Stepwise;
using UnityEngine.UI;

public class IntroMainMenuInteraction : MainMenuRawInteraction
{
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    [SerializeField] private Image _credits;

    private Sprite sceneA_Hilite;
    private Sprite sceneB_Hilite;
    private Sprite credits_Hilite;

    private Sprite sceneA_original;
    private Sprite sceneB_original;
    private Sprite credits_original;

    private string selectedTag;
    private bool bDownRight;


    // Start is called before the first frame update
    void Start()
    {

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        _mainMenuActive = false;

        sceneA_original = Resources.Load<Sprite>("scene-a");
        sceneB_original = Resources.Load<Sprite>("scene-b");
        credits_original = Resources.Load<Sprite>("scene-c");

        sceneA_Hilite = Resources.Load<Sprite>("scene-a-hilite");
        sceneB_Hilite = Resources.Load<Sprite>("scene-b-hilite");
        credits_Hilite = Resources.Load<Sprite>("scene-c-hilite");

    }

    public override void OnHoverEnter(Transform t)
    {
        //set selectedTag to whatever the tag of selected GameObject is
        selectedTag = t.gameObject.tag;
        Debug.Log("hovered tag is: " + selectedTag);

        if (t.gameObject.tag == "Scene1")
        {
            _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_Hilite;
        }
        else if (t.gameObject.tag == "Scene2")
        {
            _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_Hilite;
        }
        else if (t.gameObject.tag == "Credits")
        {
            _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_Hilite;
        }
    }

    public override void OnHoverExit(Transform t)
    {
        _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_original;
        _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_original;
        _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_original;
    }

    public override void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");
        Debug.Log("selected tag is: " + selectedTag);

        if (selectedTag == "Scene1")
        {
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name != "DemoMarsScene")
            {
                Debug.Log("Load scene 1!");
                SceneManager.LoadScene("Marsat2100");
            }

        }
        else if (selectedTag == "Scene2")
        {
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name != "Scene2")
            {
                Debug.Log("Load scene 2!");
                SceneManager.LoadScene("Marsat2300");
            }

            //TODO: Add warning to show player is pressing on current scene!
        }
        else if (selectedTag == "Credits")
        {
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name != "Credits")
            {
                Debug.Log("Load credits!");
                SceneManager.LoadScene("CreditsScene");
            }

        }

        if (t.gameObject.name == "ExitButton")
        {
            if (t.parent.gameObject.tag == "MainMenu")
            {
                _mainMenuActive = !_mainMenuActive;
            }

            t.parent.gameObject.SetActive(false);
        }
    }
}
