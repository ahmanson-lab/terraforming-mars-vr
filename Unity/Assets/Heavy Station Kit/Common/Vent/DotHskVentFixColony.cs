using UnityEngine;
using System.Collections;

public class DotHskVentFixColony : StateMachineBehaviour {
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerInfo){
		animator.transform.localPosition = new Vector3(-4.42f,1f,0f);
	}
}
