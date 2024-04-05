using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionPanel : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Button leaveButton;
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);

    }
}
