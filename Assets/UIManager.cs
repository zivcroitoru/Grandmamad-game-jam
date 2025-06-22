using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
public GameObject joystickUI;
public GameObject pickupButton;

void Start()
{
    bool isMobile = Application.isMobilePlatform;
    joystickUI.SetActive(isMobile);
    pickupButton.SetActive(isMobile);
}
}