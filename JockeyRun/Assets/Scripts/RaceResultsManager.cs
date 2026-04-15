using UnityEngine;
using TMPro;

public class RaceResultsManager : MonoBehaviour
{
    public RaceManager raceManager;

    public HorsePlayer player;
    public HorseAIRacer[] aiRacers;

    public TextMeshProUGUI resultsText;

    private bool[] finished;
    private float[] finishTimes;
    private int totalRacers;
    private int finishedCount;

    private struct ResultEntry
    {
        public int Index;
        public float Time;
    }

    void Start()
    {
        totalRacers = 1 + (aiRacers != null ? aiRacers.Length : 0);
        finished = new bool[totalRacers];
        finishTimes = new float[totalRacers];
        finishedCount = 0;

        if (resultsText != null)
        {
            resultsText.text = "";
        }
    }

    public void RegisterFinish(GameObject racerObject)
    {
        int idx = GetRacerIndex(racerObject);
        if (idx < 0) return;
        if (finished[idx]) return;

        finished[idx] = true;
        finishTimes[idx] = raceManager != null ? raceManager.GetRaceTime() : Time.time;
        finishedCount++;

        if (resultsText != null)
        {
            resultsText.text = BuildResultsString();
        }

        if (finishedCount >= totalRacers)
        {
            if (raceManager != null)
            {
                raceManager.FinishRace();
            }

            SaveBestTimeIfPlayerFinished();
        }
    }

    private int GetRacerIndex(GameObject racerObject)
    {
        if (player != null && racerObject.GetComponent<HorsePlayer>() == player) return 0;

        if (aiRacers != null)
        {
            for (int i = 0; i < aiRacers.Length; i++)
            {
                if (aiRacers[i] != null && racerObject.GetComponent<HorseAIRacer>() == aiRacers[i])
                {
                    return 1 + i;
                }
            }
        }

        return -1;
    }

    private string BuildResultsString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Results:");

        ResultEntry[] entries = new ResultEntry[finishedCount];
        int n = 0;
        for (int i = 0; i < totalRacers; i++)
        {
            if (!finished[i]) continue;
            entries[n] = new ResultEntry { Index = i, Time = finishTimes[i] };
            n++;
        }

        System.Array.Sort(entries, (a, b) => a.Time.CompareTo(b.Time));

        for (int place = 0; place < entries.Length; place++)
        {
            int idx = entries[place].Index;
            float time = entries[place].Time;

            sb.Append(place + 1);
            sb.Append(". ");
            sb.Append(idx == 0 ? "Player" : ("AI " + idx));
            sb.Append(" - ");
            sb.Append(time.ToString("F2"));
            sb.AppendLine("s");
        }

        return sb.ToString();
    }

    private void SaveBestTimeIfPlayerFinished()
    {
        if (!finished[0]) return;

        float playerTime = raceManager != null ? raceManager.GetRaceTime() : Time.time;
        string key = "BestTime_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (!PlayerPrefs.HasKey(key) || playerTime < PlayerPrefs.GetFloat(key))
        {
            PlayerPrefs.SetFloat(key, playerTime);
            PlayerPrefs.Save();
        }
    }
}
