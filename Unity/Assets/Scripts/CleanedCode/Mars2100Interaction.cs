using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opertoon.Stepwise;

public class Mars2100Interaction : MainMenuRawInteraction
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
    [SerializeField] private GameObject astronautPanel;
    [SerializeField] private GameObject satellitePanel;
    [SerializeField] private GameObject solarPanel_Panel;
    [SerializeField] private GameObject habitatPod_Panel;

    [SerializeField] private string selectedTag;

    [SerializeField] private Conductor _agroPodConductor;
    [SerializeField] private Conductor _satelliteConductor;
    [SerializeField] private Conductor _solarPanelConductor;
    [SerializeField] private Conductor _habitatPodConductor;

    [SerializeField] private Camera auxCamera;

    private bool panelActive;
    private float speed;
    private float x;
    private float y;

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

    public void OnHoverEnter(Transform t)
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

    public void OnHoverExit(Transform t)
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
}
