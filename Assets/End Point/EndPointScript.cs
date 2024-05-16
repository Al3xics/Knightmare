using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScript : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<KnightScript>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Win");
            GameObject.Find("Music Background").GetComponent<AudioSource>().enabled = false;
            audioSource.Play();
            particle.SetActive(true);
            StartCoroutine(WaitForEndMusic());
        }
    }

    private IEnumerator WaitForEndMusic()
    {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
