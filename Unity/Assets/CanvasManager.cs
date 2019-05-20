using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas[] _listOfCanvas;
    [SerializeField] private int _canvasActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _listOfCanvas[_canvasActive].gameObject.SetActive(true);
    }
}
