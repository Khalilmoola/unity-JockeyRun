using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToMapMenu()
    {
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("Map Menu"); 
    }

    public void BackToMainMenu()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        Application.Quit();
    }
}