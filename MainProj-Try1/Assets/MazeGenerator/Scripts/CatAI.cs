using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * file: CatAI.cs
 * author: Wai Phyo
 * class: CS 470 - Game Development
 * Main Project
 * last modified: 05/04/2017
 * 
 * Original Cate from Unity Package:
 * https://www.assetstore.unity3d.com/en/#!/content/70180
 * purpose: Making cat to move around with an AI.  
 */

public class CatAI : MonoBehaviour {
    private float timer;
    private NavMeshAgent agent;
    public float wanderRadius;
    public float wanderTimer;
    // Range of movement
    public float rangeY = 2f;

    // Speed
    public float speed = 3f;

    // Initial direction
    public float direction = 1f;

    // To keep the initial position
    Vector3 initialPosition;
    public GameObject Cat = null;
    Rigidbody rb;
    //// Use this for initialization
    //void OnEnable() {
    //    if (Cat != null) {
    //        agent = Cat.GetComponent<NavMeshAgent>();
    //        timer = wanderTimer;
    //    }
    //}

    // Use this for initialization
    void Start () {
        initialPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Cat != null) {
        //    timer += Time.deltaTime;
        //    print(timer);
        //    if (timer >= wanderTimer) {
        //        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        //        agent.SetDestination(newPos);
        //        timer = 0;
        //    }
        //}
        // Set x and z velocities to zero
        float movementX = direction * speed * Time.deltaTime;

        // New position
        float newX = transform.position.x + movementX;

        // Check whether the limit would be passed
        if (Mathf.Abs(newX - initialPosition.x) > rangeY) {
            // Move the other way
            direction *= -1;
        }

        // If it can move further, move
        else {
            // Move the object
            transform.Translate(new Vector3(0, 0, movementX));
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
