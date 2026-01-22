using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float floatSpeed = 1.5f;
    public float lifetime = 1f;

    private Transform target;
    private Vector3 startOffset;
    private float timer = 0f;

    public void Setup(int damage, Transform enemyTransform)
    {
        textMesh.text = damage.ToString();
        target = enemyTransform;
        startOffset = Vector3.up * 2f; // start 2 units above enemy
        transform.position = target.position + startOffset;
    }

    void Update()
    {
        if (target != null)
        {
            // Keep position above enemy + float up
            transform.position = target.position + startOffset + Vector3.up * (floatSpeed * timer);
            timer += Time.deltaTime;
        }

        // Face camera
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
