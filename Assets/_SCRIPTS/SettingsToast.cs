using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToast : MonoBehaviour
{
    [SerializeField] private Text toast;
    [SerializeField] private RectTransform backing;

    public void TurnOn(string s)
    {
        toast.text = s;
        Vector2 cursorPos = new Vector2(Input.mousePosition.x - 1920/2, Input.mousePosition.y - 1080/2);
        backing.localPosition = cursorPos;
        this.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
