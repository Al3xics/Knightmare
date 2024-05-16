using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private string[] tags;

    void Awake()
    {
        if (PlayerPrefs.GetInt("mute") == 0)
        {
            GetComponent<AudioSource>().enabled = false;
        }
    }

    void Start()
    {
       foreach (string tag in tags)
       {
            GameObject[] go = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject g in go)
            {
                g.SetActive(false);
            }
       }

       GameObject.Find("Music Background").GetComponent<AudioSource>().enabled = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
