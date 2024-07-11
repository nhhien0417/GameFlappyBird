using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    public float SpawnRate = 1f;
    public float MinHeight = -1f;
    public float MaxHeight = 1f;

    public Pipes Prefab;

    private IObjectPool<Pipes> _pool;

    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 5;
    [SerializeField] private int _maxSize = 10;

    private void Awake()
    {
        _pool = new ObjectPool<Pipes>(CreatePipes, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                                            _collectionCheck, _defaultCapacity, _maxSize);
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), SpawnRate, SpawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        var pipe = _pool.Get();
        pipe.transform.position += Vector3.up * Random.Range(MinHeight, MaxHeight);
    }

    private Pipes CreatePipes()
    {
        Pipes pipes = Instantiate(Prefab);
        pipes.ObjectPool = _pool;
        return pipes;
    }

    private void OnReleaseToPool(Pipes pipe)
    {
        pipe.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Pipes pipe)
    {
        pipe.gameObject.SetActive(true);
        pipe.transform.position = new Vector3(12, 0, 0);
    }

    private void OnDestroyPooledObject(Pipes pipe)
    {
        Destroy(pipe.gameObject);
    }
}

