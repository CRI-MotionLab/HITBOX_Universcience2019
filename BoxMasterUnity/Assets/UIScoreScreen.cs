﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreScreen : MonoBehaviour, IHideable {
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected Text _player1Text;
    [SerializeField]
    protected Text _player2Text;
    [SerializeField]
    protected Text _timeText;

    void Start()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        int time = GameManager.instance.gameTime;
        _player1Text.text = GameManager.instance.player1Score.ToString();
        _player2Text.text = GameManager.instance.player2Score.ToString();
        _timeText.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
    }
}