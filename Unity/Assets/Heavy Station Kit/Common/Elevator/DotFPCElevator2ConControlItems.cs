using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DotFPCElevator2ConControlItems : MonoBehaviour {

    public int visibleButtons = 5;
    public int buttonItemHeight = 36;

    public Button[] consoleButtons;
    public Transform buttonSlider;
    public Transform buttonFrame;
    public Text elevatorLabelText;
    public Color floorTargetColor = new Color(1f, 0.7f, 0f, 1f);
    public DotFPCElevator2ConControl controlScript;

    private Dictionary<int, Text> floorMarks;
    private Vector3 sliderBaseOrg = Vector3.zero;
    private float frameBaseOrgY = 0f;
    private Color regularFloorColor;

    private int curOffset = 0;
    private int newOffset = 0;
    private int dir = 0;

    private int floorCnt = 0;
    private int floorCur = -1;
    private int floorPre = -1;
    private int floorTarget = -1;
    private float toPos = 0f;
	private bool operate = false;

    void Start() {
		operate = false;
        if ( consoleButtons.Length == 0 ) {
            #if UNITY_EDITOR
            Debug.LogWarning("Control Buttons not found!");
            #endif
            return;
        }
        controlScript = transform.parent.GetComponentInChildren<DotFPCElevator2ConControl>();
        if ( (controlScript == null) || (controlScript.elevator2 == null) ) {
            #if UNITY_EDITOR
			Debug.LogWarning((controlScript == null) ? "DotFPCElevator2ConControl script not attached" : "Elevator2 not attached");
            #endif
            return;
        }
		operate = true;
        floorMarks = new Dictionary<int, Text>();
        elevatorLabelText.text = controlScript.elevator2.elevatorLabel;
        if ( buttonSlider != null ) {
            sliderBaseOrg = buttonSlider.localPosition;
        }
        // Up / Dn buttons
        for ( int i=1; (i<consoleButtons.Length) || (i<3); i++ ) { 
            if ( consoleButtons[i] != null ) {
                int ii = -i;
                consoleButtons[i].onClick.AddListener(delegate { buttonClick(ii); });
            }
        }
        // Floor buttons
        float pos = 0f;
        string name = "";
        floorCnt = controlScript.elevator2.floors.Count;
        frameBaseOrgY = consoleButtons[0].transform.localPosition.y;
        for ( int i=0; i < floorCnt; i++ ) {
            Button b;
            int ii = floorCnt - i - 1;
            if ( i == 0 ) {
                b = consoleButtons[0];
                pos = b.transform.localPosition.y;
                name = b.transform.name;
            } else {
                b = Instantiate<Button>( consoleButtons[0] );
				b.transform.SetParent ( consoleButtons[0].transform.parent );
				b.transform.localScale = consoleButtons[0].transform.localScale;
				b.transform.localRotation = consoleButtons[0].transform.localRotation;
				Vector3 tp = consoleButtons[0].transform.localPosition;
				tp.y = pos - i * buttonItemHeight;
				b.transform.localPosition = tp;
            }
            b.transform.name = name + "_" + i;
            b.onClick.AddListener( delegate { buttonClick(ii);} );
            Text[] btext = b.GetComponentsInChildren<Text>();
            if ( btext.Length > 1 ) {
				string s = controlScript.elevator2.floors[ii].floorTitle;
				string n = "" + controlScript.elevator2.floors[ii].floorNumber;
                btext[0].text = n;
				btext[1].text = (s == "") ? "(Floor #"+n+")": s;
                if (i == 0) { regularFloorColor = btext[0].color; }
            }
            floorMarks[ii] = btext[0];
        }
        buttonFrame.SetParent( consoleButtons[0].transform.parent );
        updateCurrentFloor( true );
		updateCurOffset( curOffset, true );
    }

	void scrollToCurrent( bool instantly ){
		scrollTo( floorCnt - floorCur - Mathf.RoundToInt(visibleButtons / 2) - 1, instantly );
	}

    void buttonClick( int button_ind ) {
		if (operate) { 
			if (button_ind < 0) {
				scrollTo (curOffset + ((button_ind == -1) ? -1 : 1) * visibleButtons, false);
			} else {
				if ((button_ind != floorCur) && ((floorTarget == -1) || (floorTarget == floorCur))) {
					floorMarks [button_ind].color = floorTargetColor;
					controlScript.elevator2.call (floorTarget = button_ind);
					scrollToCurrent (false);
				}
			}
		}
    }

    private void updateCurrentFloor(bool instantly) {
		if (operate) {
			int floorNew = controlScript.elevator2.currentFloor;
			if (floorNew != floorPre) {
				if (floorCur != floorNew) {
					Vector3 bf = buttonFrame.localPosition;
					floorCur = floorNew;
					buttonFrame.localPosition = new Vector3 (bf.x, frameBaseOrgY - (floorCnt - floorCur - 1) * buttonItemHeight, bf.z);
					scrollToCurrent (instantly);
				}
				if (floorNew == floorTarget) {
					floorMarks [floorCur].color = regularFloorColor;
				}
				floorPre = floorNew;
			}
		}
    }

    private void scrollTo( int _new, bool instantly ) {
        if ( _new + visibleButtons >= floorCnt ) { _new = floorCnt - visibleButtons; }
        if ( _new < 0 ) { _new = 0; }
        if ( _new != curOffset ) {
            if (instantly) {
                placeTo( _new );
            } else {
                toPos = sliderBaseOrg.y + (newOffset = _new) * buttonItemHeight;
                dir = ( (buttonSlider.localPosition.y < toPos) ? 1 : -1 );
            }
        }
    }

    private void placeTo( int offset ) {
		updateCurOffset(offset, false);
        buttonSlider.localPosition = new Vector3( sliderBaseOrg.x, sliderBaseOrg.y + offset * buttonItemHeight, sliderBaseOrg.z );
    }

    private void Update() {
		updateCurrentFloor( true );
        if ( dir != 0 ) {
            float newPos = buttonSlider.localPosition.y + dir*Time.deltaTime * 400;
            if ( dir * (newPos - toPos) > -0.001f ) {
                dir = 0;
                newPos = toPos;
				updateCurOffset( newOffset, false );
            }
            buttonSlider.localPosition = new Vector3( sliderBaseOrg.x, newPos, sliderBaseOrg.z );
        }
    }

	private void updateCurOffset(int offset, bool init){
		if (init || (curOffset != offset)) {
			curOffset = offset;
			consoleButtons[1].interactable = (offset != 0);
			consoleButtons[2].interactable = (offset < floorCnt - visibleButtons);
		}
	}

}
