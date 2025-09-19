using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;  // 플레이어 Transform 드래그해서 연결
    public float smoothSpeed = 0.125f;  // 따라가는 부드러움 정도
    public Vector3 offset;   // 카메라와 플레이어 간격
    private float targetx = 0;
    private float targety = 0;

    void LateUpdate()
    {
        targetx = player.transform.position.x <= 0 ? 0 : player.transform.position.x;
        targety = player.transform.position.y <= 0 ? 0 : player.transform.position.y;
        Vector3 targetPos = new Vector3(targetx, targety, transform.position.z) + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        transform.position = smoothPos;
    }
}
