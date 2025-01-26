using ObjectPoolings;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    public Stats Stats;
    public Transform player;
    public NavMeshAgent agent;
    public PrefabPool pool;
    public bool isReleased;
    public AudioSource audioSource;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyPathFinding();
    }

    public void InitEnemy(PrefabPool pool) 
    {
        Stats.Health = Stats.MaxHealth;
        this.pool = pool;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Stats.MovementSpeed;
        isReleased = false;
        ApplyScaling(EnemyWaveManager.Instance.enemyIncreaseFactor, EnemyWaveManager.Instance.currentWaveIndex);
    }

    protected virtual void EnemyPathFinding()
    {
        Vector3 woundPosition = Wound.Instance.transform.position;
        Vector3 targetPosition = woundPosition;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float distanceToWound = Vector3.Distance(transform.position, woundPosition);

            // If the player is closer than the wound, target the player instead
            if (distanceToPlayer <= distanceToWound)
            {
                targetPosition = player.position;
            }
        }

        agent.SetDestination(targetPosition);
        
    }

    public void TakeDamage(float damage) {
        Stats.Health -= damage;

        if (Stats.Health <= 0) {
            Die();
        }
    }

    private void Die() 
    {
        if (!isReleased) {
            audioSource.Play();
            isReleased = true;
            EnemyWaveManager.Instance.EnemyDefeated();
            pool.Release(gameObject);
        }
    }

    private void BubbleDeathAnimation() { 
        
    }

    public void ApplyScaling(float enemyIncreaseFactor, int waveNumber)
    {
        float scalingFactor = Mathf.Pow(enemyIncreaseFactor, waveNumber);

        Stats.MaxHealth *= scalingFactor * Stats.HealthMultiplier;
        Stats.Health = Stats.MaxHealth;
        Stats.MovementSpeed *= scalingFactor * Stats.SpeedMultiplier;
        Stats.RotationSpeed *= scalingFactor * Stats.SpeedMultiplier;
        Stats.SprintSpeed *= scalingFactor * Stats.SpeedMultiplier;
        Stats.Resistance *= scalingFactor * Stats.ResistanceMultiplier;
        Stats.Shield *= scalingFactor * Stats.DamageReductionMultiplier;
        Stats.DamageReduction *= scalingFactor * Stats.DamageReductionMultiplier;
        Stats.BlockChance *= scalingFactor;
        Stats.SlowResistance *= scalingFactor;

        Debug.Log($"[Scaling Applied] Wave: {waveNumber}, Factor: {scalingFactor}");
    }

}
