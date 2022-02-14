using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    public float sensitivity = 10f;
    public float smoothness = 0.3f;
    
    [Header("Horizontal Axis")]
    public float horizontalMargin = 0.2f;
    public float horizontalLimit = 5f;

    [Header("Vertical Axis")]
    public float verticalMargin = 0.2f;
    public float verticalLimit = 5f;

    private Vector3 initialRot;
    private float yaw, pitch, yawL, pitchL, smoothMultipler;

    void Start()
    {
        initialRot = transform.eulerAngles;
        yaw = pitch = yawL = pitchL = 0;
        smoothMultipler = (1 - smoothness) * 15 + 5;
    }

    void Update()
    {
        smoothMultipler = (1 - smoothness) * 15 + 5;
        // there is a million ways to do this
        // all of these math are trying to make the movement feel smoother
        // firstly when mouse position is within xMargin or yMargin, add to yaw and pitch correspondingly
        // the stuff after Mathf.Abs is to make it "slow down" linearly after reaching half of the limit so that it wouldn't run into an abrupt stop when it hits the limit
        yaw += xMargin() * Time.deltaTime * sensitivity * (Mathf.Abs(yaw) > horizontalLimit/2 && Mathf.Sign(xMargin()) == Mathf.Sign(yaw) ? 1 - (Mathf.Abs(yaw) - horizontalLimit/2) / (horizontalLimit/2) : 1);
        pitch -= yMargin() * Time.deltaTime * sensitivity * (Mathf.Abs(pitch) > verticalLimit/2 && Mathf.Sign(-yMargin()) == Mathf.Sign(pitch) ? 1 - (Mathf.Abs(pitch) - verticalLimit/2) / (verticalLimit/2) : 1);

        // when mouse position in one of the margins but not the other, 
        // player probably still want some movement in the other direction so we give them that, with a more generous margin set by the secret multiplier
        if (xMargin() != 0 && yMargin() == 0) pitch -= yMargin(2) * Time.deltaTime * sensitivity * (Mathf.Abs(pitch) > verticalLimit/2 && Mathf.Sign(xMargin()) == Mathf.Sign(yaw) ? 1 - (Mathf.Abs(pitch) - verticalLimit/2) / (verticalLimit/2) : 1);
        if (yMargin() != 0 && xMargin() == 0) yaw += xMargin(2) * Time.deltaTime * sensitivity * (Mathf.Abs(yaw) > horizontalLimit/2 && Mathf.Sign(-yMargin()) == Mathf.Sign(pitch) ? 1 - (Mathf.Abs(yaw) - horizontalLimit/2) / (horizontalLimit/2) : 1);

        // clamp the rotations with limits before updating eulerAngles
        yaw = Mathf.Clamp(yaw, -horizontalLimit, horizontalLimit);
        pitch = Mathf.Clamp(pitch, -verticalLimit, verticalLimit);

        yawL = Mathf.Lerp(yawL, yaw, smoothMultipler * Time.deltaTime);
        pitchL = Mathf.Lerp(pitchL, pitch, smoothMultipler * Time.deltaTime);
        transform.eulerAngles = initialRot + new Vector3(pitchL, yawL, 0);
    }

    // if not in margins, return 0
    // if within margins, return how far the mouse is from the center of the screen, in (-1, 1)
    float xMargin(int secretMultiplier = 1)
    {
        float mouseX = Input.mousePosition.x;
        float margin = Mathf.Clamp(horizontalMargin * secretMultiplier, 0f, 0.5f);

        if (mouseX >= Screen.width * margin && mouseX <= Screen.width * (1 - margin)) return 0;
        else return (mouseX - Screen.width / 2) / (Screen.width / 2);
    }
    float yMargin(int secretMultiplier = 1)
    {
        float mouseY = Input.mousePosition.y;
        float margin = Mathf.Clamp(verticalMargin * secretMultiplier, 0f, 0.5f);

        if (mouseY >= Screen.height * margin && mouseY <= Screen.height * (1 - margin)) return 0;
        else return (mouseY - Screen.height / 2) / (Screen.height / 2);
    }
}
