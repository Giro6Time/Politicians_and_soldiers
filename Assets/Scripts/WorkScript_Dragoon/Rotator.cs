using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 50f; // ������ת�ٶ�

    
    void Update()
    {
        
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
