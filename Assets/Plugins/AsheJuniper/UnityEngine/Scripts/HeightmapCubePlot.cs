using AsheJuniper.Unity.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightmapCubePlot : MonoBehaviour
{
    public int chunkResolution = 16;
    public int regionResolution = 5;

    public bool rebuild = true;

    public HeightmapCubeRenderer chunkPrefab;

    private float timeSinceCreation = 0.0f;
    private float timeUntilBuild = 0.0f;

    public void Build()
    {
        gameObject.DestroyChildren();

        for (int z = 0; z < regionResolution; z++)
        {
            for (int x = 0; x < regionResolution; x++)
            {
                var chunkPosition = new Vector3(x * chunkResolution * 0.5f, 0, z * chunkResolution * 0.5f);

                Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            }
        }

        rebuild = false;
    }

    private void OnEnable()
    {
        rebuild = true;

        timeUntilBuild = Random.Range(0.0f, 15.0f);
    }

    private void Update()
    {
        timeSinceCreation += Time.deltaTime;

        if (rebuild && timeSinceCreation >= timeUntilBuild)
        {
            Build();
        }
    }
}
