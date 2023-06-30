using Cinemachine;
using UnityEngine;

public class CameraZoomINZoomOut : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    public float FOV = 40f;
    public float zoomValue = 1f;
    public float rateSpeed = 10;

    [Tooltip("joomOutMax ���� �������� �־��ֽÿ�")]
    public int joomInMax = 40;
    [Tooltip("joomInMax ���� ���� ���� �־��ֽÿ�")]
    public int joomOutMax = 60;

    public Transform cameraRoot;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        ZoomInOut();
        virtualCamera.m_Lens.FieldOfView = FOV;
    }

    void ZoomInOut()
    {
        if (virtualCamera.m_Lens.FieldOfView >= joomInMax && InputManager.Instance.scrollWheel > 0 )
        {
            FOV -= zoomValue;
        }
        else if (virtualCamera.m_Lens.FieldOfView <= joomOutMax && InputManager.Instance.scrollWheel < 0)
        {
            FOV += zoomValue;
        }


        transform.LookAt(cameraRoot);
    }

}
