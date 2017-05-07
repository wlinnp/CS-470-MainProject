using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    private float delayTime = 0.5f;

    private float counter = 0.0f;

    private float damage = 25.0f;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && counter > delayTime)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<HealthScript>().RemoveHealth(damage);
                }
                else
                {
                    //bullethole goes here
                }

            }
        }



        counter += Time.deltaTime;
    }
}
