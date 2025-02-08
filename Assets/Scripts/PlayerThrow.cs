using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public Transform handPosition; // The position where the skin tag will spawn from
    public GameObject skinTagPrefab; // The skin tag prefab
    public float throwForce = 100f; // How far the tag will be thrown

    private GameObject currentSkinTag; // The skin tag in the player's hand

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to grab the skin tag
        {
            GrabSkinTag();
        }

        if (Input.GetMouseButtonDown(0) && currentSkinTag != null) // Left-click to throw
        {
            ThrowSkinTag();
        }
    }

    void GrabSkinTag()
    {
        if (currentSkinTag == null)
        {
            // Create a new skin tag and set it in the player's hand
            currentSkinTag = Instantiate(skinTagPrefab, handPosition.position, Quaternion.identity);
            currentSkinTag.transform.SetParent(handPosition); // Attach to hand
            currentSkinTag.transform.localPosition = new Vector3(0, 0, 0); // Keep the tag at the hand position
            currentSkinTag.GetComponent<Rigidbody>().isKinematic = true; // Disable physics to prevent it from falling
        }
    }

    void ThrowSkinTag()
    {
        if (currentSkinTag != null)
        {
            currentSkinTag.transform.SetParent(null); // Detach from hand
            Rigidbody rb = currentSkinTag.GetComponent<Rigidbody>();
            rb.isKinematic = false; // Enable physics for the throw

            // Set collision detection to continuous to prevent it from passing through objects
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Align the throw direction with the camera's forward direction
            Vector3 throwDirection = Camera.main.transform.forward;  // Get the camera's forward direction

            // Apply the throw force in the camera's forward direction
            rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange); // Throw the tag
            currentSkinTag = null;
        }
    }

}

