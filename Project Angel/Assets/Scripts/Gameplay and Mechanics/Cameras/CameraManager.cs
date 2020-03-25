using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance;

    public Camera playerCamera;
    public Camera battleCamera;

    public enum CameraType { Player, Battle };

    private void Start()
    {
        UpdateCamera(CameraType.Player);
    }

    public void UpdateCamera(CameraType type)
    {

        switch (type)
        {
            case CameraType.Battle:
                playerCamera.gameObject.SetActive(false);
                battleCamera.gameObject.SetActive(true);
                break;
            case CameraType.Player:
                playerCamera.gameObject.SetActive(true);
                battleCamera.gameObject.SetActive(false);
                break;
            default:
                break;
        }

    }

    private void Awake()
    {
        Instance = this;
    }

}
