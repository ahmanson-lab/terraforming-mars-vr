﻿/************************************************************************************

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

public class RawInteraction2300 : MonoBehaviour
{
    protected Material oldHoverMat;
    public Material yellowMat;
    protected Material oldHoverMatRocket;
    protected Material oldHoverMatSatellite;
    protected Material oldHoverMatMoss;
    public Material oldHoverMatLivingPod;

    public Material outlineMaterial;
    public Material backIdle;
    public Material backACtive;
    public UnityEngine.UI.Text outText;



    //public GameObject cube;
    public bool hovering;
    public GameObject _stepwiseRocket;
    public GameObject _stepwiseMoss;
    public GameObject _stepwiseSatellite;
    public GameObject _stepwiseLivingPod;
    public GameObject stepwiseInstructionMenu;

    public GameObject _mossPanel;
    public GameObject _livingPodPanel;
    public GameObject _rocketPanel;
    public GameObject satellitePanel;

    [SerializeField] private GameObject _instructionMenuCanvas;
    [SerializeField] private bool _instructionMenuActive;
    [SerializeField] private float timeLeft;
    [SerializeField] private GameObject bButtonOverlay;

    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    [SerializeField] private Image _credits;

    private string selectedTag;

    private Conductor _livingPodConductor;
    private Conductor _satelliteConductor;
    private Conductor _mossConductor;
    private Conductor _rocketConductor;
    private Conductor _instructionMenuConductor;

    private bool panelActive;

    public GameObject rightHand;
    //private bool triggerPressed;
    bool bDownRight;
    bool _livingPodNotHovered;

    private GameObject _prevPanel;
    private GameObject _prevStepwise;
    private string _prevTag;

    Sprite sceneA_Hilite;
    Sprite sceneB_Hilite;
    Sprite credits_Hilite;

    Sprite sceneA_original;
    Sprite sceneB_original;
    Sprite credits_original;

    [SerializeField] private GameObject _mossIndicatorArrow;
    [SerializeField] private GameObject _livingPodArrow;
    [SerializeField] private GameObject _rocketArrow;
    [SerializeField] private GameObject _satelliteArrow;

    void Start()
    {
        _stepwiseRocket.SetActive(true);
        
        _livingPodConductor = _stepwiseLivingPod.GetComponent<Conductor>();
        _livingPodConductor.OnScorePrepared += HandleScorePrepared;

        _rocketConductor = _stepwiseRocket.GetComponent<Conductor>();
        _rocketConductor.OnScorePrepared += HandleScorePrepared;

        _satelliteConductor = _stepwiseSatellite.GetComponent<Conductor>();
        _satelliteConductor.OnScorePrepared += HandleScorePrepared;

        _mossConductor = _stepwiseMoss.GetComponent<Conductor>();
        _mossConductor.OnScorePrepared += HandleScorePrepared;

        _instructionMenuConductor = stepwiseInstructionMenu.GetComponent<Conductor>();
        _instructionMenuConductor.OnScorePrepared += HandleScorePrepared;

        _prevTag = "";

        //oldHoverMatControlCenter = GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material;
        oldHoverMatRocket = GameObject.Find("C2_El_Wall").GetComponent<Renderer>().material;
        oldHoverMatSatellite = GameObject.Find("C_Misk_Aerial").GetComponent<Renderer>().material;
        oldHoverMatMoss = GameObject.Find("MossPileBig").GetComponent<Renderer>().material;
        //oldHoverMatLivingPod = GameObject.Find("C_Out_Wall_3").GetComponent<Renderer>().material;

        sceneA_original = Resources.Load<Sprite>("scene-a");
        sceneB_original = Resources.Load<Sprite>("scene-b");
        credits_original = Resources.Load<Sprite>("scene-c");

        sceneA_Hilite = Resources.Load<Sprite>("scene-a-hilite");
        sceneB_Hilite = Resources.Load<Sprite>("scene-b-hilite");
        credits_Hilite = Resources.Load<Sprite>("scene-c-hilite");

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        timeLeft = 6f;

        _mainMenuActive = false;
        _instructionMenuActive = false;
        _livingPodNotHovered = false; 
    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        _instructionMenuConductor.NextStep();
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            bButtonOverlay.SetActive(false);
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A pressed!!");

            if (bButtonOverlay.activeInHierarchy)
            {
                bButtonOverlay.SetActive(false);
            }
            _mainMenuCanvas.gameObject.SetActive(!_mainMenuActive);
            _mainMenuActive = !_mainMenuActive;
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {

            Debug.Log("Interaction menu raw interaction B button pressed!!");
            if (bButtonOverlay.activeInHierarchy)
            {
                bButtonOverlay.SetActive(false);
            }
            _instructionMenuCanvas.SetActive(!_instructionMenuActive);
            _instructionMenuActive = !_instructionMenuActive;
            if (_instructionMenuActive) StartCoroutine(AutoStart());
        }
    }

    public void OnHoverEnter(Transform t)
    {
        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backACtive;
        }
        else
        {
            hovering = true;
            //set selectedTag to whatever the tag of selected GameObject is
            selectedTag = t.gameObject.tag;
            Debug.Log("hovered tag is: " + selectedTag);


            if (t.gameObject.tag == "rocket")
            {
                foreach (GameObject rocketship in GameObject.FindGameObjectsWithTag("rocket"))
                {
                    if (rocketship.name == "C2_El_Wall" || rocketship.name == "C_El_Plate")
                    {
                        rocketship.GetComponent<Renderer>().material = outlineMaterial;
                    }
                }
            }
            
            if (t.gameObject.tag == "satellite")
            {
                foreach(GameObject solarPanel in GameObject.FindGameObjectsWithTag("satellite"))
                {
                    if(solarPanel.name == "C_Misk_Aerial")
                    {
                        solarPanel.GetComponent<Renderer>().material = outlineMaterial;
                    }
                } 
            }

            if (t.gameObject.tag == "LivingPod")
            {
                foreach (GameObject livingpod in GameObject.FindGameObjectsWithTag("LivingPod"))
                {
                    if (livingpod.name == "C_Out_Wall_3")
                    {
                        livingpod.GetComponent<Renderer>().material = outlineMaterial;
                    }
                }
                _livingPodNotHovered = true;
            }

            if (t.gameObject.tag == "Moss")
            {
                foreach (GameObject moss in GameObject.FindGameObjectsWithTag("Moss"))
                {
                    if (moss.name == "MossPileBig")
                    {
                        moss.GetComponent<Renderer>().material = outlineMaterial;
                    }
                }
            }

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
            if (t.gameObject.tag == "rocket")
            {
                foreach (GameObject rocketship in GameObject.FindGameObjectsWithTag("rocket"))
                {
                    if (rocketship.name == "C2_El_Wall" || rocketship.name == "C_El_Plate")
                    {
                        rocketship.GetComponent<Renderer>().material = oldHoverMatRocket;
                    }
                }
            }
            //            GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = oldHoverMatMoss;
            //GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material = oldHoverMatSolarPanel;
            foreach (GameObject satellite in GameObject.FindGameObjectsWithTag("satellite"))
            {
                if (satellite.name == "C_Misk_Aerial")
                {
                    satellite.GetComponent<Renderer>().material = oldHoverMatSatellite;
                }
            }
            
            if(_livingPodNotHovered)
            {
                foreach (GameObject livingpod in GameObject.FindGameObjectsWithTag("LivingPod"))
                {
                    if (livingpod.name == "C_Out_Wall_3")
                    {
                        livingpod.GetComponent<Renderer>().material = oldHoverMatLivingPod;
                    }
                }
            }
            
            foreach (GameObject moss in GameObject.FindGameObjectsWithTag("Moss"))
            {
                if (moss.name == "MossPileBig")
                {
                    moss.GetComponent<Renderer>().material = oldHoverMatMoss;
                }
            }


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

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared:" + score);
        int n = score.sequences.Length;
        for (int i = 0; i < n; i++)
        {
            score.sequences[i].repeat = false;
        }

    }

    private void DeactivatePanel(string selectedTag)
    {
       
        if (panelActive && (!_prevTag.Equals(selectedTag) || _prevTag.Equals("")))
        {
            Debug.Log("deactivate panel called, selected tag: " + selectedTag + "prev tag: " + _prevTag);
            if(_prevPanel != null) 
                _prevPanel.SetActive(false);
            //_prevStepwise.SetActive(false);
            _prevTag = selectedTag;
        }
    }

    private IEnumerator DelayedResetAndNextStep()
    {
        if (selectedTag == "rocket")
        {
            Debug.Log("DelayedResetAndNextStep: Rocket");
            yield return 0;
            _rocketConductor.Reset();
            yield return 0;
            _rocketConductor.NextStep();
        } else if (selectedTag == "satellite")
        {
            Debug.Log("DelayedResetAndNextStep: Satellite");
            yield return 0;
            _satelliteConductor.Reset();
            yield return 0;
            _satelliteConductor.NextStep();
        } else if (selectedTag == "LivingPod")
        {
            Debug.Log("DelayedResetAndNextStep: LivingPod");
            yield return 0;
            _livingPodConductor.Reset();
            yield return 0;
            _livingPodConductor.NextStep();
        }
        else if (selectedTag == "Moss")
        {
            Debug.Log("DelayedResetAndNextStep: Gas Tank");
            yield return 0;
            _mossConductor.Reset();
            yield return 0;
            _mossConductor.NextStep();
        }
        else if (selectedTag == "InstructionMenu")
        {
            Debug.Log("DelayedResetAndNextStep: InstructionMenu");
            yield return 0;
            _instructionMenuConductor.Reset();
            yield return 0;
            _instructionMenuConductor.NextStep();
        }
    }

    public void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");

        Debug.Log("selected tag is: " + selectedTag);
        /*
        if (panelActive)
        {
            _conductor.NextStep();
        }
        */
        //if hovering == true


        if (hovering == true)
        {
            if(selectedTag == "exitButton")
            {

            }
            if (selectedTag == "rocket")
            {
                panelActive = true;
                if (!_rocketPanel.activeInHierarchy)
                {
                    Debug.Log("Rocket not active");
                    DeactivatePanel(selectedTag);
                    _rocketPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("rocket panel already active: next step");
                    _rocketConductor.NextStep();
                }

                _prevPanel = _rocketPanel;
                _prevStepwise = _stepwiseRocket;
                //_controlCenterArrow.SetActive(false);
                _rocketArrow.SetActive(false);
            }
            else if (selectedTag == "satellite")
            {
                panelActive = true;
                if (!satellitePanel.activeInHierarchy)
                {
                    Debug.Log("Satellite not active");
                    DeactivatePanel(selectedTag);
                    satellitePanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("satellite panel already active: next step");
                    _satelliteConductor.NextStep();
                }

                _prevPanel = satellitePanel;
                _prevStepwise = _stepwiseSatellite;
                //_controlCenterArrow.SetActive(false);
                _satelliteArrow.SetActive(false);
            }
            else if (selectedTag == "LivingPod")
            {
                panelActive = true;
                if (!_livingPodPanel.activeInHierarchy)
                {
                    Debug.Log("LivingPod not active");
                    DeactivatePanel(selectedTag);
                    _livingPodPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("LivingPod panel already active: next step");
                    _livingPodConductor.NextStep();
                }

                _prevPanel = _livingPodPanel;
                _prevStepwise = _stepwiseLivingPod;
                _livingPodArrow.SetActive(false);
            }
            else if (selectedTag == "Moss")
            {
                panelActive = true;
                if (!_mossPanel.activeInHierarchy)
                {
                    Debug.Log("Moss not active");
                    DeactivatePanel(selectedTag);
                    _mossPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("Moss panel already active: next step");
                    _mossConductor.NextStep();
                }

                _prevPanel = _mossPanel;
                _prevStepwise = _stepwiseMoss;
                _mossIndicatorArrow.SetActive(false);
                  
            }
            else if (selectedTag == "InstructionMenu")
            {
                panelActive = true;
                if (!_instructionMenuCanvas.activeInHierarchy)
                {
                    Debug.Log("Instruction menu not active");
                    DeactivatePanel(selectedTag);
                    _instructionMenuCanvas.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("Instruction Menu panel already active: next step");
                    _instructionMenuConductor.NextStep();
                }    
            }
            else if (selectedTag == "Scene1")
            {
                
                Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
                if (SceneManager.GetActiveScene().name != "Marsat2100")
                {
                    Debug.Log("Load scene 1!");
                    SceneManager.LoadScene("Marsat2100");
                }

            }
            else if (selectedTag == "Scene2")
            {
                t.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("scene-b-hilite") as Sprite;
                Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
                if (SceneManager.GetActiveScene().name != "Marsat2300")
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


            if(t.gameObject.name == "ExitButton")
            {
                if(t.parent.gameObject.tag == "MainMenu")
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
}