using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.FirstPerson 
{

	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (DotFirstPersonController))]
	public class DotFPCCrouch : MonoBehaviour
	{
        public float crouchSpeedRatio = 0.5f;
		public float crouchHeightRatio = 0.5f;

		private Transform t_person;
		private Transform t_camera = null;
		private float cameraHeightOffset = 0f;
		private Vector3 cameraLocalPos;
		private CharacterController charController;
		private float characterHeight;
		private DotFirstPersonController firstPersonController;
		private float walkSpeed;
		private float runSpeed;
		private bool headBobOrg;

		private bool crouchState = false;
		private float targetHeight;

        void Start()
		{
	        // Self person capsule transform
			t_person = transform;
			// Character Controller settings
            charController = GetComponent<CharacterController>();
			targetHeight = characterHeight = charController.height;
			// Camera position transform
			t_camera = t_person.Find ("FirstPersonCharacter");
			if (t_camera != null){
				cameraHeightOffset = characterHeight/2 - t_camera.localPosition.y;
				cameraLocalPos = t_camera.localPosition;
			}
			// First Person Controller settings
			firstPersonController = GetComponent<DotFirstPersonController>();
			walkSpeed = firstPersonController.m_WalkSpeed;
			runSpeed = firstPersonController.m_RunSpeed;
			headBobOrg = firstPersonController.m_UseHeadBob;
        }

        void Update()
		{
			// Check crouch state and set up base oparameters
			bool stateChanged = false;
			if (Input.GetKeyDown (KeyCode.C)) {
				crouchState = !crouchState;
				stateChanged = true;
			}
			if (stateChanged) {
				targetHeight = characterHeight;
				if (crouchState) {
					targetHeight *= crouchHeightRatio;
					firstPersonController.m_RunSpeed = crouchSpeedRatio * runSpeed;
					firstPersonController.m_WalkSpeed = crouchSpeedRatio * walkSpeed;
					// Disable Head Bob functionality in Crouch mode
					if (headBobOrg) { firstPersonController.m_UseHeadBob = false; }
				}
			}
			// Accomodate person capsule height
			float currentHeight = charController.height;
			float newHeight = Mathf.Lerp(currentHeight, targetHeight, 3f*Time.deltaTime);
			// Apply heights changes
			if (targetHeight != newHeight) {
				if (Mathf.Abs (targetHeight - newHeight) < 0.01f) {
					newHeight = targetHeight;
					// Restore walk&Run speed and Head Bob functionality in normal walk mode
					if (newHeight == characterHeight) {
						firstPersonController.m_UseHeadBob = headBobOrg;
						firstPersonController.m_RunSpeed = runSpeed;
						firstPersonController.m_WalkSpeed = walkSpeed;
					}
				} else {
					if (newHeight > currentHeight) {
						RaycastHit hit;
						if (Physics.Raycast (new Ray (t_person.position, t_person.up), out hit, newHeight * 0.5f + 0.1f)) {
							newHeight = currentHeight;
							return;
						}
					}
				}
				// Set person Y-position
				t_person.position = new Vector3 (t_person.position.x, t_person.position.y + (newHeight - charController.height) * 0.5f, t_person.position.z);
				// Set person Height
				charController.height = newHeight;
				// Calculate new camera local Y coord
				cameraLocalPos.y = newHeight /2 - cameraHeightOffset;
			}
			// Update camera local position
			if (t_camera != null) {
				t_camera.localPosition = cameraLocalPos;
			}
        }
    }

}
