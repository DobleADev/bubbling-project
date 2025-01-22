using System.Collections;
using System.Runtime.InteropServices;
using Cinemachine;
using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    [SerializeField] int activePriority = 15;
    [SerializeField] int inactivePriority = 10;
    [SerializeField] string defaultCamera = "DEFAULT_CAM";
    CinemachineVirtualCamera currentCamera;

    // void Start()
    // {
    //     CinemachineBrain brain = CinemachineCore.Instance.GetActiveBrain(0);
    //     currentCamera = (CinemachineVirtualCamera) brain.ActiveVirtualCamera;
    // }

    public void ChangeCamera(CinemachineVirtualCamera nextCamera)
    {
        if (nextCamera == null)
        {
            var defaultCam = GameObject.Find(defaultCamera);
            if (defaultCam == null)
            {
                return;
            }
            nextCamera = defaultCam.GetComponent<CinemachineVirtualCamera>();
        }
        StartCoroutine(CameraSwitchCoroutine(nextCamera, true));
    }

    public void ChangeCamera(string nextCameraName)
    {
        CinemachineVirtualCamera nextCamera = null;
        var nextCameraObject = GameObject.Find(nextCameraName);
        if (nextCameraObject != null)
        {
            nextCamera = nextCameraObject.GetComponent<CinemachineVirtualCamera>();
        }
        ChangeCamera(nextCamera);
    }

    public void ChangeCameraNoTransition(string nextCameraName)
    {
        CinemachineVirtualCamera nextCamera = null;
        var nextCameraObject = GameObject.Find(nextCameraName);
        if (nextCameraObject != null)
        {
            nextCamera = nextCameraObject.GetComponent<CinemachineVirtualCamera>();
        }
        ChangeCameraNoTransition(nextCamera);
    }

    public void ChangeCameraNoTransition(CinemachineVirtualCamera nextCamera)
    {
        if (nextCamera == null)
        {
            var defaultCam = GameObject.Find(defaultCamera);
            if (defaultCam == null)
            {
                return;
            }
            nextCamera = defaultCam.GetComponent<CinemachineVirtualCamera>();
        }
        StartCoroutine(CameraSwitchCoroutine(nextCamera, false));
    }

    IEnumerator CameraSwitchCoroutine(CinemachineVirtualCamera nextCamera, bool waitTransition)
    {
        CinemachineBrain brain = CinemachineCore.Instance.GetActiveBrain(0);
        if (currentCamera != null)
        {
            currentCamera.Priority = inactivePriority;
        }
        nextCamera.Priority = activePriority;

        if (!waitTransition)
        {

            CinemachineBlenderSettings currentBlendSettings = brain.m_CustomBlends;
            brain.m_CustomBlends = null;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            brain.m_CustomBlends = currentBlendSettings;
        }
        currentCamera = nextCamera;
        // yield return null;
    }
}
