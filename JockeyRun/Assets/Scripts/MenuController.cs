using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject instructionPanel; 

    //bool to track if the game has already been opened before 
    private bool pendingGameStart = false;

    void Start()
    {   
        //hide the panel initially 
        if(instructionPanel != null)
        {
            instructionPanel.SetActive(false); 
        }
    }

    void Update()
    {   //listen for key presses if the how to play thing is visible 
        if(instructionPanel != null && instructionPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                instructionPanel.SetActive(false);
                pendingGameStart = false; 

            }
            else if (Input.anyKeyDown)
            {
                if (pendingGameStart)
                {
                    SceneManager.LoadScene("Map Menu");
                }
                else
                {
                    instructionPanel.SetActive(false); //any button closes it 
                }
            }
        }
    }

    public void GoToMapMenu()
    {
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);

        if(PlayerPrefs.GetInt("HasPlayedBefore",0) == 0)
        {   
            //set flag so it does not trigger again 
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            PlayerPrefs.Save();

            //set the flag to true and show the panel 
            pendingGameStart = true;
            if(instructionPanel != null) instructionPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Map Menu");
        }

    }

    //this function is to show the instruction panel 
    public void ShowInfo()
    {
        AudioManager.Instance.PlaySfx(AudioEvent.MenuButton);
        
        //dont start the game yet tho 
        pendingGameStart = false;
        if (instructionPanel != null) instructionPanel.SetActive(true);
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

    public void ResetSaveData()
    {
        //debug tool 
        PlayerPrefs.DeleteKey("HasPlayedBefore");
        Debug.Log("Reset! Clicking Play will show the How To Play screen again.");
    }
}