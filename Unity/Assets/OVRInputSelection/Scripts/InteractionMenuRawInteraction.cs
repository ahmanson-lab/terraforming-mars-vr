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

public class InteractionMenuRawInteraction : MonoBehaviour
{
    
    public Material backIdle;
    public Material backActive;
    public UnityEngine.UI.Text outText;

    public bool hovering;

    [SerializeField] private Canvas _interactionMenuCanvas;
    [SerializeField] private bool _interactionMenuActive;
    
    public string selectedTag;

    private bool panelActive;

    public GameObject rightHand;

    private string _prevTag;

    void Start()
    {
        _prevTag = "";   
        hovering = false;
        _interactionMenuActive = false;
        
    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        

    }

    public void Update()
    {

        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("Interaction menu raw interaction B button pressed!!");
            _interactionMenuCanvas.gameObject.SetActive(!_interactionMenuActive);
            _interactionMenuActive = !_interactionMenuActive;
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
        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Enter:" + t.gameObject.name;
        }
    }

    public void OnHoverExit(Transform t)
    {

        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backIdle;
        }
        else
        {      
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

        if (t.gameObject.name == "ExitButton")
        {
            if (t.parent.gameObject.tag == "InteractionMenu")
            {
                _interactionMenuActive = !_interactionMenuActive;
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