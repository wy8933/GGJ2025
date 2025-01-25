using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    public PlayerController player;

    [Header("Health")]
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;

    [Header("Bubble")]
    public Slider bubbleSlider;
    public Gradient bubbleGradient;
    public Image bubbleFill;

    public void Awake()
    {
        Instance = this;
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;

        healthGradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        healthSlider.value = health;

        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void SetMaxBubble(float value)
    {
        bubbleSlider.maxValue = value;

        bubbleGradient.Evaluate(1f);
    }
    public void SetBubble(float value)
    {
        bubbleSlider.value = value;

        bubbleFill.color = bubbleGradient.Evaluate(bubbleSlider.normalizedValue);
    }
}
