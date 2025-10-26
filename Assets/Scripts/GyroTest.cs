using UnityEngine;

public class GyroTest : MonoBehaviour
{
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        Vector3 g = Input.acceleration;
        Debug.Log($"gyro: {g}");
    }
}