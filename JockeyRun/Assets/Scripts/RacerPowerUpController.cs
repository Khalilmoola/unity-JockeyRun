using UnityEngine;

public class RacerPowerUpController : MonoBehaviour
{
    public PowerUpType equippedPowerUp;
    public bool hasPowerUp;

    public float speedBoostMultiplier = 1.5f;
    public float speedBoostDurationSeconds = 1.5f;
    public float shieldDurationSeconds = 1.5f;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 12f;

    public bool isPlayer;
    public KeyCode activateKey = KeyCode.E;

    private HorsePlayer player;
    private HorseAIRacer ai;

    void Awake()
    {
        player = GetComponent<HorsePlayer>();
        ai = GetComponent<HorseAIRacer>();
    }

    void Update()
    {
        if (!hasPowerUp) return;

        if (isPlayer)
        {
            if (Input.GetKeyDown(activateKey)) Activate();
        }
        else
        {
            
            if (Random.Range(0f, 100f) < 1f) // activate the power up with a delay 
            {
                Activate();
            }
        }
    }


    public void GivePowerUp(PowerUpType type)
    {
        equippedPowerUp = type;
        hasPowerUp = true;
    }

    //check which entity to apply it to 
    public void Activate()
    {
        if (!hasPowerUp) return;

        if (player != null) ApplyToPlayer(player);
        else if (ai != null) ApplyToAI(ai);

        hasPowerUp = false;
    }
    
    //logic for the player 
    private void ApplyToPlayer(HorsePlayer p)
    {
        switch (equippedPowerUp)
        {
            case PowerUpType.SpeedBoost:
                p.ApplySpeedMultiplier(speedBoostMultiplier, speedBoostDurationSeconds);
                break;

            case PowerUpType.Shield:
                p.ApplyShield(shieldDurationSeconds);
                break;


            case PowerUpType.Projectile:
                FireProjectile();
                break;
            case PowerUpType.FreezeTrap:
                //if they hit a trap, then they take damage 
                p.TakeFreeze(2.0f);
                break;
        }
    }

    //logic for the ai 
    private void ApplyToAI(HorseAIRacer a)
    {
        switch (equippedPowerUp)
        {
            case PowerUpType.SpeedBoost:
                a.ApplySpeedMultiplier(speedBoostMultiplier, speedBoostDurationSeconds);
                break;

            case PowerUpType.Shield:
                a.ApplyShield(shieldDurationSeconds);
                break;

            case PowerUpType.Projectile:
                FireProjectile();
                break;
            case PowerUpType.FreezeTrap:
                a.TakeFreeze(2.0f);
                break;
        }
    }

    //logic for shooting the projectile, from the player to the AI 
    private void FireProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
        GameObject go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projectile = go.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.owner = gameObject;
            projectile.speed = projectileSpeed;
            projectile.direction = Vector2.right;
        }
        else
        {
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.right * projectileSpeed;
        }
    }
}