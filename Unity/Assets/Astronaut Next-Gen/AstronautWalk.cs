// Taken from: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class AstronautWalk : MonoBehaviour {

    public Transform[] points;
    private int destPoint = 0;
    [SerializeField] float _moveSpeed = 2f;


    void Start () {

        GotoNextPoint();
    }


    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        transform.LookAt(points[destPoint].position);
        Vector3 newTransform = transform.position + transform.forward * _moveSpeed;
        float fraction =+ Time.deltaTime * _moveSpeed;
        transform.position = Vector3.Lerp(transform.position, newTransform, fraction);
        //agent.destination = points[destPoint].position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        if(collision.collider.tag == "WayPoint")
        {
            destPoint = (destPoint + 1) % points.Length;
            Debug.Log("Waypoint detected");
        }
    }


    void Update () {
        // Choose the next destination point when the agent gets
        // close to the current one.
        //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}