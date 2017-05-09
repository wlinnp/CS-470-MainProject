using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {
   
    public enum MovementState {
        Idle,
        Walking
    };
    public MovementState MovementType;

    float walkingSpeed = 0.4f;
    float rotatingSpeed = 0.8f;

    public GameObject WayPoint;
    GameObject wp;

    float minDistance = -2.0f;
    float maxDistance = 2.0f;

    bool hasWayPoint = false;
    

    Animator catAnimator;

    private void Awake() {
        catAnimator = GetComponent<Animator>();
    }
    void Start () {
        StartCoroutine(ChooseAction());
    }
	
	void Update () {
        if (MovementType.Equals(MovementState.Idle)) {
            catAnimator.SetBool("isMoving", false);
        }
        if (MovementType.Equals(MovementState.Walking)) {
            CreateWayPoint();
            catAnimator.SetBool("isMoving", true);

        }
        if (hasWayPoint) {
            MoveObject();

        }
    }

    private void CreateWayPoint() {
        if (!hasWayPoint) {
            hasWayPoint = true;
            wp = Instantiate(WayPoint, new Vector3(transform.position.x + Random.Range(minDistance, maxDistance), 
                transform.position.y, 
                transform.position.z + Random.Range(minDistance, maxDistance)), Quaternion.identity) as GameObject;
            
        }
    }

    private void MoveObject() {
        transform.position = Vector3.MoveTowards(transform.position, wp.transform.position, walkingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(wp.transform.position, transform.position),
            rotatingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("WayPoint")) {
            Destroy(wp);
            hasWayPoint = false;
            MovementType = MovementState.Idle;
        }
    }

    private IEnumerator ChooseAction() {
        while (true) {
            yield return new WaitForSeconds(3.0f);
            if (!hasWayPoint) {
                int actionType = Random.Range(0, 2);
                if (actionType.Equals(0)) {
                    MovementType = MovementState.Idle;
                } else {
                    MovementType = MovementState.Idle;
                }
            }
        }
    }
}
