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

    public float aimSpeed;

    public virtual void Shoot()
    {
        StopCharge();
    }

    public virtual void Charge()
    {
        chargeBuffer = -Mathf.PI/2.0f;
        isCharging = true;
        GameManager.instance.uiRef.chargeBar.transform.GetChild(1).gameObject.SetActive(true);
    }

    public virtual void StopCharge()
    {
        isCharging = false;
        GameManager.instance.uiRef.chargeBar.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isCharging)
        {
            chargeBuffer += Time.deltaTime * 5;
            currentCharge = ((Mathf.Sin(chargeBuffer) + 1) / 2) * weaponPowerMax;
            GameManager.instance.uiRef.chargeBar.value = currentCharge / weaponPowerMax;
        }
    }

    public void AdjustAim(bool _adjustDown = false)
    {
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, 
            transform.localEulerAngles.y, 
            Mathf.Clamp(transform.localEulerAngles.z + ((_adjustDown) ? -1 : 1) * aimSpeed, 230, 320));
    }
}
