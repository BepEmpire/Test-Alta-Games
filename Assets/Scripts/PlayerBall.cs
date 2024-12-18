using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBall : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float initialSize = 1.0f;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private float shotScaleFactor = 0.2f;
    [SerializeField] private float shotForce = 500f;
    
    [SerializeField] private float minSize = 0.3f;
    [SerializeField] private float moveSpeed = 2.0f;
    
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float raycastLength = 5.0f;

    private float _currentSize;
    private GameObject _currentShot;
    private bool _isCharging = false;

    private void Start()
    {
        _currentSize = initialSize;
        transform.localScale = Vector3.one * _currentSize;
    }

    private void Update()
    {
        if (IsPathClear())
        {
            EnableMovement();
        }

        if (_isCharging && _currentShot != null)
        {
            ChargeShot();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_currentSize > minSize)
        {
            _isCharging = true;
            StartChargingShot();
        }
        else
        {
            Debug.Log("Game Over: Not enough size to charge the shot.");
            HandleGameOver();
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

    private void StartChargingShot()
    {
        _currentShot = Instantiate(shotPrefab, transform.position + transform.forward * 2.5f, Quaternion.identity);
        _currentShot.transform.localScale = Vector3.zero;
    }
    
    private void ChargeShot()
    {
        if (_currentSize > minSize)
        {
            float chargeAmount = Time.deltaTime * shotScaleFactor;
            _currentSize -= chargeAmount;
            transform.localScale = Vector3.one * _currentSize;

            _currentShot.transform.localScale += Vector3.one * chargeAmount;
            
            ShotBall shotBallScript = _currentShot.GetComponent<ShotBall>();
            if (shotBallScript != null)
            {
                shotBallScript.IncreaseInfectionRadius();
            }
        }
        else
        {
            Debug.Log("Ball is too small! Game over.");
            _isCharging = false;
        }
    }
    
    /*private void ChargeShot()
    {
        _chargeTime += Time.deltaTime;

        float shotSize = Mathf.Clamp(_chargeTime / 2.0f, 0.0f, shotScaleFactor);

        if (_currentSize - shotSize <= minSize)
        {
            Debug.Log("Game Over: Ball size depleted while charging.");
            HandleGameOver();
        }
        else
        {
            _currentSize -= shotSize * Time.deltaTime;
            transform.localScale = Vector3.one * _currentSize;

            if (_currentShot != null)
            {
                _currentShot.transform.localScale = Vector3.one * shotSize;
            }
        }
    }*/
    
    private void ReleaseShot()
    {
        if (_currentShot == null) return;

        Rigidbody rb = _currentShot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * shotForce);
            Debug.Log("Shot fired!");
        }

        _isCharging = false;
        _currentShot = null;
    }
    
    /*private void ReleaseShot()
    {
        if (_currentSize > minSize)
        {
            float shotSize = Mathf.Min(_currentSize - minSize, shotScaleFactor);
            _currentSize -= shotSize;
            transform.localScale = Vector3.one * _currentSize;

            Debug.Log("Creating shot with size: " + shotSize);

            GameObject shot = Instantiate(shotPrefab, transform.position + transform.forward * 1.0f, Quaternion.identity);
            shot.transform.localScale = Vector3.one * shotSize;

            Rigidbody rb = shot.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * shotForce);
                Debug.Log("Shot fired!");
            }

            // Adjust infection radius based on shot size
            ShotBall shotBall = shot.GetComponent<ShotBall>();
            if (shotBall != null)
            {
                // Call IncreaseInfectionRadius to enhance explosion range
                shotBall.IncreaseInfectionRadius();
            }
        }
        else
        {
            Debug.Log("Not enough size to fire a shot.");
        }
    }*/

    private bool IsPathClear()
    {
        float rayLength = transform.localScale.z * raycastLength;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, obstacleLayer))
        {
            return false;
        }
        
        Vector3 leftRayOrigin = transform.position + transform.right * (transform.localScale.x / 2);
        Vector3 rightRayOrigin = transform.position - transform.right * (transform.localScale.x / 2);

        if (Physics.Raycast(leftRayOrigin, transform.forward, out hit, rayLength, obstacleLayer) ||
            Physics.Raycast(rightRayOrigin, transform.forward, out hit, rayLength, obstacleLayer))
        {
            return false;
        }

        return true;
    }
    
    private void HandleGameOver()
    {
        _isCharging = false;
        Debug.LogError("Game Over: Implement game over logic here.");
    }
    
    public void EnableMovement()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
