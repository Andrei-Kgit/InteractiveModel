using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float _zoomSpeed = 1f;
    private CinemachineFreeLook _freeLookCam;
    private CinemachineFreeLook.Orbit[] _orbits;

    private void Start()
    {
        _freeLookCam = GetComponent<CinemachineFreeLook>();
        _orbits = _freeLookCam.m_Orbits;
    }

    public void SetTarget(Transform target)
    {
        _freeLookCam.Follow = target;
        _freeLookCam.LookAt = target;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            for (int i = 0; i < _orbits.Length; i++)
            {
                if (i == 1)
                    _orbits[i].m_Radius += (_zoomSpeed * 2) * Time.deltaTime;
                else
                    _orbits[i].m_Radius += _zoomSpeed * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            for (int i = 0; i < _orbits.Length; i++)
            {
                if (i == 1)
                    _orbits[i].m_Radius -= (_zoomSpeed * 2) * Time.deltaTime;
                else
                    _orbits[i].m_Radius -= _zoomSpeed * Time.deltaTime;
            }
        }
    }
}
