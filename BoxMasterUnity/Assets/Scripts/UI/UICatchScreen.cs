﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class UICatchScreen : UIScreen, IPointerClickHandler
{
    /// <summary>
    /// The Screen Menu.
    /// </summary>
    [SerializeField]
    [Tooltip("The screen menu.")]
    protected UIScreenMenu _UIScreenMenu;

    [SerializeField]
    protected RawImage _videoTexture;

    public string videoClipPath = "";

    [SerializeField]
    public VideoPlayer _videoPlayer = null;

    protected override void Awake()
    {
        videoClipPath = GameManager.instance.gameSettings.catchScreenVideoPath;
        VideoManager.instance.AddClip(videoClipPath);
        _videoPlayer.clip = VideoManager.instance.GetClip(videoClipPath);
        _videoPlayer.targetTexture = (RenderTexture)_videoTexture.texture;
        base.Awake();
        if (_UIScreenMenu == null)
            _UIScreenMenu = GetComponentInParent<UIScreenMenu>();
    }

    public override void Hide()
    {
        base.Hide();
        _videoPlayer.Stop();
        //VideoManager.instance.StopClip(videoClipPath);
    }

    public override void Show()
    {
        base.Show();
        _videoPlayer.Play();
        //VideoManager.instance.PlayClip(videoClipPath, (RenderTexture)_videoTexture.texture);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _UIScreenMenu.menuBar.SetState(true);
    }
}
