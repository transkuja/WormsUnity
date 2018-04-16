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

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }

        // Aim mode
        if (Input.GetMouseButtonDown(0))
        {
            if (GetComponentInChildren<Weapon>())
            {
                if (GetComponentInChildren<Weapon>().isAimAvailable)
                    currentState = ControllerState.Aim;
            }
        }

        if (Input.GetMouseButtonUp(0))
            currentState = ControllerState.Move;

        // Shoot!
        if (Input.GetMouseButtonDown(1))
        {
            if (GetComponentInChildren<Weapon>())
            {
                GetComponentInChildren<Weapon>().Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.instance.GetComponent<TurnHandler>().SwitchCharacter();
        }


    }
}
