using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam instance;

    public float sensX = 1600f;
    public float sensY = 1600f;
    public Transform orientation;

    float xRotation;
    float yRotation;

    bool cameraEnabled = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CameraLock(true); // lock/look enabled when game starts
    }

    public void CameraLock(bool locked)
    {
        cameraEnabled = locked;

        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (!cameraEnabled)
            return; // 🔥 stops camera movement completely when UI is open

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
