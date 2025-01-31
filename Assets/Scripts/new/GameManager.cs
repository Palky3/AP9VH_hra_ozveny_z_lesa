using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TextMeshProUGUI congratulationText;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] GameObject healthBar;

    public static bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        congratulationText.enabled = false;

        questText.enabled = false;
        healthBar.gameObject.SetActive(false);

        StartCoroutine(WaitForCutsceneEnd());
    }

    IEnumerator WaitForCutsceneEnd()
    {
        yield return new WaitForSeconds(18f);
        questText.enabled = true;
        healthBar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void showVictoryText()
    {
        congratulationText.enabled = true;
        congratulationText.gameObject.SetActive(true);
        congratulationText.CrossFadeAlpha(0f, 6f, false);
    }

    public void ChangeQuestText()
    {
        questText.text = "Mùžeš volnì prozkoumávat, ale nic zajímavého tì už neèeká.";
    }
}
