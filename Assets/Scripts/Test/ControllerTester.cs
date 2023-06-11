using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTester : MonoBehaviour
{
    public int controllerIndex;
    public List<ControllerIndicator> indicators = new List<ControllerIndicator>();

    private void Update()
    {
        indicators.ForEach(UpdateImage);
    }

    private void UpdateImage(ControllerIndicator indicator)
    {
        bool isPressed = InputUtility.IsButtonDown(controllerIndex, indicator.direction);
        indicator.image.color = isPressed ? Color.green : Color.white;
    }

    [Serializable]
    public struct ControllerIndicator
    {
        public Image image;
        public ControllerButtonDirection direction;
    }
}
