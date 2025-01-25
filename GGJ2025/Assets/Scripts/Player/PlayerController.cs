using ObjectPoolings;
using System;
using UnityEngine;
using ObjectPoolings;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;
    public Camera mainMamera;
    [Header("Movement/Rotation")]
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public Vector2 moveDirection;
    public Vector2 mousePosition;

    public GameObject bulletPrefab;

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
        SpawnBullet();
    }

    public void SpawnBullet() {
        var (objectInstance, pool) = ObjectPooling.GetOrCreate(bulletPrefab, transform.position, transform.rotation);
        var bulletController = objectInstance.GetComponent<BulletController>();

        if (bulletController) {
            bulletController.InitBullet();
        }

        var lifetime = TimeSpan.FromSeconds(1.0f);
        (objectInstance, pool).TimedRelease(lifetime);
    }
}
