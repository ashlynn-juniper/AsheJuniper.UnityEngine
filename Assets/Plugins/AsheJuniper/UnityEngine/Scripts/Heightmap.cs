using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsheJuniper.UnityEngine
{
    public class Heightmap : MonoBehaviour
    {
        public int resolution = 16;
        public float frequency = 0.05f;

        public RenderTexture heightmap;

        public ComputeShader computeShader;

        public bool rebuild = true;

        public bool IsReady => !rebuild;

        public void Build()
        {
            heightmap = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);

            heightmap.enableRandomWrite = true;

            computeShader.SetFloat("Resolution", resolution);
            computeShader.SetFloat("Frequency", frequency);

            computeShader.SetTexture(0, "Result", heightmap);

            var noise2DKernel = computeShader.FindKernel("Noise2D");

            computeShader.Dispatch(noise2DKernel, 16, 16, 1);

            rebuild = false;
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
            if (rebuild)
            {
                Build();
            }
        }
    }
}
