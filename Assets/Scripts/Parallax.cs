using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public float AnimationSpeed = 0.1f;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        _meshRenderer.material.mainTextureOffset += new Vector2(AnimationSpeed * Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        if (!Bird.isAlive)
        {
            AnimationSpeed = 0f;
        }
        else if (gameObject.tag == "Background")
        {
            AnimationSpeed = 0.1f;
        }
        else
        {
            AnimationSpeed = 0.5f;
        }
    }
}
