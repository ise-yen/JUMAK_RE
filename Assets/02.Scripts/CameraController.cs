using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    PlayerSkill playerskill;
    GameObject playerPosCam;
    CinemachineVirtualCamera cineCam;
    Transform cineFollowTarget;

    [SerializeField]

    private void Awake()
    {
        playerskill = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkill>();

        playerPosCam = GameObject.FindGameObjectWithTag("PosCam");

        cineCam = GetComponent<CinemachineVirtualCamera>();
        cineCam.Follow = playerPosCam.transform;
    }

    private void Update()
    {
    }
}
