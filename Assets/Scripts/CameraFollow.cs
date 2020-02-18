using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Determines the limitations of vertical camera movement
    [SerializeField]
    public float Y_ANGLE_MIN = 5.0f;
    [SerializeField]
    public float Y_ANGLE_MAX = 35.0f;
    [SerializeField]
    public float distance = -2.0f; // Distance to stay from character, Make sure it is negative
    [SerializeField]
    public float horizontalOffset = 0f;
    [SerializeField]
    public float verticalOffset = 0f;
    [SerializeField]
    public Transform player; //What the camera is looking at..the main character
    [SerializeField]
    public int horizontalAcceleration = 10;
    [SerializeField]
    public int verticalAcceleration = 1;


    private float currentX = 0.0f; // Holds value of X mouse movement
    private float currentY = 0.0f; // Holds value of Y mouse movement

    void start() { }

    void Update()
    {
        if (Input.GetAxis("Mouse X") != null || Input.GetAxis("Mouse Y") != null)
        {
            currentX += Input.GetAxis("Mouse X") * horizontalAcceleration;
            currentY += - Input.GetAxis("Mouse Y") * verticalAcceleration;
        }

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate()
    {                                                        //Rotation around character............/...Keeps distance from character          
        gameObject.transform.position = player.position + Quaternion.Euler(currentY, currentX, 0) * new Vector3(0, 0, distance) + new Vector3(horizontalOffset, verticalOffset, 0);
        gameObject.transform.LookAt(player.position + new Vector3(horizontalOffset, verticalOffset, 0));//Points camera at character
    }
}