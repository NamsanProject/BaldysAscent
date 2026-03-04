using UnityEngine;

public class came : MonoBehaviour
{
    public Transform cam;


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam);
    }
}
