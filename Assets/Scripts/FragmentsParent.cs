using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsParent : MonoBehaviour {

    public List<Collider> everythingImpacted;

    private IEnumerator Start()
    {
        everythingImpacted = new List<Collider>();
        yield return new WaitUntil(() => transform.childCount > 0);
        yield return new WaitUntil(() => transform.childCount == 0);
        GameManager.instance.GetComponent<TurnHandler>().WeaponEndProcess(GameManager.instance.GetComponent<TurnHandler>().CheckSelfDamage(everythingImpacted.ToArray()));
    }
}
