using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour {

    private Transform player;

    void Start()
    {
        player = GameObject.Find("First Person").GetComponent<Transform>();
    }

    void Update()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, 2*Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().name == "First Person")
        {
            Application.LoadLevel(0);
        }
    }

}
