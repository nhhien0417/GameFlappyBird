using UnityEngine;

public class Background : MonoBehaviour
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
}
