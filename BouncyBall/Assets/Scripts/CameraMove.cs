using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows free control of the camera in both horizontal (XZ
// for the sake of this script) and vertical (Y for the sake
// of this script) axes.
public class CameraMove : MonoBehaviour
{
    public float offsetRadiusXZ;
    public float offsetRadiusY;
    public float initXZAngle; // XZ, as in the horizontal plane
    public float initYAngle; // Y, as in the vertical plane
    public float mouseSensXZ;
    public float mouseSensY;
    GameObject player;
    float xzAngle;
    float yAngle;

    private void Start()
    {
        xzAngle = initXZAngle;
        yAngle = initYAngle;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        xzAngle += mouseSensXZ * Input.GetAxis("Mouse X") * Time.deltaTime;
        yAngle += mouseSensY * Input.GetAxis("Mouse Y") * Time.deltaTime;
        yAngle = Mathf.Clamp(yAngle, 0, 1.57f);
        Vector3 xzOffset =
            new Vector3(
                Mathf.Cos(xzAngle) * Mathf.Abs(Mathf.Cos(yAngle)),
                0,
                Mathf.Sin(xzAngle) * Mathf.Abs(Mathf.Cos(yAngle))) * offsetRadiusXZ;
        Vector3 yOffset = new Vector3(0, Mathf.Sin(yAngle), 0) * offsetRadiusY;
        transform.position = player.transform.position + xzOffset + yOffset;
        transform.LookAt(player.transform);
    }

    public void SetMouseSensXZ(float num)
    {
        mouseSensXZ = num;
    }

    public void SetMouseSensY(float num)
    {
        mouseSensY = num;
    }
}
