using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TimerScript : MonoBehaviour
{
    public bool started = false;
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject FailPanel;
    public TextMeshProUGUI textMeshProUGUI;
    public float timer = 24;

    void Update()
    {
        if (started)
        {
            timer -= Time.deltaTime;
            int txtt = (int)timer;
            textMeshProUGUI.text = txtt.ToString();
            if (timer < 0)
            {
                FailPanel.SetActive(true);
            }
        }
    }
    public void StartGame()
    {
        started = true;
        StartPanel.SetActive(false);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
