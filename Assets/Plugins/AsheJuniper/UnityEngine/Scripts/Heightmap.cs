using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsheJuniper.Unity.Engine
{
    public class Heightmap : MonoBehaviour
    {
        public int resolution = 256;
        public float frequency = 0.7f;

        public RenderTexture heightmap;

        public ComputeShader computeShader;

        public bool rebuild = true;

        private bool hasBuiltBefore = false;

        public bool IsReady => !rebuild;

        public void Build()
        {
            heightmap = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);

            heightmap.enableRandomWrite = true;

            computeShader.SetInt("Resolution", resolution);
            computeShader.SetFloat("Frequency", frequency);

            var offset = new Vector2(transform.position.x, transform.position.z);

            computeShader.SetVector("Offset", offset);

            //Debug.Log("[Task] [✔] Compute heightmap for chunk (" + transform.position.x + ", " + transform.position.z + ")");

            computeShader.SetTexture(0, "Result", heightmap);

            var noise2DKernel = computeShader.FindKernel("Noise2D");

            computeShader.Dispatch(noise2DKernel, 16, 16, 1);

            rebuild = false;
            hasBuiltBefore = true;
        }

        public RenderTexture ToTexture()
        {
            return heightmap;
        }

        public Texture2D ToTexture2D()
        {
            return heightmap.ToTexture2D();
        }

        private void OnEnable()
        {
            rebuild = heightmap == null;
        }

        private void Update()
        {
            if (!hasBuiltBefore || rebuild)
            {
                Build();
            }
        }
    }
}
