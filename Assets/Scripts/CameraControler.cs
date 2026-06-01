using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 1f;
    public float zoomSpeed = 5f;

    public float minZoom = 4f;
    public float maxZoom = 18f;

    private Camera cam;
    private Vector3 lastMouseWorldPosition;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            lastMouseWorldPosition = GetMouseWorldPosition();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 currentMouseWorldPosition = GetMouseWorldPosition();
            Vector3 difference = lastMouseWorldPosition - currentMouseWorldPosition;

            transform.position += difference * panSpeed;
        }
    }

    void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll == 0)
            return;

        cam.orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}