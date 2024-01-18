using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsheJuniper.Unity.Engine
{
    [RequireComponent(typeof(Heightmap))]
    public class HeightmapCubeRenderer : MonoBehaviour
    {
        public bool rebuild = true;

        public Heightmap heightmap;

        private float timeSinceCreation = 0.0f;
        private float timeUntilBuild = 0.0f;

        public bool IsReady => heightmap != null && heightmap.IsReady;

        public void Build()
        {
            SetHeightmap();

            gameObject.DestroyChildren();

            var heightmapTexture = heightmap.ToTexture2D();

            for (int z = 0; z < heightmap.resolution; z++)
            {
                for (int x = 0; x < heightmap.resolution; x++)
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.parent = transform;

                    var height = heightmapTexture.GetPixel(x, z).r;

                    cube.transform.localPosition = new Vector3(x, height * heightmap.resolution, z);
                }
            }

            gameObject.CombineChildMeshes();

            rebuild = false;
        }

        private void SetHeightmap()
        {
            heightmap = GetComponent<Heightmap>();
        }

        private void OnEnable()
        {
            timeUntilBuild = Random.Range(0.0f, 30.0f);

            SetHeightmap();
        }

        private void Update()
        {
            timeSinceCreation += Time.deltaTime;

            if (rebuild && heightmap.IsReady && timeSinceCreation >= timeUntilBuild)
            {
                Build();
            }
        }

        private void OnDisable()
        {
            gameObject.DestroyChildren();
        }
    }
}
