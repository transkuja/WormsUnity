using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    enum ControllerState { Move, Aim }
    CharacterData data;
    Rigidbody rb;
    public float jumpStrength;

    ControllerState currentState;

    bool isGrounded = false;
    Weapon equippedWeapon;

    public float moveSpeed;
    public float maxSpeed;

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

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        set
        {
            isGrounded = value;
            if (isGrounded)
                rb.drag = 15.0f;
            else
                rb.drag = 0.0f;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        data = GetComponentInParent<CharacterData>();
        currentState = ControllerState.Move;
    }

    private void OnEnable()
    {
        currentState = ControllerState.Move;
    }

    void MoveStateControls()
    {
        // Switch to aim mode
        if (!IsGrounded)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }

        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal"));
        rb.AddForce(transform.forward * moveSpeed * Input.GetAxis("Vertical"));
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameManager.instance.uiRef.inventory.gameObject.activeSelf)
                GameManager.instance.uiRef.inventory.gameObject.SetActive(false);
            else
            {
                GameManager.instance.uiRef.inventory.SetInventory(data.inventory);
                GameManager.instance.uiRef.inventory.gameObject.SetActive(true);
            }
        }
    }

    void AimStateControls()
    {
        if (Input.GetMouseButtonUp(0))
            currentState = ControllerState.Move;
    }

    void Update()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (currentState == ControllerState.Move)
            MoveStateControls();
        else if (currentState == ControllerState.Aim)
            AimStateControls();

        if (EquippedWeapon != null)
        {
            if (EquippedWeapon.isAimAvailable)
                if (Input.GetKey(KeyCode.KeypadPlus))
                    EquippedWeapon.AdjustAim();
            if (Input.GetKey(KeyCode.KeypadMinus))
                EquippedWeapon.AdjustAim(true);

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

        if (Input.GetKeyDown(KeyCode.Tab) && IsGrounded)
        {
            if (EquippedWeapon != null && EquippedWeapon.isCharging)
                EquippedWeapon.StopCharge();

            GameManager.instance.GetComponent<TurnHandler>().SwitchCharacter();
        }


    }
}
