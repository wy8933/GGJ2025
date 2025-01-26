using ObjectPoolings;
using System.Collections;
using System;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;
    public Camera mainMamera;
    public GameObject bulletPrefab;
    public Stats Stats;
    public WeaponType weaponType;
    public GameObject playerModel;

    [Header("Movement/Rotation")]
    public float maxSpeed;
    public Vector2 moveDirection;
    public Vector2 mousePosition;

    [Header("Bubble")]
    public Transform firepoint;
    public float maxBubble;
    public float currentBubble;
    public float bubbleHealthDeduct;
    public float bubbleGainAmount;
    public float bubbleCost;

    [Header("Machine Gun")]
    public bool isShooting;
    public float shootCooldown;
    public float shootshootCooldownTimer;
    public int machineGunAngleOffset;

    [Header("Shot Gun")]
    public float shotGunBubbleCost;
    public int bulletCount;
    public int shotGunAngleOffset;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _playerRB.maxLinearVelocity = maxSpeed;
        HUDManager.Instance.SetMaxHealth(Stats.MaxHealth);
        HUDManager.Instance.SetMaxBubble(maxBubble);
        HUDManager.Instance.SetHealth(Stats.Health);
        HUDManager.Instance.SetBubble(currentBubble);
    }

    void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }

    private void Update()
    {
        if (isShooting) 
        {
            shootshootCooldownTimer -= Time.deltaTime;
            if (shootshootCooldownTimer < 0) 
            {
                Attack();
                shootshootCooldownTimer += shootCooldown;
            }
        }
    }

    public void PlayerMovement() 
    {
        // Physics doesn't need delta time
        _playerRB.AddForce(new Vector3(moveDirection.x,0,moveDirection.y) * Stats.MovementSpeed);
    }

    public void PlayerRotation()
    {
        Ray ray = mainMamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = (targetPosition - transform.position).normalized;

            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * Stats.RotationSpeed);
            }
        }
    }

    public void Attack() 
    {
        switch (weaponType)
        {
            case WeaponType.Scatter:
                if (currentBubble >= bubbleCost) {
                    SpawnBullet(machineGunAngleOffset);
                    ReduceBubble(bubbleCost);
                }
                break;
            case WeaponType.ShotGun:
                if (currentBubble >= shotGunBubbleCost) {
                    for (int i = 0; i < bulletCount; i++)
                    {
                        SpawnBullet(shotGunAngleOffset);
                    }
                    ReduceBubble(shotGunBubbleCost);
                }
                break;
            case WeaponType.RapidFire:
                if (currentBubble >= bubbleCost)
                {
                    SpawnBullet(0);
                    ReduceBubble(bubbleCost);
                }
                break;
        }
    }

    public void SpawnBullet(int offset)
    {
        Vector3 currentRotation = playerModel.transform.eulerAngles;
        float randomOffset = Random(offset);
        currentRotation.y += randomOffset + 180;
        Quaternion rotation = Quaternion.Euler(currentRotation);

        var (objectInstance, pool) = ObjectPooling.GetOrCreate(bulletPrefab, firepoint.position, rotation);
        var bulletController = objectInstance.GetComponent<BulletController>();

        if (bulletController)
        {
            bulletController.InitBullet(pool, Stats.AtkMultiplier);
        }
    }

    private float Random(int num)
    { 
        return new System.Random().Next(-num,num) - new System.Random().Next(-num, num);
    }

    public void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        HUDManager.Instance.SetHealth(Stats.Health);

        if (Stats.Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
    }

    public void GainBubble(float amount)
    {
        currentBubble += amount;

        if (currentBubble > maxBubble)
        { 
            currentBubble = maxBubble;
        }
        HUDManager.Instance.SetBubble(currentBubble);
    }

    public void ReduceBubble(float amount)
    {
        currentBubble -= amount;

        if (currentBubble < 0)
        {
            currentBubble = 0;
        }

        HUDManager.Instance.SetBubble(currentBubble);
    }

    public void WaterLogic() {
        TakeDamage(bubbleHealthDeduct);
        GainBubble(bubbleGainAmount);
    }
}
