using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVCamera : MonoBehaviour {
    public GVChar _target;
    public float _distance = 1f;
    public float _height = 0.2f;
    Camera _cam;
    Vector3 _prevPos;
    Vector3 _upDir = Vector3.up;
    Vector3 _targetPos = Vector3.zero;
    float _folowDistance = 2f;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _prevPos = _cam.transform.position;
        _targetPos = _target.transform.position;
    }

    // Use this for initialization
    void Start()
    {
    }

    List<Vector3> _traceLine = new List<Vector3>();
    void trace(Vector3 p)
    {
        _traceLine.Add(p);
        if (_traceLine.Count>50) _traceLine.RemoveAt(0);

        for(int i=0;i<_traceLine.Count-1;++i)
        {
            Debug.DrawLine(_traceLine[i],_traceLine[i+1]);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float elapsed = Time.deltaTime;
        Vector3 pos = _target.transform.position;
        Vector3 dirTarget = (pos - _targetPos);
        float len = dirTarget.magnitude;
        if (_folowDistance < len)
        {
            _targetPos += dirTarget.normalized * (len - _folowDistance);
        }
//        Vector3 dir = _target.Direction;
        Vector3 dirG = _target.GravityDirection;
        Vector3 dirUp = Vector3.Lerp(_upDir, -dirG,Time.deltaTime * 5f);
        _upDir = dirUp;
        Vector3 dir = _targetPos - _cam.transform.position;
        dir = (dir - (dirUp * Vector3.Dot(dirUp, dir))).normalized;
        _cam.transform.position = _targetPos + (-dir * _distance) + (dirUp * _height);
        _cam.transform.LookAt(_targetPos, dirUp);

        _prevPos = _cam.transform.position;
        trace(_cam.transform.position);
    }
}
