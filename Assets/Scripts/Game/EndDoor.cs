using UnityEngine;
using UnityEngine.Events;

public class EndDoor : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onWin;
    
    [Header("Player Transform")]
    [SerializeField] private Transform playerBall;

    [Header("Settings")]
    [SerializeField] private float yDestination = 3.0f;
    [SerializeField] private float openDistance = 5.0f;

    private bool _isOpen = false;

    private void Update()
    {
        if (!_isOpen && Vector3.Distance(playerBall.position, transform.position) < openDistance)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpen = true;
        transform.Translate(Vector3.up * yDestination);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerBall ball)) return;
        
        onWin?.Invoke();
    }
}