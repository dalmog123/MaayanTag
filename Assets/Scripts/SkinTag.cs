using UnityEngine;

public class SkinTag : MonoBehaviour
{
    public float stickDuration = 3f;
    public string bossTag = "Boss";
    private bool isStuck = false;
    private bool hasBeenUsed = false;  // New flag to track if it's been used
    private float timer = 0f;
    private Transform originalParent;

    private void Start()
    {
        originalParent = transform.parent;
    }

    private void Update()
    {
        if (isStuck)
        {
            timer += Time.deltaTime;
            if (timer >= stickDuration)
            {
                DetachFromBoss();
            }
        }

        // Check if the skin tag is below -5 on the Y-axis and delete it if so
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only stick if it hasn't been used before
        if (collision.gameObject.CompareTag(bossTag) && !isStuck && !hasBeenUsed)
        {
            StickToBoss(collision.transform);
        }
    }

    private void StickToBoss(Transform bossTransform)
    {
        isStuck = true;
        hasBeenUsed = true;  // Mark it as used
        timer = 0f;

        transform.parent = bossTransform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DetachFromBoss()
    {
        isStuck = false;
        timer = 0f;
        transform.parent = null; // Completely detach from any parent

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
        }
    }
}
