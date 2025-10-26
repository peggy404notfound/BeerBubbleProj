using UnityEngine;
public class AccelTest : MonoBehaviour
{
    void Update()
    {
        if (Time.frameCount % 10 == 0)
            Debug.Log("acc=" + Input.acceleration);
    }
}