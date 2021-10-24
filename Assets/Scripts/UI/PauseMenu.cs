using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private Movement _movement;

    public void CloseGame()
    {
        Application.Quit();
    }
    
    private void OnEnable()
    {
        _movement = FindObjectOfType<Movement>();
        
        _movement.pause += OnPause;
    }

    private void OnDisable()
    {
        _movement.pause -= OnPause;
    }

    private void OnPause()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
