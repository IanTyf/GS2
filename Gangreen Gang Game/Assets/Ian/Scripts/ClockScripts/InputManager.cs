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
    private InputAction rewind;
    private InputAction fastforward;
    private InputAction toggleUI;
    private InputAction switchTask;

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

        rewind = playerInput.actions["Rewind"];
        rewind.started += onRewind;
        rewind.canceled += onStopRewind;

        fastforward = playerInput.actions["Fastforward"];
        fastforward.started += onFastforward;
        fastforward.canceled += onStopFastforward;

        toggleUI = playerInput.actions["ToggleUI"];
        toggleUI.started += onToggleUI;

        switchTask = playerInput.actions["SwitchTask"];
        switchTask.performed += onSwitchTask;

        //getGamepad();
        //Gamepad gp = getGamepad();
        //Debug.Log(gp.name);
    }

    private void Update()
    {
        
    }

    private void onTick(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.clockManager.currentClock.GetComponent<Clock>().RotateMinuteHand(6);
    }

    private void onStop(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.clockManager.currentClock.GetComponent<Clock>().StopMinuteHand();
    }

    private void onResume(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.clockManager.currentClock.GetComponent<Clock>().ResumeMinuteHand();
    }

    private void onRingLeft(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        // ring left
        //Services.timeEventManager.isRinging = true;
        Services.actionConditionsManager.isRinging = true;
        Services.audioManager.playLeftAudio();
    }

    private void onRingRight(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        // ring right
        //Services.timeEventManager.isRinging = true;
        Services.actionConditionsManager.isRinging = true;
        Services.audioManager.playRightAudio();
    }

    private void onSkip(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.timeManager.skipping = true;
    }
    
    private void Skip(InputAction.CallbackContext ctx)
    {
        //Services.timeManager.Skip(ctx.ReadValue<Vector2>());
    }

    private void onSkipExit(InputAction.CallbackContext ctx)
    {
        //Services.timeManager.skipping = false;
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
        if (Services.timeManager.skipping) return;
        Services.clockManager.handleInput(ctx.ReadValue<Vector2>());
    }

    private void ResetSwitchDir(InputAction.CallbackContext ctx)
    {
        // handled this in clockManager.handleInput
        //Services.clockManager.resetInputDir();
    }

    private void GoToClock(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.clockManager.GoToHighlightedClock();
    }

    private void onRewind(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.fastForwarding) return;
        Services.timeManager.skipping = true;
    }

    private void onStopRewind(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.fastForwarding) return;
        Services.timeManager.skipping = false;
    }

    private void onFastforward(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.timeManager.fastForwarding = true;
    }

    private void onStopFastforward(InputAction.CallbackContext ctx)
    {
        if (Services.timeManager.skipping) return;
        Services.timeManager.fastForwarding = false;
    }

    private void onToggleUI(InputAction.CallbackContext ctx)
    {
        Services.taskUIManager.ToggleUI();
    }

    private void onSwitchTask(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        if (dir.y < -0.5) Services.taskUIManager.NextTask();
        if (dir.y > 0.5) Services.taskUIManager.PrevTask();
    }

    public void rumble(float low, float high)
    {
        Gamepad.current.SetMotorSpeeds(low, high);
    }

    public void stopRumble()
    {
        Gamepad.current.SetMotorSpeeds(0f, 0f);
        Gamepad.current.ResetHaptics();
    }
}
