﻿using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
    private Transform[] waypoints;
    private int currentWaypoint = 0;
	private float rotationSpeed = 100f;

	private bool spinO=false;
	private int spinC=0;

    public GameObject waypointContainer;
    public float speed = 500f;

	// Use this for initialization
	void Start () {
	    // Copy waypoint info from public variable.  The container itself is among the transforms
		// in the container.
        Transform[] potentialWaypoints = waypointContainer.GetComponentsInChildren<Transform>();

		// Create the private waypoint array; it is one smaller than the potential array,
		// since we do not want to include the container itself.
        waypoints = new Transform[potentialWaypoints.Length - 1]; 

		// Loop through the potential array, only copying actual waypoints into the private array.
		for (int i = 0, j = 0; i < potentialWaypoints.Length; i++)
        {
            if (potentialWaypoints[i] != waypointContainer.transform)
            {
                // This is a waypoint rather than the container;
				// add the waypoint to the array.
                waypoints[j++] = potentialWaypoints[i];
            }
        }

		// Set the velocity of the player.
        //GetComponent<Rigidbody>().velocity = Vector3.up * speed;
	}
	void FixedUpdate(){
		if (spinO && spinC < 4) {
			var lookPos = waypoints[currentWaypoint].position - transform.position;
			lookPos.y = 0;
			var rotation = Quaternion.LookRotation(lookPos);
			rotation *= Quaternion.Euler(0, 90, 0); // this add a 90 degrees Y rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*50);
			spinC++;
			print ("spin out ai");
		} else {
			spinO = false;
			spinC = 0;
		}
	}

//	void Update() {
//		transform.position = Vector3.MoveTowards (transform.position, waypoints [currentWaypoint].position, speed * Time.deltaTime);
//	}

	void Update () {
		Vector3 movementVector = NavigateTowardWaypoint();

		// Set velocity to direction * speed * deltaTime.
		//GetComponent<Rigidbody>().velocity = movementVector.normalized * speed * Time.deltaTime;
		//GetComponent<Rigidbody>().rotation = Quaternion.Euler((movementVector.normalized) * rotationSpeed * Time.deltaTime);
		//GetComponent<Rigidbody>().rotation = Quaternion.Euler(0,movementVector.y-30,0);
		//transform.LookAt(movementVector);
		//movementVector.y=0;
		//transform.rotation = Quaternion.LookRotation(-movementVector);
	}

    Vector3 NavigateTowardWaypoint()
    {
		// This tells us how close we get to a waypoint before we turn our attention
		// to the next waypoint.
		float neighborhood = 10.6f;

		Vector3 movementVector =
			waypoints[currentWaypoint].position - transform.position;

		//print("*** Distance: " + movementVector.magnitude);

		// Are we in the neighborhood of the target waypoint?
		if (movementVector.magnitude <= neighborhood)
        {
			// Yes; focus our attention on the next waypoint in the path.
			currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
//			int rand = Random.Range(1, waypoints.Length -1);
//          currentWaypoint = (currentWaypoint + rand) % waypoints.Length;

           // print("Current waypoint: " + currentWaypoint + "    Relative Position: " + movementVector);
        }

        return movementVector;
    }
	void OnTriggerEnter(Collider other){
		Vector3 plVec = waypoints [currentWaypoint].position - other.transform.position;
		Vector3 aiVec = waypoints [currentWaypoint].position - transform.position;
		if (other.name.Equals("Cube")&&plVec.magnitude>aiVec.magnitude){
			//print ("spin out ai");
			spinO=true;
		}
	}
}
