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
    private InputAction stopAction;
    private InputAction skipAction;
    private InputAction switchRight;
    private InputAction switchLeft;
    private InputAction switchClock;
    private InputAction goToClock;

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

        stopAction = playerInput.actions["Stop"];
        stopAction.started += onStop;
        stopAction.canceled += onResume;

        skipAction = playerInput.actions["Skip"];
        //skipAction.started += onSkip;
        skipAction.performed += Skip;
        skipAction.canceled += onSkipExit;

        switchRight = playerInput.actions["SwitchRight"];
        switchRight.started += SwitchRight;

        switchLeft = playerInput.actions["SwitchLeft"];
        switchLeft.started += SwitchLeft;

        switchClock = playerInput.actions["SwitchClock"];
        switchClock.performed += SwitchClock;
        switchClock.canceled += ResetSwitchDir;

        goToClock = playerInput.actions["GoToClock"];
        goToClock.started += GoToClock;
    }

    private void Update()
    {
        
    }

    private void onTick(InputAction.CallbackContext ctx)
    {
        Services.clockManager.currentClock.GetComponent<Clock>().RotateMinuteHand(6);
    }

    private void onStop(InputAction.CallbackContext ctx)
    {
        Services.clockManager.currentClock.GetComponent<Clock>().StopMinuteHand();
    }

    private void onResume(InputAction.CallbackContext ctx)
    {
        Services.clockManager.currentClock.GetComponent<Clock>().ResumeMinuteHand();
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
        //Services.clockManager.nextClock();
    }

    private void SwitchLeft(InputAction.CallbackContext ctx)
    {
        //Services.clockManager.prevClock();
    }

    private void SwitchClock(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx.ReadValue<Vector2>());
        Services.clockManager.handleInput(ctx.ReadValue<Vector2>());
    }

    private void ResetSwitchDir(InputAction.CallbackContext ctx)
    {
        // handled this in clockManager.handleInput
        //Services.clockManager.resetInputDir();
    }

    private void GoToClock(InputAction.CallbackContext ctx)
    {
        Services.clockManager.GoToHighlightedClock();
    }
}
