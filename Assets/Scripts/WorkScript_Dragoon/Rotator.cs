using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 50f; // 设置旋转速度

    
    void Update()
    {
        
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
