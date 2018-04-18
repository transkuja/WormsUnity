using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMinimap : MonoBehaviour {

    Transform target;

    public void ChangeTarget(Transform _newTarget)
    {
        target = _newTarget;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.eulerAngles = target.eulerAngles;
            transform.eulerAngles = new Vector3(90.0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
