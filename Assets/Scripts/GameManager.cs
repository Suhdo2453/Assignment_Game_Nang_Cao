using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxScore = 10;
    [SerializeField] private Slider coinBar;
    private int _score = 0;

    private void Start()
    {
        coinBar.maxValue = maxScore;
        coinBar.value = _score;
        coinBar.enabled = false;
    }

    public void CollectCoint()
    {
        // Score++;
        // this.UpdateScoreText();

        _score++;
        coinBar.value = _score;
    }
}
