using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool enabled = false;
    
    public void OnClick()
    {
        if (enabled)
        {
            panel.SetActive(true);
            enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            panel.SetActive(false);
            enabled = true;
            Time.timeScale = 1;
        }
    }
}
