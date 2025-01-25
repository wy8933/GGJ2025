using UnityEngine;

public class Wound : MonoBehaviour
{
    public static Wound Instance;

    private void Start()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") {
            GameManager.Instance.GameOver();
        }
    }
}
