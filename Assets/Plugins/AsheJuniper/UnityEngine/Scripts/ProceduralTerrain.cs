using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsheJuniper.Unity.Engine
{
    public class ProceduralTerrain : MonoBehaviour
    {
        public int resolution = 16;
        public float amplitude = 1.0f;
        public float frequency = 0.05f;

        public Heightmap heightmap;

        public bool rebuild = true;

        private Heightmap heightmapPrevious;

        public void Build()
        {
            heightmap.resolution = resolution;
            heightmap.frequency = frequency;
            heightmap.transform.position = transform.position;

            heightmap.Build();

            var heightmapTexture = heightmap.ToTexture2D();

            var terrain = GetComponent<Terrain>();

            var data = terrain.terrainData;
            var hw = data.heightmapResolution;
            var hh = data.heightmapResolution;

            var heights = data.GetHeights(0, 0, hw, hh);

            for (var y = 0; y < hh; y++)
            {
                for (var x = 0; x < hw; x++)
                {
                    // Normalize the coordinates.
                    var x1 = 1.0f / hw * x * heightmapTexture.width;
                    var y1 = 1.0f / hh * y * heightmapTexture.height;

                    // Get the color height.
                    var pixel = heightmapTexture.GetPixel((int)x1, (int)y1);
                    //var f = pixel.grayscale; // defines height
                    //var g = f * f * f; // some smoothing
                    //var s = 0.025f; // some scaling

                    //heights[x, y] = g * s;

                    heights[x, y] = pixel.r;
                }
            }

            data.SetHeights(0, 0, heights);

            rebuild = false;

            Debug.Log("Build complete!");
        }

        private void Update()
        {
            if (heightmap != heightmapPrevious)
            {
                rebuild = true;
            }

            if (rebuild && heightmap.IsReady)
            {
                Build();
            }

            heightmapPrevious = heightmap;
        }
    }
}
