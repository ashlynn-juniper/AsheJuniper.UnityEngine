using System;
using UnityEngine;

namespace AsheJuniper.Unity.Engine
{
    public static class ObjectExtMds
    {
        public static Mesh CombineChildMeshes(this GameObject gameObject)
        {
            var gameObjectMeshFilter = gameObject.GetComponent<MeshFilter>();
            var gameObjectMeshRenderer = gameObject.GetComponent<MeshRenderer>();

            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            Material material = gameObjectMeshRenderer.material;

            int i = 0;

            while (i < meshFilters.Length)
            {
                if (meshFilters[i] == gameObjectMeshFilter)
                {
                    i++;

                    continue;
                }

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

                meshFilters[i].gameObject.SetActive(false);

                var meshRenderer = meshFilters[i].gameObject.GetComponent<MeshRenderer>();

                material = meshRenderer.material;

                i++;
            }

            Mesh mesh = new Mesh();

            mesh.CombineMeshes(combine);

            gameObjectMeshFilter.sharedMesh = mesh;
            gameObjectMeshRenderer.material = material;

            gameObject.transform.gameObject.SetActive(true);

            return mesh;
        }

        public static Mesh CombineChildMeshes<T>(this GameObject gameObject)
            where T : MonoBehaviour
        {
            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                if (meshFilters[i].GetComponent<T>() == null)
                {
                    continue;
                }

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

                meshFilters[i].gameObject.SetActive(false);

                i++;
            }

            Mesh mesh = new Mesh();

            mesh.CombineMeshes(combine);

            gameObject.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            gameObject.transform.gameObject.SetActive(true);

            return mesh;
        }

        public static GameObject CreateChild(this GameObject gameObject)
        {
            var child = new GameObject();

            child.transform.parent = gameObject.transform;

            return child;
        }
        
        public static T CreateChild<T>(this GameObject gameObject)
            where T : MonoBehaviour
        {
            var child = new GameObject().AddComponent<T>();

            child.transform.parent = gameObject.transform;

            return child;
        }

        public static void DestroyChild(this GameObject gameObject)
        {
            UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<Transform>().gameObject);
        }

        public static void DestroyChild<T>(this GameObject gameObject)
            where T : MonoBehaviour
        {
            UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<T>().gameObject);
        }

        public static void DestroyChildren(this GameObject gameObject)
        {
            Array.ForEach(
                gameObject.GetComponentsInChildren<Transform>(),
                (Transform child) =>
                {
                    if (child == gameObject.transform) return;

                    UnityEngine.Object.Destroy(child.gameObject);
                }
            );
        }

        public static void DestroyChildren<T>(this GameObject gameObject)
            where T : MonoBehaviour
        {
            Array.ForEach(
                gameObject.GetComponentsInChildren<T>(),
                (T child) =>
                {
                    if (child == gameObject.GetComponent<T>()) return;

                    UnityEngine.Object.Destroy(child.gameObject);
                }
            );
        }

        public static string GetVirtualPath(this GameObject gameObject)
        {
            return "assets://" + gameObject.GetType().Name + "/" + gameObject.name;
        }
    }
}
