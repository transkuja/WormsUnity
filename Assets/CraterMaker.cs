using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterMaker : MonoBehaviour {
    private TerrainData terrainData;
    private float[,] terrainHeights;
    float[,,] savedAlphas;
    [SerializeField] Texture2D craterTexture;
    [SerializeField] Texture2D craterTextureToApply;

    public int xResHeight;
    public int yResHeight;
    public int xResAlpha;
    public int yResAlpha;
    Color[] craterData;
    Vector3 mousePos;
    int layers;

    void Start () {
        terrainData = Terrain.activeTerrain.terrainData;
        xResHeight = terrainData.heightmapWidth;
        yResHeight = terrainData.heightmapHeight;
        xResAlpha = terrainData.alphamapWidth;
        yResAlpha = terrainData.alphamapHeight;
        terrainHeights = terrainData.GetHeights(0, 0, xResHeight, yResHeight);
        craterData = craterTexture.GetPixels();

        layers = terrainData.alphamapLayers;
        savedAlphas = terrainData.GetAlphamaps(0, 0, xResAlpha, yResAlpha);
    }
	
	public void MakeCrater(Vector3 _impactWithTerrain)
    {
        // Handle terrain height
        int x = (int)Mathf.Lerp(0, xResHeight, Mathf.InverseLerp(0, terrainData.size.x, _impactWithTerrain.x));
        int z = (int)Mathf.Lerp(0, yResHeight, Mathf.InverseLerp(0, terrainData.size.z, _impactWithTerrain.z));
        x = Mathf.Clamp(x, craterTexture.width / 2, xResHeight - craterTexture.width / 2);
        z = Mathf.Clamp(z, craterTexture.height / 2, yResHeight - craterTexture.height / 2);
        float[,] craterArea = terrainData.GetHeights(x - craterTexture.width / 2, z - craterTexture.height / 2, craterTexture.width, craterTexture.height);
        for (int i = 0; i < craterTexture.height; i++)
        {
            for (int j = 0; j < craterTexture.width; j++)
            {
                craterArea[i, j] = craterArea[i, j] - craterData[i * craterTexture.width + j].a * 0.01f;
            }
        }
        terrainData.SetHeights(x - craterTexture.width / 2, z - craterTexture.height / 2, craterArea);

        // Apply texture
        int g = (int)Mathf.Lerp(0, xResAlpha, Mathf.InverseLerp(0, terrainData.size.x, _impactWithTerrain.x));
        var b = (int)Mathf.Lerp(0, yResAlpha, Mathf.InverseLerp(0, terrainData.size.z, _impactWithTerrain.z));
        g = Mathf.Clamp(g, craterTextureToApply.width / 2, xResAlpha - craterTextureToApply.width / 2);
        b = Mathf.Clamp(b, craterTextureToApply.height / 2, yResAlpha - craterTextureToApply.height / 2);
        float[,,] area = terrainData.GetAlphamaps(g - craterTextureToApply.width / 2, b - craterTextureToApply.height / 2, craterTextureToApply.width, craterTextureToApply.height);
        for (int xx = 0; xx < craterTextureToApply.height; xx++)
        {
            for (int y = 0; y < craterTextureToApply.width; y++)
            {
                for (int zz = 0; zz < layers; zz++)
                {
                    if (zz == 1)
                        area[xx, y, zz] += craterData[xx * craterTextureToApply.width + y].a;
                    else
                        area[xx, y, zz] -= craterData[xx * craterTextureToApply.width + y].a;
                }
            }
        }
        terrainData.SetAlphamaps(g - craterTextureToApply.width / 2, b - craterTextureToApply.height / 2, area);
    }

    private void OnApplicationQuit()
    {
        terrainData.SetHeights(0, 0, terrainHeights);
        terrainData.SetAlphamaps(0, 0, savedAlphas);
    }
}
