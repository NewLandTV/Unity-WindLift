using System.Collections;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // World settings
    [Header("World Settings"), SerializeField]
    private string worldName;
    [SerializeField]
    private Vector2Int worldSize;

    [Space, SerializeField]
    private Texture2D blocksTexture;
    [SerializeField]
    private Vector2Int blocksTextureSize;

    // Rendering settings
    [Space, Header("Rendering Settings"), SerializeField]
    private uint renderingBlocks;

    // Prefabs
    [Space, Header("Prefabs"), SerializeField]
    private BlockData worldBlockDataPrefab;

    // Other components
    [Space, Header("Other Components"), SerializeField]
    private Transform player;

    // World datas
    private BlockData[] worldBlockDatas;
    private Texture2D[] blockTextures;

    private Vector3 previousPlayerPosition;

    private void Awake()
    {
        blockTextures = new Texture2D[blocksTextureSize.x * blocksTextureSize.y];

        for (int i = 0; i < blocksTextureSize.x * blocksTextureSize.y; i++)
        {
            blockTextures[i] = new Texture2D(16, 16);

            blockTextures[i].wrapMode = TextureWrapMode.Repeat;

            blockTextures[i].SetPixels(blocksTexture.GetPixels(i % blocksTextureSize.x * 16, blocksTexture.height - i / blocksTextureSize.x * 16 - 16, 16, 16));
            blockTextures[i].Apply();
        }
    }

    private IEnumerator Start()
    {
        yield return GenerateWorld();

        while (true)
        {
            yield return RenderingBlocks();

            yield return null;
        }
    }

    private IEnumerator GenerateWorld()
    {
        Loading.Instance.IsLoading = true;
        Loading.Instance.ProgressBarFillAmount = 0f;

        player.gameObject.SetActive(false);

        int x = worldSize.x;
        int z = worldSize.y;
        float progressBarFillAmount = 1f / (x * z);

        worldBlockDatas = new BlockData[x * z];

        for (int xPosition = 0; xPosition < x; xPosition++)
        {
            for (int zPosition = 0; zPosition < z; zPosition++)
            {
                worldBlockDatas[xPosition + zPosition * x] = Instantiate(worldBlockDataPrefab, new Vector3(xPosition + 0.5f - x * 0.5f, 0f, zPosition + 0.5f - z * 0.5f) * worldBlockDataPrefab.Scale, Quaternion.identity, transform);

                worldBlockDatas[xPosition + zPosition * x].Initialize(GetTexture(BlockData.Name.Grass));

                Loading.Instance.ProgressBarFillAmount += progressBarFillAmount;
            }

            yield return null;
        }

        previousPlayerPosition = Vector3.up * worldBlockDatas[0].Scale * worldBlockDatas[0].Scale;

        player.gameObject.SetActive(true);

        Loading.Instance.IsLoading = false;
    }

    private IEnumerator RenderingBlocks()
    {
        if ((previousPlayerPosition - player.position).sqrMagnitude < worldBlockDataPrefab.Scale * worldBlockDataPrefab.Scale)
        {
            yield break;
        }

        previousPlayerPosition = player.position;

        int length = worldSize.x * worldSize.y;
        int count = Mathf.RoundToInt(Mathf.PI * renderingBlocks * renderingBlocks);
        float blockDistance = worldBlockDatas[0].Scale * renderingBlocks * worldBlockDataPrefab.Scale * renderingBlocks;
        bool active = false;

        for (int i = 0, c = 0; c < count && i < length; i++, c += active ? 1 : 0)
        {
            active = (player.position - worldBlockDatas[i].transform.position).sqrMagnitude < blockDistance;

            worldBlockDatas[i].gameObject.SetActive(active);
        }
    }

    private Texture2D GetTexture(BlockData.Name name)
    {
        return blockTextures[(int)name];
    }
}
