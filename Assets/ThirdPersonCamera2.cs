using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Hráè
    public float distance = 5f; // Vzdálenost od hráèe
    public float height = 2f; // Výška kamery nad hráèem
    public float rotationSpeed = 100f; // Rychlost otáèení myší

    private float currentYaw = 0f; // Aktuální rotace kamery
    private float currentPitch = 0f; // Aktuální náklon kamery
    public float pitchMin = -30f; // Minimální úhel náklonu
    public float pitchMax = 60f; // Maximální úhel náklonu

    void LateUpdate()
    {
        if (target == null) return;

        // Získání vstupu z myši
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Aktualizace rotace kamery
        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax); // Omezení vertikální rotace

        // Požadovaná pozice kamery
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);

        transform.position = target.position + offset;

        // Kamera sleduje hráèe
        transform.LookAt(target.position + Vector3.up * 1.5f); // Sleduj hrudník hráèe
    }
}
