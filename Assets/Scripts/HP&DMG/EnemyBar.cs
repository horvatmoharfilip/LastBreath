using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;            // Slider component of health bar
    public Gradient gradient;        // Optional gradient for fill color
    public Image fill;               // The fill image of the slider
    public Transform target;         // The enemy to follow
    public Vector3 offset = new Vector3(0, 2f, 0); // position above enemy

    private void LateUpdate()
    {
        if (target != null)
        {
            // Follow the enemy position + offset
            transform.position = target.position + offset;

            // Make the health bar face the camera
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

    // Initialize health bar with max health
    public void SetMaxHealth(int maxHealth)
    {
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        if (fill != null)
            fill.color = gradient.Evaluate(1f);
    }

    // Update health bar value
    public void SetHealth(int currentHealth)
    {
        if (slider != null)
            slider.value = currentHealth;

        if (fill != null)
            fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
