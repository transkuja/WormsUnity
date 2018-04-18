using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterMaker : MonoBehaviour {
    private TerrainData terrainData;
    private float[,] terrainHeights;
    float[,,] savedAlphas;
    [SerializeField] Texture2D[] craterTextures;
    [SerializeField] Texture2D[] craterTexturesToApply;  

    public int xResHeight;
    public int yResHeight;
    public int xResAlpha;
    public int yResAlpha;
    Color[][] cratersData = new Color[3][];
    Vector3 mousePos;
    int layers;

    void Start () {
        terrainData = Terrain.activeTerrain.terrainData;
        xResHeight = terrainData.heightmapWidth;
        yResHeight = terrainData.heightmapHeight;
        xResAlpha = terrainData.alphamapWidth;
        yResAlpha = terrainData.alphamapHeight;
        terrainHeights = terrainData.GetHeights(0, 0, xResHeight, yResHeight);
        for (int i = 0; i < 3; i++)
            cratersData[i] = craterTextures[i].GetPixels();

        layers = terrainData.alphamapLayers;
        savedAlphas = terrainData.GetAlphamaps(0, 0, xResAlpha, yResAlpha);
    }
	
	public void MakeCrater(Vector3 _impactWithTerrain, int _explosionType)
    {
        // Handle terrain height
        int x = (int)Mathf.Lerp(0, xResHeight, Mathf.InverseLerp(0, terrainData.size.x, _impactWithTerrain.x));
        int z = (int)Mathf.Lerp(0, yResHeight, Mathf.InverseLerp(0, terrainData.size.z, _impactWithTerrain.z));
        x = Mathf.Clamp(x, craterTextures[_explosionType].width / 2, xResHeight - craterTextures[_explosionType].width / 2);
        z = Mathf.Clamp(z, craterTextures[_explosionType].height / 2, yResHeight - craterTextures[_explosionType].height / 2);
        float[,] craterArea = terrainData.GetHeights(x - craterTextures[_explosionType].width / 2, z - craterTextures[_explosionType].height / 2, craterTextures[_explosionType].width, craterTextures[_explosionType].height);
        float depth = (_explosionType == 2) ? 5 : _explosionType + 1;
        for (int i = 0; i < craterTextures[_explosionType].height; i++)
        {
            for (int j = 0; j < craterTextures[_explosionType].width; j++)
            {
                craterArea[i, j] = craterArea[i, j] - cratersData[_explosionType][i * craterTextures[_explosionType].width + j].a * 0.01f * 
                    (Mathf.Lerp(1, depth, cratersData[_explosionType][i * craterTextures[_explosionType].width + j].a));
            }
        }
        terrainData.SetHeights(x - craterTextures[_explosionType].width / 2, z - craterTextures[_explosionType].height / 2, craterArea);

        // Apply texture
        int g = (int)Mathf.Lerp(0, xResAlpha, Mathf.InverseLerp(0, terrainData.size.x, _impactWithTerrain.x));
        var b = (int)Mathf.Lerp(0, yResAlpha, Mathf.InverseLerp(0, terrainData.size.z, _impactWithTerrain.z));
        g = Mathf.Clamp(g, craterTexturesToApply[_explosionType].width / 2, xResAlpha - craterTexturesToApply[_explosionType].width / 2);
        b = Mathf.Clamp(b, craterTexturesToApply[_explosionType].height / 2, yResAlpha - craterTexturesToApply[_explosionType].height / 2);
        float[,,] area = terrainData.GetAlphamaps(g - craterTexturesToApply[_explosionType].width / 2, b - craterTexturesToApply[_explosionType].height / 2, craterTexturesToApply[_explosionType].width, craterTexturesToApply[_explosionType].height);
        for (int xx = 0; xx < craterTexturesToApply[_explosionType].height; xx++)
        {
            for (int y = 0; y < craterTexturesToApply[_explosionType].width; y++)
            {
                for (int zz = 0; zz < layers; zz++)
                {
                    if (zz == 1)
                        area[xx, y, zz] += cratersData[_explosionType][xx * craterTexturesToApply[_explosionType].width + y].a;
                    else
                        area[xx, y, zz] -= cratersData[_explosionType][xx * craterTexturesToApply[_explosionType].width + y].a;
                }
            }
        }
        terrainData.SetAlphamaps(g - craterTexturesToApply[_explosionType].width / 2, b - craterTexturesToApply[_explosionType].height / 2, area);
    }

    private void OnApplicationQuit()
    {
        terrainData.SetHeights(0, 0, terrainHeights);
        terrainData.SetAlphamaps(0, 0, savedAlphas);
    }
}
