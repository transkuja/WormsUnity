using UnityEngine;
public enum WeaponType { Bazooka, Finger, Dynamite, HolyGrenade, Grenade, Size, None  }

[System.Serializable]
public class Weapon : MonoBehaviour {
    public float weaponPowerMax;
    public WeaponType weaponType;
    public bool isAimAvailable;
    public int damage;
    public bool isChargeable;

    [SerializeField]
    protected float currentCharge;
    public bool isCharging = false;
    float chargeBuffer;

    public float aimSpeed;

    public Sprite uiSprite;

    public int ammo;
    public bool hasAmmo;

    [SerializeField]
    protected GameObject projectile;

    public virtual void Shoot()
    {
        if (hasAmmo)
        {
            ammo--;
            GameManager.instance.GetComponent<TurnHandler>().GetCurrentCharacter().inventory[weaponType]--;
            if (ammo == 0)
                GameManager.instance.GetComponent<TurnHandler>().DestroyWeapon(weaponType);
        }
        else
            GameManager.instance.GetComponent<TurnHandler>().DestroyWeapon(weaponType);

        GameManager.instance.uiRef.equippedSlot.UpdateSlot(weaponType, ammo);
        StopCharge();

        ProjectileHandling();
    }

    public virtual GameObject ProjectileHandling()
    {
        GameObject instance = GameManager.instance.GetComponent<TurnHandler>().currentProjectileInstance;
        instance.AddComponent<Rigidbody>();
        return instance;
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

    public virtual void AdjustAim(bool _adjustDown = false)
    {}
}
