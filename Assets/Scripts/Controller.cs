using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    enum ControllerState { Move, Aim, Inventory, MoveOnly, Blocked }
    CharacterData data;
    Rigidbody rb;
    public float jumpStrength;

    [SerializeField]
    ControllerState currentState;

    Weapon equippedWeapon;

    public float moveSpeed;
    public float maxSpeed;

    GroundControl groundControl;

    public Weapon EquippedWeapon
    {
        get
        {
            if (equippedWeapon == null)
                equippedWeapon = GetComponentInChildren<Weapon>();
            return equippedWeapon;
        }

        set
        {
            equippedWeapon = value;
        }
    } 

    public Rigidbody Rb
    {
        get
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    public GroundControl GroundControl
    {
        get
        {
            if (groundControl == null)
                groundControl = GetComponent<GroundControl>();
            return groundControl;
        }

        set
        {
            groundControl = value;
        }
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        data = GetComponentInParent<CharacterData>();
        currentState = ControllerState.Move;
    }

    private void OnEnable()
    {
        currentState = ControllerState.Move;
    }

    public void SetToMoveOnly()
    {
        currentState = ControllerState.MoveOnly;
    }

    public void SetToBlocked()
    {
        currentState = ControllerState.Blocked;
    }

    void MoveStateControls()
    {
        if (GroundControl == null || !GroundControl.IsGrounded)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (EquippedWeapon != null)
            {
                if (EquippedWeapon.isAimAvailable)
                {
                    currentState = ControllerState.Aim;
                    return;
                }
            }
        }

        MoveControls();

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.instance.uiRef.inventory.SetInventory(data.inventory);
            GameManager.instance.uiRef.inventory.gameObject.SetActive(true);
            currentState = ControllerState.Inventory;
        }
    }

    void MoveControls()
    {
        if (GroundControl == null || !GroundControl.IsGrounded)
            return;
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            if (AudioManager.Instance != null && AudioManager.Instance.jumpVoiceFX != null)
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.jumpVoiceFX, 2.0f);
        }

        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal"));
        Rb.AddForce(transform.forward * moveSpeed * Input.GetAxis("Vertical"));
        Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxSpeed);
    }

    void AimStateControls()
    {
        if (Input.GetMouseButtonUp(0))
            currentState = ControllerState.Move;
    }

    void InventoryControls()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.uiRef.inventory.gameObject.SetActive(false);
            currentState = ControllerState.Move;
        }
    }

    void Update()
    {
        if (currentState == ControllerState.Blocked)
            return;
        if (currentState == ControllerState.MoveOnly)
        {
            MoveControls();
            return;
        }

        if (currentState == ControllerState.Move)
            MoveStateControls();
        else if (currentState == ControllerState.Aim)
            AimStateControls();
        else if (currentState == ControllerState.Inventory)
            InventoryControls();


        if (EquippedWeapon != null)
        {
            if (EquippedWeapon.isAimAvailable)
            {
                if (Input.GetKey(KeyCode.KeypadPlus))
                    EquippedWeapon.AdjustAim();
                if (Input.GetKey(KeyCode.KeypadMinus))
                    EquippedWeapon.AdjustAim(true);
            }

            if (EquippedWeapon.isCharging && (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f))
                EquippedWeapon.StopCharge();
        }

        // Shoot!
        if (Input.GetMouseButtonDown(1))
        {
            if (EquippedWeapon != null)
            {
                if (EquippedWeapon.isChargeable && !EquippedWeapon.isCharging)
                {
                    EquippedWeapon.Charge();
                }
                else
                {
                    EquippedWeapon.Shoot();
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && GroundControl.IsGrounded)
        {
            if (EquippedWeapon != null && EquippedWeapon.isCharging)
                EquippedWeapon.StopCharge();

            GameManager.instance.GetComponent<TurnHandler>().SwitchCharacter();
        }


    }

    public IEnumerator ResetRigidbody()
    {
        while (Rb.constraints != RigidbodyConstraints.FreezeRotation)
        {
            yield return new WaitForSeconds(2.0f);
            if (Physics.Raycast(transform.position, Vector3.down, 1.5f))
            {
                Rb.constraints = RigidbodyConstraints.FreezeRotation;
                transform.rotation = Quaternion.identity;
            }
        }
    }

    public void ResetState()
    {
        currentState = ControllerState.Move;
    }
}
