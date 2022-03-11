using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //public Clock currentClock;

    private PlayerInput playerInput;

    private InputAction ringLeftAction;
    private InputAction ringRightAction;
    private InputAction tickAction;
    private InputAction skipAction;
    private InputAction switchRight;
    private InputAction switchLeft;

    private void Awake()
    {
        Services.inputManager = this;

        playerInput = GetComponent<PlayerInput>();

        ringLeftAction = playerInput.actions["RingLeft"];
        ringLeftAction.started += onRingLeft;

        ringRightAction = playerInput.actions["RingRight"];
        ringRightAction.started += onRingRight;

        tickAction = playerInput.actions["Tick"];
        tickAction.started += onTick;
        //tickAction.canceled += _ => Debug.Log("release");

        skipAction = playerInput.actions["Skip"];
        //skipAction.started += onSkip;
        skipAction.performed += Skip;
        skipAction.canceled += onSkipExit;

        switchRight = playerInput.actions["SwitchRight"];
        switchRight.started += SwitchRight;

        switchLeft = playerInput.actions["SwitchLeft"];
        switchLeft.started += SwitchLeft;
    }

    private void Update()
    {

    }

    private void onTick(InputAction.CallbackContext ctx)
    {
        Services.clockManager.currentClock.GetComponent<Clock>().RotateMinuteHand(6);
    }

    private void onRingLeft(InputAction.CallbackContext ctx)
    {
        // ring left
        Services.audioManager.playLeftAudio();
    }

    private void onRingRight(InputAction.CallbackContext ctx)
    {
        // ring right
        Services.audioManager.playRightAudio();
    }

    private void onSkip(InputAction.CallbackContext ctx)
    {
        Services.timeManager.skipping = true;
    }
    
    private void Skip(InputAction.CallbackContext ctx)
    {
        Services.timeManager.Skip(ctx.ReadValue<Vector2>());
    }

    private void onSkipExit(InputAction.CallbackContext ctx)
    {
        Services.timeManager.skipping = false;
    }

    private void SwitchRight(InputAction.CallbackContext ctx)
    {
        Services.clockManager.nextClock();
    }

    private void SwitchLeft(InputAction.CallbackContext ctx)
    {
        Services.clockManager.prevClock();
    }
}
