using UnityEngine;

public class BossRise : MonoBehaviour
{
    public GameObject groundObject;
    public float riseSpeed = 5f;
    public float startingDepth = -80f; // How far below ground to start

    private void Start()
    {
        // Set initial position lower than current
        Vector3 startPos = transform.position;
        startPos.y = startingDepth;
        transform.position = startPos;
    }

    private void Update()
    {
        if (transform.position.y < groundObject.transform.position.y-2f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            newPosition.y = Mathf.Min(newPosition.y, groundObject.transform.position.y);
            transform.position = newPosition;
        }
    }
}