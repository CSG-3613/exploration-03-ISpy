using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public OpenSee.OpenSee openSee;
    public float confidenceThreshold = 0.3f;
    public float blinkCooldown = 0.1f;
    public int faceId = 0;
    public int blinkCounts;
    public bool inFrame;
    private float blinkTimer = 0f;
    OpenSee.OpenSee.OpenSeeData openSeeData;

    double previousTime = 0;

    void Start()
    {
        UpdateOpenSeeData();
    }


    bool UpdateOpenSeeData()
    {
        openSeeData = openSee.GetOpenSeeData(faceId);
        return openSeeData != null;
    }

    void FixedUpdate()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer > blinkCooldown)
        {
            if (!UpdateOpenSeeData())
                return;
            blinkTimer = 0f;

            // Method to determine whether the player is in frame or not
            checkInFrame(openSeeData.time);

            // Check if player is blinking
            if (isClosed(openSeeData.leftEyeOpen) && isClosed(openSeeData.rightEyeOpen))
            {
                Debug.Log("Player blinked");
                blinkCounts++;
            }

        }
    }

    bool checkInFrame(double time)
    {
        bool b = time != previousTime;
        previousTime = time;
        inFrame = b;
        return b;
    }

    bool isClosed(float confidence)
    {
        return confidence <= confidenceThreshold;
    }
}
