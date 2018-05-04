﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenMenu : MonoBehaviour {
    /// <summary>
    /// The page array.
    /// </summary>
	[SerializeField]
	protected IHideable[] _pages;
    /// <summary>
    /// The catch screen
    /// </summary>
	[SerializeField]
    [Tooltip("The catch screen.")]
	protected UIScreen _catchScreen;
    /// <summary>
    /// The time out screen.
    /// </summary>
	[SerializeField]
    [Tooltip("The time out screen.")]
	protected UIScreen _timeOutScreen;
    /// <summary>
    /// The countdown screen
    /// </summary>
    [SerializeField]
    [Tooltip("The countdown screen.")]
    protected UICountdownPage _countdownPage;
    /// <summary>
    /// The score screen.
    /// </summary>
    [SerializeField]
    [Tooltip("The score screen.")]
    protected UIScoreScreen _scoreScreen;
    /// <summary>
    /// The menu bar
    /// </summary>
    [SerializeField]
    [Tooltip("The menu bar.")]
	protected UIMenuAnimator _menuBar;
    /// <summary>
    /// First model of a page prefab
    /// </summary>
	[SerializeField]
    [Tooltip("First type of page prefab.")]
	protected UIPage _pagePrefabType1;
    /// <summary>
    /// Second model of a page prefab
    /// </summary>
	[SerializeField]
    [Tooltip("Second type of page prefab.")]
	protected UIPage _pagePrefabType2;
    /// <summary>
    /// The current page
    /// </summary>
	protected IHideable _currentPage;

	protected int _pageIndex;

	public IHideable currentPage {
		get {
			return _currentPage;
		}
	}

	void OnEnable()
	{
		GameManager.onActivity += OnActivity;
		GameManager.onTimeOut += OnTimeOut;
		GameManager.onTimeOutScreen += OnTimeOutScreen;
	}

	void OnTimeOutScreen ()
	{
		GoToTimeoutScreen ();
	}

	void OnTimeOut ()
	{
		GoToHome ();
		GameManager.instance.Home ();
		_menuBar.SetState (false);
	}

	void OnActivity ()
	{
		_timeOutScreen.Hide ();
		_currentPage.Show ();
	}

    void Start() {
        _catchScreen.Show();
        _menuBar.Show();
        _pages = new IHideable[2];
        _pages[0] = GameObject.Instantiate(_pagePrefabType1, this.transform);
        _pages[1] = GameObject.Instantiate(_pagePrefabType2, this.transform);
		_currentPage = _catchScreen;
	}

	public IHideable GetNextPage() {
		var tempPageIndex = _pageIndex + 1;
		return (tempPageIndex >= _pages.Length) ? null : _pages [tempPageIndex];
	}

	public IHideable GetPreviousPage() {
		var tempPageIndex = _pageIndex - 1;
		return (tempPageIndex < 0) ? null : _pages [tempPageIndex];
	}

	public void GoToHome()
	{
		GoTo (0);
		_currentPage.Hide ();
		_catchScreen.Show ();
		_timeOutScreen.Hide ();
		_menuBar.Show ();
		_currentPage = _catchScreen;
		TextManager.instance.SetDefaultLang ();
		GameManager.instance.Home ();
	}

	public void GoToFirstPage()
	{
		GoTo (0);
		GameManager.instance.StartPages ();
	}

	public void GoToLastPage()
	{
		GoTo (_pages.Length - 1);
	}
		
	public void GoTo(int index)
	{
		var previous = _currentPage;
		int previousIndex = _pageIndex;
		try {
			_pageIndex = index;
			_currentPage = _pages[_pageIndex];
			previous.Hide();
			_currentPage.Show();
		}
		catch {
			_pageIndex = previousIndex;
			previous.Show ();
			_currentPage = previous;
		}
		_menuBar.Hide ();
		_catchScreen.Hide ();
		_timeOutScreen.Hide ();
	}

	public void GoToTimeoutScreen() {
		_currentPage.Hide ();
		_catchScreen.Hide ();
		_timeOutScreen.Show ();
	}

    public void GoToCountdownScreen()
    {
        _currentPage.Hide();
        _catchScreen.Hide();
        _timeOutScreen.Hide();
        _countdownPage.Show();
    }

    public void GoToScoreScreen()
    {
        _countdownPage.Hide();
        _timeOutScreen.Hide();
        _catchScreen.Hide();
        _currentPage.Hide();
        _scoreScreen.Show();
        _currentPage = _scoreScreen;
    }

	public void GoToPage() {
		var previous = _currentPage;
		_currentPage = _pages [_pageIndex];
		previous.Hide ();
		_currentPage.Show ();
		_catchScreen.Hide ();
		_timeOutScreen.Hide ();
	}

	public void GoToNext() {
        if (_pageIndex + 1 >= _pages.Length)
        {
            GoToCountdownScreen();
        }
        else
        {
            var previous = _currentPage;
            _pageIndex = Mathf.Clamp(_pageIndex + 1, 0, _pages.Length - 1);
            _currentPage = _pages[_pageIndex];
            previous.Hide();
            currentPage.Show();
            _catchScreen.Hide();
            _timeOutScreen.Hide();
        }
	}

	public void GoToPrevious() {
		var previous = _currentPage;
		_pageIndex = Mathf.Clamp (_pageIndex - 1, 0, _pages.Length - 1);
		_currentPage = _pages [_pageIndex];
		previous.Hide ();
		_currentPage.Show ();
		_catchScreen.Hide ();
		_timeOutScreen.Hide ();
	}
}