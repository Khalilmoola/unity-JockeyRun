using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HorsePlayer player = other.GetComponent<HorsePlayer>();

        if (player != null)
        {
            player.ApplySpeedMultiplier(boostMultiplier, boostDuration);
        }
    }
}
