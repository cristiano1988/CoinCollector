using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7.2f;
    public float rotationSpeed = 180f;

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * moveInput * moveSpeed * Time.deltaTime, Space.Self);

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.A))
            rotationInput = 1f;
        else if (Input.GetKey(KeyCode.D))
            rotationInput = -1f;

        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, GameBounds.minX, GameBounds.maxX);
        float clampedY = Mathf.Clamp(transform.position.y, GameBounds.minY, GameBounds.maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}