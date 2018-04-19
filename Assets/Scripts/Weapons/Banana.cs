using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Grenade {

    public override void AdjustAim(bool _adjustDown = false)
    {
        transform.Rotate(((_adjustDown) ? 1 : -1) * aimSpeed * Vector3.right);

        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, 1, 90),
            transform.localEulerAngles.y, transform.localEulerAngles.z
        );
    }
}
