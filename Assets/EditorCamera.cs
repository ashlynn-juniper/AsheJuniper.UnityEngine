using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class EditorCamera : MonoBehaviour
{
    void Update()
    {
        var view = SceneView.currentDrawingSceneView;
        
        if (view != null)
        {
            transform.position = view.pivot;
        }
    }
}
