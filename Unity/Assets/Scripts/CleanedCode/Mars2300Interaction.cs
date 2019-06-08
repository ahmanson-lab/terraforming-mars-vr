using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opertoon.Stepwise;

public class Mars2300Interaction : RawInteraction
{
    [SerializeField] private Material outlineMaterial;

    protected Material oldHoverMatRocket;
    protected Material oldHoverMatSatellite;
    protected Material oldHoverMatMoss;
    [SerializeField] private Material oldHoverMatLivingPod; // Shown in editor and assign specific material, otherwise Unity can't find it:/ (idk why)

    [SerializeField] private GameObject _stepwiseRocket;
    [SerializeField] private GameObject _stepwiseMoss;
    [SerializeField] private GameObject _stepwiseSatellite;
    [SerializeField] private GameObject _stepwiseLivingPod;

    [SerializeField] private GameObject _mossPanel;
    [SerializeField] private GameObject _livingPodPanel;
    [SerializeField] private GameObject _rocketPanel;
    [SerializeField] private GameObject _satellitePanel;

    [SerializeField] private GameObject _mossIndicatorArrow;
    [SerializeField] private GameObject _livingPodArrow;
    [SerializeField] private GameObject _rocketArrow;
    [SerializeField] private GameObject _satelliteArrow;

    private string selectedTag;

    private Conductor _livingPodConductor;
    private Conductor _satelliteConductor;
    private Conductor _mossConductor;
    private Conductor _rocketConductor;
    private bool panelActive;

    private GameObject _prevPanel;
    private GameObject _prevStepwise;
    private string _prevTag;

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
            if (_prevPanel != null)
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
        }
        else if (selectedTag == "satellite")
        {
            Debug.Log("DelayedResetAndNextStep: Satellite");
            yield return 0;
            _satelliteConductor.Reset();
            yield return 0;
            _satelliteConductor.NextStep();
        }
        else if (selectedTag == "LivingPod")
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
    }

    // Start is called before the first frame update
    void Start()
    {
        _livingPodConductor = _stepwiseLivingPod.GetComponent<Conductor>();
        _livingPodConductor.OnScorePrepared += HandleScorePrepared;

        _rocketConductor = _stepwiseRocket.GetComponent<Conductor>();
        _rocketConductor.OnScorePrepared += HandleScorePrepared;

        _satelliteConductor = _stepwiseSatellite.GetComponent<Conductor>();
        _satelliteConductor.OnScorePrepared += HandleScorePrepared;

        _mossConductor = _stepwiseMoss.GetComponent<Conductor>();
        _mossConductor.OnScorePrepared += HandleScorePrepared;

        // Assign material found in assets folder
        oldHoverMatRocket = GameObject.Find("C2_El_Wall").GetComponent<Renderer>().material;
        oldHoverMatSatellite = GameObject.Find("C_Misk_Aerial").GetComponent<Renderer>().material;
        oldHoverMatMoss = GameObject.Find("MossPileBig").GetComponent<Renderer>().material;

        _prevTag = "";
    }

    public override void OnHoverEnter(Transform t)
    {
        Debug.Log("inside onhoverenter");
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
            foreach (GameObject solarPanel in GameObject.FindGameObjectsWithTag("satellite"))
            {
                if (solarPanel.name == "C_Misk_Aerial")
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
    }

    public override void OnHoverExit(Transform t)
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

        foreach (GameObject satellite in GameObject.FindGameObjectsWithTag("satellite"))
        {
            if (satellite.name == "C_Misk_Aerial")
            {
                satellite.GetComponent<Renderer>().material = oldHoverMatSatellite;
            }
        }
        
        foreach (GameObject livingpod in GameObject.FindGameObjectsWithTag("LivingPod"))
        {
            if (livingpod.name == "C_Out_Wall_3")
            {
                livingpod.GetComponent<Renderer>().material = oldHoverMatLivingPod;
            }
        }

        foreach (GameObject moss in GameObject.FindGameObjectsWithTag("Moss"))
        {
            if (moss.name == "MossPileBig")
            {
                moss.GetComponent<Renderer>().material = oldHoverMatMoss;
            }
        }
    }

    public override void OnSelected(Transform t)
    {
        Debug.Log("Mars2300: OnSelected");
        selectedTag = t.gameObject.tag;
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
            _rocketArrow.SetActive(false);
        }
        else if (selectedTag == "satellite")
        {
            panelActive = true;
            if (!_satellitePanel.activeInHierarchy)
            {
                Debug.Log("Satellite not active");
                DeactivatePanel(selectedTag);
                _satellitePanel.SetActive(true);
                StartCoroutine(DelayedResetAndNextStep());
            }
            else
            {
                Debug.Log("satellite panel already active: next step");
                _satelliteConductor.NextStep();
            }

            _prevPanel = _satellitePanel;
            _prevStepwise = _stepwiseSatellite;
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
    }
}
