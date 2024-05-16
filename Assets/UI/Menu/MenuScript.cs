using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Sprite spriteMute;
    [SerializeField] private Sprite spriteSound;
    [SerializeField] private Image imgMute;

    private bool isPlaying = false;

    void Start()
    {
        bool mute = PlayerPrefs.GetInt("mute") == 0 ? true : false;
        imgMute.sprite = mute ? spriteMute : spriteSound;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel") + 1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MuteSound()
    {
        isPlaying = !isPlaying;
        imgMute.sprite = isPlaying ? spriteSound : spriteMute;
        PlayerPrefs.SetInt("mute", isPlaying ? 1 : 0);
    }
}
