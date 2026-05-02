using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public void GoToMapCentral()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("Mong Kok Map"); 
        

    }

    public void GoToMapTST()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("TST Map"); 
    }
    public void GoToMapMongKok()
    {
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("Mong Kok Map"); 
    }
    
    public void GoToMainMenu()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("Main Menu"); 
    }

    
}