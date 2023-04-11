using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeData : MonoBehaviour
{
    [Header("Blink detection settings")]
    public float confidenceThreshold = 0.3f;
    public float blinkCooldown = 0.15f;

    [Header("Feedback viewing")]
    [SerializeField] int blinkCounts;
    [SerializeField] bool inFrame;


    [Header("OpenSeeFace script")]
    [SerializeField] OpenSee.OpenSee openSee;
    [SerializeField] int faceId = 0;


    /// <summary>
    /// To hook up with this, call EyeData.Instance.BlinkEvent.AddListener("Method Name");
    /// </summary>
    [HideInInspector] public UnityEvent BlinkEvent;

    // Private fields
    OpenSee.OpenSee.OpenSeeData openSeeData;
    double previousTime = 0;
    float blinkTimer = 0f;
    float startDelay = 1f;

    #region EyeData Singleton
    static private EyeData instance;
    static public EyeData Instance { get { return instance; } }

    void CheckManagerInScene()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void Awake()
    {
        CheckManagerInScene();
        BlinkEvent = new UnityEvent();
    }

    bool UpdateOpenSeeData()
    {
        openSeeData = openSee.GetOpenSeeData(faceId);
        return openSeeData != null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Instance.BlinkEvent.Invoke();
        }
    }

    void FixedUpdate()
    {
        blinkTimer += Time.deltaTime;

        if (startDelay < 2f) { startDelay += Time.deltaTime; return; }

        if (blinkTimer > blinkCooldown)
        {
            if (!UpdateOpenSeeData())
                return;
            blinkTimer = 0f;

            // Method to determine whether the player is in frame or not
            checkInFrame(openSeeData.time);

            // Check if player is blinking
            if ((isClosed(openSeeData.leftEyeOpen) && isClosed(openSeeData.rightEyeOpen)) || !inFrame)
            {
                Blink();
            }

        }
    }

    /// <summary>
    /// This method will be called if the program has determined that both eyes are closed.
    /// </summary>
    void Blink()
    {
        Debug.Log("Player blinked");
        BlinkEvent.Invoke();
        blinkCounts++;
    }

    /// <summary>
    /// Checks to see if the previous data packet had the same time stamp as the current time stamp. 
    /// This means that the software could not capture your face.
    /// We can assume the player is out of frame if this happens.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    bool checkInFrame(double time)
    {
        bool b = time != previousTime;
        previousTime = time;
        inFrame = b;
        return b;
    }

    public void updateConfidence(float f)
    {
        this.confidenceThreshold = f;
    }

    /// <summary>
    /// Given an eye closed confidence value, it returns true or false if this value is less than the confidence threshold.
    /// </summary>
    /// <param name="confidence"></param>
    /// <returns></returns>
    bool isClosed(float confidence)
    {
        return confidence <= confidenceThreshold;
    }

    #region Getters

    /// <summary>
    /// Returns the confidence level associated with your left eye.
    /// This is on a scale of 0-1, where 0 is 100% certain it is closed, and 1 is 100% certain it is open.
    /// </summary>
    /// <returns></returns>
    public float getLeftEyeConfidence()
    {
        return openSeeData.leftEyeOpen;
    }

    /// <summary>
    /// Returns the confidence level associated with your right eye.
    /// This is on a scale of 0-1, where 0 is 100% certain it is closed, and 1 is 100% certain it is open.
    /// </summary>
    /// <returns></returns>
    public float getRightEyeConfidence()
    {
        return openSeeData.rightEyeOpen;
    }

    /// <summary>
    /// Returns the quaternion associated with the left eye's gaze direction.
    /// </summary>
    /// <returns></returns>
    public Quaternion getLeftEyeGaze()
    {
        return openSeeData.leftGaze;
    }

    /// <summary>
    /// Returns the quaternion associated with the right eye's gaze direction.
    /// </summary>
    /// <returns></returns>
    public Quaternion getRightEyeGaze()
    {
        return openSeeData.rightGaze;
    }

    /// <summary>
    /// Returns true or false if the program has determined that the player is currently in camera frame.
    /// </summary>
    /// <returns></returns>
    public bool playerInFrame()
    {
        return inFrame;
    }

    #endregion
}
