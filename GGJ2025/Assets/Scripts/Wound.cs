using UnityEngine;

public class Wound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") {
            GameManager.Instance.GameOver();
        }
    }
}
