using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCamera : MonoBehaviour {
	
	void Update () {
        Vector3 rotateToward = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-rotateToward, Vector3.up);
	}
}
