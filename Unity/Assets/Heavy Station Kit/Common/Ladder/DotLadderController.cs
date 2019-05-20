using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson {
	
	public class DotLadderController : MonoBehaviour {
		
		public float speed = 5.0f;
		[SerializeField] private DotMouseLook m_MouseLook;
		
		private CharacterController controller;
		private Vector3 moveDirection = Vector3.zero;
		private Camera m_Camera;
		
		public void _init(){
			if((m_Camera != null) && (m_MouseLook != null)) {
				m_MouseLook.Init (transform, m_Camera.transform);
			}
		}
		
		void Start () {
			controller = gameObject.GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_MouseLook.Init (transform, m_Camera.transform);
		}
		
		
		void Update () {
			RotateView();
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			
			controller.Move(moveDirection * Time.deltaTime);
		}
		
		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}
	}
	
}