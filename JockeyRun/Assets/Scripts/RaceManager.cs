using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public HorsePlayer playerScript;
    
    private float raceTime = 0f;
    private bool raceActive = false;

    void Start() => StartCoroutine(StartCountdown());

    IEnumerator StartCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO!";
        
        playerScript.canMove = true;
        raceActive = true;
        
        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Hide text
    }

    void Update()
    {
        if (raceActive)
        {
            raceTime += Time.deltaTime;
            timerText.text = raceTime.ToString("F2") + "s";
        }
    }

    public void FinishRace()
    {
        raceActive = false;
        playerScript.canMove = false;
        countdownText.text = "FINISHED!";
        Invoke("ReturnToMenu", 10f); // Wait 10 seconds then leave
    }

    void ReturnToMenu() => SceneManager.LoadScene("Map Menu");
}