using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;

public class Billbord : MonoBehaviour
{
    public Transform cam;
    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);    
    }
}
