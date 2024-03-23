using UnityEngine;

public class BlockData : MonoBehaviour
{
    // Datas
    [SerializeField]
    private float scale;
    public float Scale => scale;

    // Components
    private MeshRenderer meshRenderer;

    // Enums
    public enum Name
    {
        Grass,
        Stone,
        Sand,
        Floor
    }

    public void Initialize(Texture2D texture)
    {
        // Get components
        meshRenderer = GetComponent<MeshRenderer>();

        transform.eulerAngles = new Vector3(0f, Random.Range(0, 4) * 90f, 0f);

        meshRenderer.material.SetTexture("_BaseMap", texture);
    }
}
