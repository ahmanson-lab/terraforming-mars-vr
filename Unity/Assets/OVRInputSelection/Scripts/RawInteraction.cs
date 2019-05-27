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

public class RawInteraction : MonoBehaviour
{
    protected Material oldHoverMat;
    public Material yellowMat;
    protected Material oldHoverMatOuter;
    protected Material oldHoverMatInner;
    protected Material oldHoverMatSatellite;
    protected Material oldHoverMatSolarPanel;
    protected Material oldHoverMatHabitatPod;

    public Material outlineMaterial;
    public Material backIdle;
    public Material backACtive;
    public UnityEngine.UI.Text outText;



    //public GameObject cube;
    public bool hovering;
    public GameObject stepwiseAgroPod;
    public GameObject stepwiseSatellite;
    public GameObject stepwiseSolarPanel;
    public GameObject stepwiseHabitatPod;
    public GameObject stepwiseInstructionMenu;

    public GameObject agroPodPanel;
    public GameObject astronautPanel;
    public GameObject satellitePanel;
    public GameObject solarPanel_Panel;
    public GameObject habitatPod_Panel;

    [SerializeField] private GameObject _instructionMenuCanvas;
    [SerializeField] private bool _instructionMenuActive;
    [SerializeField] private float timeLeft;
    [SerializeField] private GameObject bButtonOverlay;

    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    [SerializeField] private Image _credits;

    public string selectedTag;
    
    private Conductor _agroPodConductor;
    private Conductor _satelliteConductor;
    private Conductor _solarPanelConductor;
    private Conductor _habitatPodConductor;
    private Conductor _instructionMenuConductor;
    private bool panelActive;

    public GameObject rightHand;
    //private bool triggerPressed;
    bool bDownRight;
    

    public Camera auxCamera;
    private float speed;
    private float x;
    private float y;

    private GameObject _prevPanel;
    private GameObject _prevStepwise;
    private string _prevTag;

    Sprite sceneA_Hilite;
    Sprite sceneB_Hilite;
    Sprite credits_Hilite;

    Sprite sceneA_original;
    Sprite sceneB_original;
    Sprite credits_original;

    [SerializeField] private GameObject _agroPodArrow;
    [SerializeField] private GameObject _solarPanelArrow;
    [SerializeField] private GameObject _satelliteArrow;
    [SerializeField] private GameObject _habitatPodArrow;

    void Start()
    {
       // stepwiseControlCenter.SetActive(true);
        stepwiseAgroPod.SetActive(true);
        
        _agroPodConductor = stepwiseAgroPod.GetComponent<Conductor>();
        _agroPodConductor.OnScorePrepared += HandleScorePrepared;

        _satelliteConductor = stepwiseSatellite.GetComponent<Conductor>();
        _satelliteConductor.OnScorePrepared += HandleScorePrepared;

        _solarPanelConductor = stepwiseSolarPanel.GetComponent<Conductor>();
        _solarPanelConductor.OnScorePrepared += HandleScorePrepared;

        _habitatPodConductor = stepwiseHabitatPod.GetComponent<Conductor>();
        _habitatPodConductor.OnScorePrepared += HandleScorePrepared;

        _instructionMenuConductor = stepwiseInstructionMenu.GetComponent<Conductor>();
        _instructionMenuConductor.OnScorePrepared += HandleScorePrepared;

        _prevTag = "";

        oldHoverMatOuter = GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material;
        oldHoverMatInner = GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material;
        
        oldHoverMatSatellite = GameObject.Find("Satelite_plate").GetComponent<Renderer>().material;
        oldHoverMatSolarPanel = GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material;
        oldHoverMatHabitatPod = GameObject.Find("Base_right").GetComponent<Renderer>().material;

        sceneA_original = Resources.Load<Sprite>("scene-a");
        sceneB_original = Resources.Load<Sprite>("scene-b");
        credits_original = Resources.Load<Sprite>("scene-c");

        sceneA_Hilite = Resources.Load<Sprite>("scene-a-hilite");
        sceneB_Hilite = Resources.Load<Sprite>("scene-b-hilite");
        credits_Hilite = Resources.Load<Sprite>("scene-c-hilite");

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        speed = -2f;
        x = 17;
        y = 0;
        timeLeft = 6f;

        _mainMenuActive = false;
        _instructionMenuActive = false;
    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        _instructionMenuConductor.NextStep();
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            bButtonOverlay.SetActive(false);
        }

        y += speed * Time.deltaTime;
       //auxCamera.transform.Rotate(0, speed * Time.deltaTime, 0);
       auxCamera.transform.rotation = Quaternion.Euler(x, y, 0);

        if(OVRInput.GetDown(OVRInput.Button.One))
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
            if(_instructionMenuActive) StartCoroutine(AutoStart());
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

            //oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            //t.gameObject.GetComponent<Renderer>().material = yellowMat;
            if(t.gameObject.tag == "agroPod")
            {
                
                GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = outlineMaterial;
                GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = outlineMaterial;
            }

            if (t.gameObject.tag == "satellite")
            {
                GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = outlineMaterial;
            }

            if (t.gameObject.tag == "solarPanel")
            {
                foreach(GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
                {
                    if(solarPanel.name == "Solar_panel_panel")
                    {
                        solarPanel.GetComponent<Renderer>().material = outlineMaterial;
                    }
                } 
            }
          
            if(t.gameObject.tag == "HabitatPod")
            {
                GameObject.Find("Base_right").GetComponent<Renderer>().material = outlineMaterial;
            }
        

            /* if(t.gameObject.tag == "controlCenter")
             {
                 GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material = outlineMaterial;
             }*/

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

            //t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = oldHoverMatInner;
            GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = oldHoverMatOuter;
            
            GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = oldHoverMatSatellite;
            //GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material = oldHoverMatSolarPanel;
            foreach (GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
            {
                if (solarPanel.name == "Solar_panel_panel")
                {
                    solarPanel.GetComponent<Renderer>().material = oldHoverMatSolarPanel;
                }
            }

            GameObject.Find("Base_right").GetComponent<Renderer>().material = oldHoverMatHabitatPod;

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
        if (selectedTag == "agroPod")
        {
            Debug.Log("DelayedResetAndNextStep: Agropod");
            yield return 0;
            _agroPodConductor.Reset();
            yield return 0;
            _agroPodConductor.NextStep();
        } else if (selectedTag == "satellite")
        {
            Debug.Log("DelayedResetAndNextStep: Satellite");
            yield return 0;
            _satelliteConductor.Reset();
            yield return 0;
            _satelliteConductor.NextStep();
        } else if (selectedTag == "solarPanel")
        {
            Debug.Log("DelayedResetAndNextStep: SolarPanel");
            yield return 0;
            _solarPanelConductor.Reset();
            yield return 0;
            _solarPanelConductor.NextStep();
        }
        else if (selectedTag == "HabitatPod")
        {
            Debug.Log("DelayedResetAndNextStep: HabitatPod");
            yield return 0;
            _habitatPodConductor.Reset();
            yield return 0;
            _habitatPodConductor.NextStep();
        }
        else if(selectedTag == "InstructionMenu")
        {
            Debug.Log("DelayedResetAndNextStep: InstructionMenu");
            yield return 0;
            _instructionMenuConductor.Reset();
            yield return 0;
            _instructionMenuConductor.NextStep();
        }
    }

    private IEnumerator DelayedReset()
    {
   
        if (selectedTag == "agroPod")
        {
            yield return 0;
            _agroPodConductor.Reset();
          
        }
        /*else if (selectedTag == "controlCenter")
        {
            yield return 0;
            _controlCenterConductor.Reset();
          
        }*/
    }

    private IEnumerator DelayedNextStep()
    {

        if (selectedTag == "agroPod")
        {
            yield return 0;
            _agroPodConductor.NextStep();

        }
       /* else if (selectedTag == "controlCenter")
        {
            yield return 0;
            _controlCenterConductor.NextStep();

        }*/
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
            //activate the stepwise panel for selected object
            if (selectedTag == "agroPod")
            {
                //stepwiseAgroPod.SetActive(true);
                //_conductor = stepwiseAgroPod.GetComponent<Conductor>();

                //Pseudocode
                /*if (panel isn’t active) {
                    make panel active
                    DelayedReset
                }
                DelayedNextStep
                */
                panelActive = true;
                if (!agroPodPanel.activeInHierarchy)
                {
                    Debug.Log("Not active");
                    DeactivatePanel(selectedTag);
                    agroPodPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("Agro pod panel already active: next step");
                    _agroPodConductor.NextStep();
                }


                _prevPanel = agroPodPanel;
                _prevStepwise = stepwiseAgroPod;
                _agroPodArrow.SetActive(false);

                //child the panel to the right controller
                //agroPodPanel.transform.SetParent(rightHand.transform);

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
                _prevStepwise = stepwiseSatellite;
                //_controlCenterArrow.SetActive(false);
                _satelliteArrow.SetActive(false);
            }
            else if (selectedTag == "solarPanel")
            {
                panelActive = true;
                if (!solarPanel_Panel.activeInHierarchy)
                {
                    Debug.Log("SolarPanel not active");
                    DeactivatePanel(selectedTag);
                    solarPanel_Panel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("solarPanel panel already active: next step");
                    _solarPanelConductor.NextStep();
                }

                _prevPanel = solarPanel_Panel;
                _prevStepwise = stepwiseSolarPanel;
                _solarPanelArrow.SetActive(false);
            }
            else if (selectedTag == "astronaut")
            {
                panelActive = true;
                //cube.SetActive(true);
                //activate the astronaut stepwise panel
                //stepwise.SetActive(true);
                //astronautPanel.SetActive(true);
                //astronautPanel.transform.SetParent(rightHand.transform);
            }
            else if (selectedTag == "HabitatPod")
            {
                panelActive = true;
                if (!habitatPod_Panel.activeInHierarchy)
                {
                    Debug.Log("habitatPod not active");
                    DeactivatePanel(selectedTag);
                    habitatPod_Panel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("habitatPod panel already active: next step");
                    _habitatPodConductor.NextStep();
                }

                _prevPanel = habitatPod_Panel;
                _prevStepwise = stepwiseHabitatPod;
                //Deactivate habitatPod arrow 
                //_solarPanelArrow.SetActive(false);
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

                _prevPanel = habitatPod_Panel;
                _prevStepwise = stepwiseHabitatPod;
                //Deactivate habitatPod arrow 
                //_solarPanelArrow.SetActive(false);
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

                if (t.parent.gameObject.tag == "InstructionMenu")
                {
                    _instructionMenuActive = !_instructionMenuActive;
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