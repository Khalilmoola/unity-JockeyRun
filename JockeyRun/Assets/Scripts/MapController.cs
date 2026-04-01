using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public void GoToMapCentral()
    {
        SceneManager.LoadScene("Central Map"); 
    }

    public void GoToMapTST()
    {
        SceneManager.LoadScene("TST Map"); 
    }
    public void GoToMapMongKok()
    {
        SceneManager.LoadScene("Mong Kok Map"); 
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); 
    }
}