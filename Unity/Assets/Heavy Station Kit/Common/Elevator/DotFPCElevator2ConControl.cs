using UnityEngine;
using System.Collections;


public class DotFPCElevator2ConControl : MonoBehaviour {

    public DotHskElevator2 elevator2;

    private DotFPCElevator2ConControlItems panelScript;
    private Canvas panelCanvas;
    private bool operate = false;

    void Start() {
        panelScript = GetComponentInChildren<DotFPCElevator2ConControlItems>();
        if (panelScript == null) {
            #if UNITY_EDITOR
            Debug.LogWarning("Panel canvas element script (DotFPCElevator2ConControlItems) not found!");
            #endif
        } else { 
            panelCanvas = panelScript.GetComponent<Canvas>();
            if (panelCanvas != null) {
                operate = true;
            } else {
                #if UNITY_EDITOR
                Debug.LogWarning("Panel canvas component not found!");
                #endif
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (operate && (other.gameObject.tag == "Player")) {
            panelCanvas.enabled = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (operate && (other.gameObject.tag == "Player")) {
            panelCanvas.enabled = false;
        }
    }

}
