using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConfidenceUpdater : MonoBehaviour
{
    [SerializeField] EyeData ed;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text text;

    public void readSlider()
    {
        text.text = "Blink Confidence: " + Mathf.Round(slider.value * 100f) / 100f;
        ed.updateConfidence(Mathf.Round(slider.value * 100f) / 100f);
    }
}
