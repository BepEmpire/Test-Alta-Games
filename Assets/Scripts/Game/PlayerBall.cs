using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerBall : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Events")]
    [SerializeField] private UnityEvent onGameOver;
    
    [Header("Prefabs")]
    [SerializeField] private ShotBall shotPrefab;
    
    [Header("Ball Settings")]
    [SerializeField] private float initialSize = 1.2f;
    [SerializeField] private float minSize = 0.25f;
    [SerializeField] private float moveSpeed = 2.0f;
    
    [Header("Shot Settings")]
    [SerializeField] private float shotScaleFactor = 0.75f;
    [SerializeField] private float shotOffset = 2.5f;
    
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float raycastLength = 7.0f;

    private ShotBall _currentShot;
    
    private bool _isCharging = false;
    
    private float _currentSize;
    private float _rayLength;

    private void Start()
    {
        _currentSize = initialSize;
        transform.localScale = Vector3.one * _currentSize;
        _rayLength = transform.localScale.z * raycastLength;
    }

    private void Update()
    {
        DrawRay();
        
        if (IsPathClear())
        {
            EnableMovement();
        }

        if (_isCharging && _currentShot != null)
        {
            ChargeShot();
        }
    }

    private void DrawRay()
    {
        Debug.DrawRay(transform.position, transform.forward * _rayLength, Color.red);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPathClear()) return;
        
        if (_currentSize > minSize)
        {
            _isCharging = true;
            InstantiateShotBall();
        }
        else
        {
            GameOver();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isCharging)
        {
            _isCharging = false;
            ReleaseShot();
        }
    }
    
    private void EnableMovement()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void InstantiateShotBall()
    {
        _currentShot = Instantiate(shotPrefab, transform.position + transform.forward * shotOffset, Quaternion.identity);
    }
    
    private void ChargeShot()
    {
        if (_currentSize > minSize)
        {
            float chargeAmount = Time.deltaTime * shotScaleFactor;
            _currentSize -= chargeAmount;
            transform.localScale = Vector3.one * _currentSize;

            _currentShot.transform.localScale += Vector3.one * chargeAmount;
            _currentShot.IncreaseInfectionRadius(chargeAmount);
        }
        else
        {
            GameOver();
        }
    }
    
    private void ReleaseShot()
    {
        if (_currentShot == null) return;

        _currentShot.Shoot();
        _isCharging = false;
        _currentShot = null;
    }

    private bool IsPathClear()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, _rayLength, obstacleLayer))
        {
            return false;
        }
        
        Vector3 leftRayOrigin = transform.position + transform.right * (transform.localScale.x / 2);
        Vector3 rightRayOrigin = transform.position - transform.right * (transform.localScale.x / 2);

        if (Physics.Raycast(leftRayOrigin, transform.forward, out hit, _rayLength, obstacleLayer) ||
            Physics.Raycast(rightRayOrigin, transform.forward, out hit, _rayLength, obstacleLayer))
        {
            
            return false;
        }

        return true;
    }
    
    private void GameOver()
    {
        _isCharging = false;
        onGameOver?.Invoke();
        Debug.Log("Game Over: Not enough size to charge the shot.");
    }
}