using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [Tooltip("The Y position under which the object is considered out of bounds")]
    public float outOfBoundsY = -1f;

    [Tooltip("The position to teleport the object back to")]
    public Vector3 resetPosition = Vector3.zero;

    [Tooltip("Optional: Reset rotation as well")]
    public bool resetRotation = true;
    public Quaternion resetRotationValue = Quaternion.identity;

    private Rigidbody rb;

    void Start()
    {
        // If your object uses physics, cache the rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < outOfBoundsY)
        {
            TeleportBack();
        }
    }

    void TeleportBack()
    {
        // Reset position
        transform.position = resetPosition;

        // Reset rotation if selected
        if (resetRotation)
            transform.rotation = resetRotationValue;

        // Reset velocity if there's a Rigidbody
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
