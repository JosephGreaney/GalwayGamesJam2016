using UnityEngine;

public class PermaRotate : MonoBehaviour
{
    void Update()
    {
        // Rotate the object around its local Y axis at 1 degree per second
        transform.Rotate(Vector3.up * Time.deltaTime*150);

        // ...also rotate around the World's Y axis
      //  transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
    }
}