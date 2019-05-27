/************************************************************************************

Copyright   :   Copyright 2017-Present Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using Opertoon.Stepwise;
using UnityEngine.UI;
using System.Collections;

public class MainMenuRawInteraction : MonoBehaviour
{
    
    public Material backIdle;
    public Material backActive;
    public UnityEngine.UI.Text outText;

    public bool hovering;

    //public GameObject controlCenterPanel;

    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    [SerializeField] private Image _credits;
    
    Sprite sceneA_Hilite;
    Sprite sceneB_Hilite;
    Sprite credits_Hilite;

    Sprite sceneA_original;
    Sprite sceneB_original;
    Sprite credits_original;
    
    public string selectedTag;

    private Conductor _roverConductor;
    private Conductor _gasTanksConductor;
    private Conductor _agroPodConductor;
    private Conductor _rocketConductor;
    private Conductor _satelliteConductor;
    private Conductor _solarPanelConductor;

    public GameObject stepwiseCredits;

    private Conductor _creditsConductor;


    private bool panelActive;

    public GameObject rightHand;
    //private bool triggerPressed;
    bool bDownRight;

    private string _prevTag;

    void Start()
    {
        _prevTag = "";

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        _mainMenuActive = false;
        
        sceneA_original = Resources.Load<Sprite>("scene-a");
        sceneB_original = Resources.Load<Sprite>("scene-b");
        credits_original = Resources.Load<Sprite>("scene-c");

        sceneA_Hilite = Resources.Load<Sprite>("scene-a-hilite");
        sceneB_Hilite = Resources.Load<Sprite>("scene-b-hilite");
        credits_Hilite = Resources.Load<Sprite>("scene-c-hilite");

        _creditsConductor = stepwiseCredits.GetComponent<Conductor>();
        _creditsConductor.OnScorePrepared += HandleScorePrepared;

        StartCoroutine(AutoStart());

    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        _creditsConductor.NextStep();
    }

    public void Update()
    {

        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("main menu raw interaction A pressed!!");
            _mainMenuCanvas.gameObject.SetActive(!_mainMenuActive);
            _mainMenuActive = !_mainMenuActive;
        }
    }

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared:" + score);
        int n = score.sequences.Length;
        for (int i = 0; i < n; i++)
        {
            score.sequences[i].repeat = false;
        }

    }

    public void OnHoverEnter(Transform t)
    {
        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backActive;
        }
        else
        {
            hovering = true;
            //set selectedTag to whatever the tag of selected GameObject is
            selectedTag = t.gameObject.tag;
            Debug.Log("hovered tag is: " + selectedTag);


            if (t.gameObject.tag == "Scene1")
            {
                _scene1.GetComponentInChildren<Image>().color = Color.yellow;
                _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_Hilite;
                
            }
            
            if(t.gameObject.tag == "Scene2")
            {
                _scene2.GetComponentInChildren<Image>().color = Color.yellow;
                _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_Hilite;
            }

            if (t.gameObject.tag == "Credits")
            {
                _credits.GetComponentInChildren<Image>().color = Color.yellow;
                _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_Hilite;
            }

            //set hovering bool = true;

        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Enter:" + t.gameObject.name;
        }
    }

    public void OnHoverExit(Transform t)
    {
        if(t.gameObject.name == "exitButton")
        {

        }

        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backIdle;
        }
        else
        {
            _scene2.GetComponentInChildren<Image>().color = Color.clear;
            _scene1.GetComponentInChildren<Image>().color = Color.clear;

            _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_original;
            _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_original;
            _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_original;

            //set hovering bool = false;
            hovering = false;
        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Exit:" + t.gameObject.name;
        }
    }

    public void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");

        Debug.Log("selected tag is: " + selectedTag);

        if (selectedTag == "CreditsPanel")
        {
          
                Debug.Log("Instruction Menu panel already active: next step");
                _creditsConductor.NextStep();
          
        }
        else if (selectedTag == "Scene1")
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
        } else if (selectedTag == "Credits")
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


        if (t.gameObject.name == "BackButton")
        {
            SceneManager.LoadScene("main", LoadSceneMode.Single);
        }

        Debug.Log("Clicked on " + t.gameObject.name);
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nClicked On:" + t.gameObject.name;
        }
    }

}