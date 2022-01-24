using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
    }
}
