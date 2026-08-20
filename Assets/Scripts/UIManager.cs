using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] internal PlayerController playerController;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private bool isAutoKickActive = false;
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private List<BallController> ballControllers;
    [SerializeField] private BallController nearestBallController;
    public Transform goalRightTransform;
    public Transform goalLeftTransform;
    public GameObject fireworkEffectGO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        isAutoKickActive = false;
        if (kickButton != null)
        {
            kickButton.gameObject.SetActive(false);
            kickButton.onClick.AddListener(Kick);
        }
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetScene);
        }
        if (autoKickButton != null)
        {
            autoKickButton.onClick.AddListener(ToggleAutoKick);
        }
        fireworkEffectGO.SetActive(false);
        autoKickButton.GetComponentInChildren<TextMeshProUGUI>().text = isAutoKickActive ? "Auto Kick: ON" : "Auto Kick: OFF";

    }

    private void ToggleAutoKick()
    {
        isAutoKickActive = !isAutoKickActive;
        autoKickButton.GetComponentInChildren<TextMeshProUGUI>().text = isAutoKickActive ? "Auto Kick: ON" : "Auto Kick: OFF";
    }

    void Update()
    {
        if (isAutoKickActive)
        {
            Kick();
        }
    }
    // public void Kick()
    // {
    //     foreach (var ballController in ballControllers)
    //     {
    //         ballController.KickBall();
    //     }
    // }
    public void Kick()
    {
        nearestBallController = GetNearestBallController();
        if (nearestBallController != null)
        {
            nearestBallController.KickBall();
            SetCameraFollow(nearestBallController.transform);
        }
    }
    public void SetCameraFollow(Transform target)
    {
        virtualCamera.Follow = target;
    }
    private BallController GetNearestBallController()
    {
        nearestBallController = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var ballController in ballControllers)
        {
            float distance = Vector3.Distance(ballController.transform.position, playerController.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBallController = ballController;
            }
        }

        return nearestBallController;
    }
    public void ActiveKickBtn(bool isActive)
    {
        kickButton.gameObject.SetActive(isActive);
    }

    public void ActiveFireworkEffect()
    {
        fireworkEffectGO.SetActive(true);
        DOVirtual.DelayedCall(2f, () => fireworkEffectGO.SetActive(false));
    }
    public void AddBallController(BallController ballController)
    {
        if (!ballControllers.Contains(ballController))
        {
            ballControllers.Add(ballController);
        }
    }

    public void RemoveBallController(BallController ballController)
    {
        if (ballControllers.Contains(ballController))
        {
            ballControllers.Remove(ballController);
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
