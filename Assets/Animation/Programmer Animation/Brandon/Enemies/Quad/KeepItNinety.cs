using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepItNinety : MonoBehaviour
{
    Vector3 eulerAngles = new Vector3(0f, -90f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, -90f);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.localEulerAngles = eulerAngles;
    }
}
