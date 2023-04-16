using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public                 GameObject  PauseMenu,         GameOverMenu,      MainMenu, SettingsMenu, GamePlay;
    public                 TMP_Text    GameOverScoreText, MainMenuScoreText, IngameScoreText;
    public                 AudioSource AudioSource;
    public static readonly UnityEvent  StartGame   = new();
    public static readonly UnityEvent  PauseGame   = new();
    public static readonly UnityEvent  UnPauseGame = new();

    private int _score;

    private bool _enableVibration = true;
    private bool _enableSound     = true;

    private void UpdateScore()
    {
        _score                 += 1;
        GameOverScoreText.text =  "Your score: " + _score;
        MainMenuScoreText.text =  "Best score: " + _score;
        IngameScoreText.text   =  "Score :"      + _score;

        if (_enableVibration)
        {
            Handheld.Vibrate();
        }
    }

    private void OnEnable()
    {
        BallController.EndGame.AddListener(OnEndGameCalled);
        BallController.BallTouched.AddListener(UpdateScore);
    }

    private void OnEndGameCalled()
    {
        TriggerEndGame();
    }

    public void ChangeVibrationState(bool state)
    {
        _enableVibration = state;
    }

    public void ChangeSoundState(bool state)
    {
        _enableSound     = state;
        AudioSource.mute = !_enableSound;
    }

    public void PlayButton()
    {
        TriggerStartGame();
    }

    public void SettingsButton()
    {
        SettingsMenu.SetActive(true);
    }

    public void PauseButton()
    {
        PauseGame?.Invoke();
        PauseMenu.SetActive(true);
    }

    public void BackToMenuButton()
    {
        SettingsMenu.SetActive(false);
    }

    public void MenuButton()
    {
        GamePlay.SetActive(false);
        PauseMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void UnpauseGameButton()
    {
        PauseMenu.SetActive(false);
        UnPauseGame?.Invoke();
    }

    private void TriggerStartGame()
    {
        GamePlay.SetActive(true);
        StartGame?.Invoke();
        MainMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    private void TriggerEndGame()
    {
        GameOverMenu.SetActive(true);
    }
}