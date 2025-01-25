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
    public PlayerStats Stats;
    public WeaponType weaponType;

    [Header("Movement/Rotation")]
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public Vector2 moveDirection;
    public Vector2 mousePosition;


    [Header("Machine Gun")]
    public bool isShooting;
    public float shootCooldown;
    public float shootshootCooldownTimer;
    public int machineGunAngleOffset;

    [Header("Shot Gun")]
    public int bulletCount;
    public int shotGunAngleOffset;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _playerRB.maxLinearVelocity = maxSpeed;
    }

    void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }

    private void Update()
    {
        if (isShooting) {
            shootshootCooldownTimer -= Time.deltaTime;
            if (shootshootCooldownTimer < 0) {
                Attack();
                shootshootCooldownTimer += shootCooldown;
            }
        }
    }

    public void PlayerMovement() {
        // Physics doesn't need delta time
        _playerRB.AddForce(new Vector3(moveDirection.x,0,moveDirection.y) * speed);
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
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    public void Attack() {
        switch (weaponType)
        {
            case WeaponType.MachineGun:
                SpawnBullet(machineGunAngleOffset);
                break;
            case WeaponType.Shotgun:
                for (int i = 0; i < bulletCount; i++) {
                    SpawnBullet(shotGunAngleOffset);
                }
                break;
        }
    }

    public void SpawnBullet(int offset) {
        Vector3 currentRotation = transform.eulerAngles;
        float randomOffset = Random(offset);
        currentRotation.y += randomOffset;
        Quaternion rotation = Quaternion.Euler(currentRotation);

        var (objectInstance, pool) = ObjectPooling.GetOrCreate(bulletPrefab, transform.position, rotation);
        var bulletController = objectInstance.GetComponent<BulletController>();

        if (bulletController) {
            bulletController.InitBullet(pool);
        }
    }

    private float Random(int num) { 
        return new System.Random().Next(-num,num) - new System.Random().Next(-num, num);
    }

    public void TakeDamage(float damage) {
        Stats.Health -= damage;

        if (Stats.Health <= 0) {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player has died!");
    }
}
