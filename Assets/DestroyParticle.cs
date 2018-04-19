using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

	IEnumerator Start () {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
	}

}
