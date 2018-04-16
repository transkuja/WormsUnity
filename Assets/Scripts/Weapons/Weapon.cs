using UnityEngine;

public class Weapon : MonoBehaviour {
    public float weaponPowerMax;

    public bool isAimAvailable;
    public int damage;
    public bool isChargeable;

    [SerializeField]
    protected float currentCharge;
    public bool isCharging = false;
    float chargeBuffer;

    public virtual void Shoot()
    {
        StopCharge();
    }

    public virtual void Charge()
    {
        chargeBuffer = -Mathf.PI/2.0f;
        isCharging = true;
    }

    public virtual void StopCharge()
    {
        isCharging = false;
    }

    private void Update()
    {
        if (isCharging)
        {
            chargeBuffer += Time.deltaTime;
            currentCharge = ((Mathf.Sin(chargeBuffer) + 1) / 2) * weaponPowerMax;
        }
    }
}
