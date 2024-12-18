using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    [SerializeField] private Transform playerBall;
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
        transform.Translate(Vector3.up * 3.0f);
    }
}
