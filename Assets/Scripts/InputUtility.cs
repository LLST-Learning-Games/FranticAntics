using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtility
{
    public static string Platform
    {
        get
        {
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                return "macOS";

            return "Windows";
        }
    }

    public static string GetKeyString(int controllerIndex, ControllerButtonDirection direction)
    {
        return $"{Platform}-Controller-{controllerIndex}-{direction}";
    }

    public static bool IsButtonDown(int controllerIndex, ControllerButtonDirection direction)
    {
        try
        {
            return Input.GetButtonDown(GetKeyString(controllerIndex, direction));
        }
        catch
        {
            return false;
        }
    }

    public static bool IsButtonPressed(int controllerIndex, ControllerButtonDirection direction)
    {
        try
        {
            return Input.GetButton(GetKeyString(controllerIndex, direction));
        }
        catch
        {
            return false;
        }
    }

    public static bool IsButtonUp(int controllerIndex, ControllerButtonDirection direction)
    {
        return Input.GetButtonUp(GetKeyString(controllerIndex, direction));
    }
}

public enum ControllerButtonDirection
{
    North,
    East,
    South,
    West,
}