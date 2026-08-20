using DG.Tweening;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float kickSpeed = 15f;
    [SerializeField] private Rigidbody rb;
    public Transform goalRightTransform;
    public Transform goalLeftTransform;
    public Transform nearestGoalTransform;

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        goalRightTransform = UIManager.instance.goalRightTransform;
        goalLeftTransform = UIManager.instance.goalLeftTransform;
    }
    public void KickBall()
    {
        Vector3 direction = GetDirectionToNearestGoal();
        float requiredImpulse = GetRequiredKickImpulse();
        rb.AddForce(direction * requiredImpulse, ForceMode.Impulse);

    }
    private float GetRequiredKickImpulse()
    {
        Vector3 direction = GetDirectionToNearestGoal();

        float curSpeed = Vector3.Dot(rb.linearVelocity, direction);
        float requiredSpeed = Mathf.Max(0f, kickSpeed - curSpeed);
        Debug.Log($"Current Speed: {curSpeed}, Required Speed: {requiredSpeed}, Impulse: {rb.mass * requiredSpeed}");
        return rb.mass * requiredSpeed;
    }
    private Vector3 GetDirectionToNearestGoal()
    {
        Transform nearestGoal = GetNearestGoalTransform();
        return (nearestGoal.position - transform.position).normalized;
    }
    public Transform GetNearestGoalTransform()
    {
        float distanceToRightGoal = Vector3.Distance(transform.position, goalRightTransform.position);
        float distanceToLeftGoal = Vector3.Distance(transform.position, goalLeftTransform.position);

        if (distanceToRightGoal < distanceToLeftGoal)
        {
            return goalRightTransform;
        }
        else
        {
            return goalLeftTransform;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.ActiveKickBtn(true);
            UIManager.instance.AddBallController(this);
        }
        if (other.CompareTag("Goal"))
        {
            PlayerController.instance.PlayHappy();
            UIManager.instance.ActiveFireworkEffect();
            gameObject.SetActive(false);
            DOVirtual.DelayedCall(2f, () => UIManager.instance.SetCameraFollow(UIManager.instance.playerController.transform));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.ActiveKickBtn(false);
            UIManager.instance.RemoveBallController(this);
        }
    }
}
