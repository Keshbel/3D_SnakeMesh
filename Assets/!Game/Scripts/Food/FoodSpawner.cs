using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food")]
    [SerializeField] private Transform foodParent;
    [SerializeField] private Food foodPrefab;
    [SerializeField] private Vector2 foodCountRange;
    
    [Header("Planet")]
    [SerializeField] private Transform planetTransform;
    private Vector3 _planetSize;
    private Bounds _planetBounds;
    
    private void Awake()
    {
        _planetBounds = planetTransform.GetComponent<MeshCollider>().bounds;
        _planetSize = _planetBounds.size;
        
        var randomCount = Random.Range(foodCountRange.x, foodCountRange.y);
        
        for (int i = 0; i < randomCount; i++) RandomSpawn();
    }

    public void RandomSpawn()
    {
        if (!foodPrefab) return;
        
        var prefab = Instantiate(foodPrefab, foodParent, true);
        prefab.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        var planetPosition = planetTransform.position;
        var min = planetPosition - _planetSize/2;
        var max = planetPosition + _planetSize/2;
        var position = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        position = _planetBounds.ClosestPoint(position);
        
        return position;
    }
}
