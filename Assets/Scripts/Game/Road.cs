using UnityEngine;

public class Road : MonoBehaviour
{
    [Header("Player Transform")]
    [SerializeField] private Transform playerBall;
    
    private float _initialBallScale;
    private float _initialRoadWidth;

    private void Start()
    {
        _initialBallScale = playerBall.localScale.x;
        _initialRoadWidth = transform.localScale.x;
        
        AdjustRoadWidth();
    }

    private void Update()
    {
        AdjustRoadWidth();
    }

    private void AdjustRoadWidth()
    {
        float currentBallScale = playerBall.localScale.x;
        float newRoadWidth = _initialRoadWidth * (currentBallScale / _initialBallScale);
        
        Vector3 newScale = transform.localScale;
        newScale.x = newRoadWidth;
        transform.localScale = newScale;
    }
}