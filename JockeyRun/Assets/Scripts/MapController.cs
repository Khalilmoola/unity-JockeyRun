using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public void GoToMapCentral()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("khalilMap1"); 
    }

    public void GoToMapTST()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("khalilMap1"); 
    }
    public void GoToMapMongKok()
    {
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("khalilMap1"); 
    }
    
    public void GoToMainMenu()
    {   
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        SceneManager.LoadScene("khalilMap1"); 
    }

    
}