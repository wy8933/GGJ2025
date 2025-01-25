using UnityEngine;

public class Wound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Enemy") {
            GameManager.Instance.GameOver();
        }
    }
}
