using UnityEngine;
using UnityEngine.Pool;
public class Pipes : MonoBehaviour
{
    public float Speed = 3f;

    public IObjectPool<Pipes> _managePool;
    public IObjectPool<Pipes> ObjectPool { set => _managePool = value; }

    private float _leftEdge;

    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        transform.position += Vector3.left * Speed * Time.deltaTime;

        if (transform.position.x < _leftEdge)
        {
            _managePool.Release(this);
        }
    }

    private void FixedUpdate()
    {
        if (!Bird.isAlive)
        {
            Speed = 0f;
        }
        else
        {
            Speed = 3f;
        }
    }
}
