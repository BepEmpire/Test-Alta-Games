using UnityEngine;

public class ShotBall : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 10.0f;
    [SerializeField] private float baseInfectionRadius = 1.0f;
    [SerializeField] private float increaseInfectionSize = 0.1f;

    private float _infectionRadius;
    private void Start()
    {
        _infectionRadius = baseInfectionRadius * transform.localScale.x;
        
        Destroy(gameObject, timeToDestroy);
    }

    public void IncreaseInfectionRadius()
    {
        _infectionRadius += increaseInfectionSize;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle")) return;
        
        Explode();
    }

    private void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, baseInfectionRadius);
        foreach (Collider col in hitObjects)
        {
            if (col.CompareTag("Obstacle"))
            {
                Destroy(col.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
