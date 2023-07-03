using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private PlayerInput input = null;

    public PlayerInput PlayerInput => input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }


    }

    private void OnEnable() => input.actions.Enable();

    private void OnDisable() => input.actions.Disable();

}
