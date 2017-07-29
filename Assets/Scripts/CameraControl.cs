using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject m_Target;
    private Vector3 m_offset;

    // Use this for initialization
    void Start()
    {
        m_offset = m_Target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 np = m_Target.transform.position - m_offset;
        transform.position = Vector3.Lerp(transform.position, np, 0.1F);
    }
}
