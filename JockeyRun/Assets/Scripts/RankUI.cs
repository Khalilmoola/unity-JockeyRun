using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    public Transform player;
    public Transform[] racers;
    public TextMeshProUGUI rankText;

    void Update()
    {
        if (player == null || racers == null || rankText == null) return;

        int rank = 1;
        for (int i = 0; i < racers.Length; i++)
        {
            if (racers[i] == null) continue;
            if (racers[i].position.x > player.position.x)
            {
                rank++;
            }
        }

        int total = 1;
        for (int i = 0; i < racers.Length; i++)
        {
            if (racers[i] != null) total++;
        }

        rankText.text = rank + "/" + total;
    }
}
