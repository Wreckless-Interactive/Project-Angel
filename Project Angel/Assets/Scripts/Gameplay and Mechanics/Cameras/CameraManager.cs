using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance;

    public Camera playerCamera;
    public Camera battleCamera;

    [Header("Battle Camera")]
    public Vector3 charSelectOffset;
    private Vector3 startPos;
    private Vector3 moveToPos;

    public enum CameraType { Player, Battle };

    private void Start()
    {
        UpdateCamera(CameraType.Player);
        startPos = battleCamera.transform.position;
        ResetBattleCameraPosition();
        ResetBattleCameraPosition();
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

    public void SetBattleCameraPosition(Vector3 newPos)
    {
        moveToPos = newPos + charSelectOffset;
    }

    public void ResetBattleCameraPosition()
    {
        moveToPos = startPos;
    }

    private void Update()
    {
        battleCamera.transform.position = Vector3.MoveTowards(battleCamera.transform.position, moveToPos, Time.deltaTime * 40);
    }

    private void Awake()
    {
        Instance = this;
    }

}
