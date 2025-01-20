using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Hr��
    public float distance = 5f; // Vzd�lenost od hr��e
    public float height = 2f; // V��ka kamery nad hr��em
    public float rotationSpeed = 100f; // Rychlost ot��en� my��

    private float currentYaw = 0f; // Aktu�ln� rotace kamery
    private float currentPitch = 0f; // Aktu�ln� n�klon kamery
    public float pitchMin = -30f; // Minim�ln� �hel n�klonu
    public float pitchMax = 60f; // Maxim�ln� �hel n�klonu

    void LateUpdate()
    {
        if (target == null) return;

        // Z�sk�n� vstupu z my�i
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Aktualizace rotace kamery
        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax); // Omezen� vertik�ln� rotace

        // Po�adovan� pozice kamery
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);

        transform.position = target.position + offset;

        // Kamera sleduje hr��e
        transform.LookAt(target.position + Vector3.up * 1.5f); // Sleduj hrudn�k hr��e
    }
}
