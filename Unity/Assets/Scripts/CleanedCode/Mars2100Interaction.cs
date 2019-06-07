using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opertoon.Stepwise;

public class Mars2100Interaction : RawInteraction
{
    [SerializeField] private Material outlineMaterial;

    private Material oldHoverMatOuter;
    private Material oldHoverMatInner;
    private Material oldHoverMatSatellite;
    private Material oldHoverMatSolarPanel;
    private Material oldHoverMatHabitatPod;

    [SerializeField] private GameObject stepwiseAgroPod;
    [SerializeField] private GameObject stepwiseSatellite;
    [SerializeField] private GameObject stepwiseSolarPanel;
    [SerializeField] private GameObject stepwiseHabitatPod;

    [SerializeField] private GameObject agroPodPanel;
    [SerializeField] private GameObject satellitePanel;
    [SerializeField] private GameObject solarPanel_Panel;
    [SerializeField] private GameObject habitatPod_Panel;

    [SerializeField] private GameObject _agroPodArrow;
    [SerializeField] private GameObject _solarPanelArrow;
    [SerializeField] private GameObject _satelliteArrow;
    [SerializeField] private GameObject _habitatPodArrow;

    [SerializeField] private Camera auxCamera;

    private Conductor _agroPodConductor;
    private Conductor _satelliteConductor;
    private Conductor _solarPanelConductor;
    private Conductor _habitatPodConductor;

    private bool panelActive;
    private float speed;
    private float x;
    private float y;

    private GameObject _prevPanel;
    private GameObject _prevStepwise;
    private string _prevTag;
    private string selectedTag;

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
        }
        else if (selectedTag == "satellite")
        {
            Debug.Log("DelayedResetAndNextStep: Satellite");
            yield return 0;
            _satelliteConductor.Reset();
            yield return 0;
            _satelliteConductor.NextStep();
        }
        else if (selectedTag == "solarPanel")
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
    }

    void Start()
    {
        _agroPodConductor = stepwiseAgroPod.GetComponent<Conductor>();
        _agroPodConductor.OnScorePrepared += HandleScorePrepared;

        _satelliteConductor = stepwiseSatellite.GetComponent<Conductor>();
        _satelliteConductor.OnScorePrepared += HandleScorePrepared;

        _solarPanelConductor = stepwiseSolarPanel.GetComponent<Conductor>();
        _solarPanelConductor.OnScorePrepared += HandleScorePrepared;

        _habitatPodConductor = stepwiseHabitatPod.GetComponent<Conductor>();
        _habitatPodConductor.OnScorePrepared += HandleScorePrepared;

        _prevTag = "";

        oldHoverMatOuter = GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material;
        oldHoverMatInner = GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material;

        oldHoverMatSatellite = GameObject.Find("Satelite_plate").GetComponent<Renderer>().material;
        oldHoverMatSolarPanel = GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material;
        oldHoverMatHabitatPod = GameObject.Find("Base_right").GetComponent<Renderer>().material;

        panelActive = false;
        speed = -2f;
        x = 17;
        y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        y += speed * Time.deltaTime;
        auxCamera.transform.rotation = Quaternion.Euler(x, y, 0);
    }

    public override void OnHoverEnter(Transform t)
    {
        //set selectedTag to whatever the tag of selected GameObject is
        selectedTag = t.gameObject.tag;
        Debug.Log("hovered tag is: " + selectedTag);

        if (t.gameObject.tag == "agroPod")
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
            foreach (GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
            {
                if (solarPanel.name == "Solar_panel_panel")
                {
                    solarPanel.GetComponent<Renderer>().material = outlineMaterial;
                }
            }
        }

        if (t.gameObject.tag == "HabitatPod")
        {
            GameObject.Find("Base_right").GetComponent<Renderer>().material = outlineMaterial;
        }
    }

    public override void OnHoverExit(Transform t)
    {
        GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = oldHoverMatInner;
        GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = oldHoverMatOuter;
        GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = oldHoverMatSatellite;
        GameObject.Find("Base_right").GetComponent<Renderer>().material = oldHoverMatHabitatPod;

        foreach (GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
        {
            if (solarPanel.name == "Solar_panel_panel")
            {
                solarPanel.GetComponent<Renderer>().material = oldHoverMatSolarPanel;
            }
        }
    }

    public override void OnSelected(Transform t)
    {
        if (selectedTag == "agroPod")
        {
            panelActive = true;
            if (agroPodPanel.activeInHierarchy == false)
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
        }
        else if (selectedTag == "satellite")
        {
            panelActive = true;
            if (satellitePanel.activeInHierarchy == false)
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
            if (solarPanel_Panel.activeInHierarchy == false)
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
        else if (selectedTag == "HabitatPod")
        {
            panelActive = true;
            if (habitatPod_Panel.activeInHierarchy == false)
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
        }
    }

}
