using UnityEngine;

public class Obstacles : MonoBehaviour
{
	[Header("Materials")]
	[SerializeField] private Material infectionMaterial;
	
	[Header("Settings")]
	[SerializeField] private float destroyDelay = 1.0f;
	
	private Renderer _objectRenderer;

	private void Awake()
	{
		_objectRenderer = GetComponent<Renderer>();
	}

	public void Infect()
	{
		_objectRenderer.material = infectionMaterial;
		
		Destroy(gameObject, destroyDelay);
	}
}