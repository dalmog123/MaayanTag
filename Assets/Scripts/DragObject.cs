using UnityEngine;

public class DragObject : MonoBehaviour
{
    public LineRenderer lineLeft;
    public LineRenderer lineRight;
    public Transform leftAnchor;
    public Transform rightAnchor;
    public float returnSpeed = 10f;  // Adjust speed as needed
    private bool isReturning = false;
    private Vector3 startPosition;
    private Vector3 mouseOffset;
    private float mouseZCoord;
    void Start()
    {
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        mouseZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mouseOffset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mouseOffset;
    }
    void OnMouseUp()
    {
        isReturning = true;
    }


    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void Update()
    {
        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, returnSpeed * Time.deltaTime);

            // Stop moving when very close to the start position
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                transform.position = startPosition;
                isReturning = false;
            }
        }

        // Keep updating the slingshot lines
        Vector3 leftEdge = transform.position + transform.right * -0.03f;  // Adjust for new scale
        Vector3 rightEdge = transform.position + transform.right * 0.03f;

        lineLeft.SetPosition(0, leftAnchor.position);
        lineLeft.SetPosition(1, leftEdge);

        lineRight.SetPosition(0, rightAnchor.position);
        lineRight.SetPosition(1, rightEdge);
    }



}
