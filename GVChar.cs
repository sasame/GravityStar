using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVChar : MonoBehaviour
{
    public float _radius = 0.5f;
//    public float _height = 2f;
    public float _speed = 10f;
    public Camera _cam;
    float _rotSpeed = 20f;
    float _skinWidth = 0.01f;
    Vector3 _direction;
    Vector3 _velocity = Vector3.zero;
    bool _isGround = false;
    Vector3 _upDir = Vector3.up;

    public Vector3 GravityDirection
    {
        get { return GVUtil.GetGravityDirection(transform.position); }
    }
    public Vector3 Direction
    {
        get { return _direction; }
    }

	// Use this for initialization
	void Start () {
        _direction = transform.forward;
    }

    bool move(ref Vector3 pos,Vector3 dir,float length,bool isSlide)
    {
        Vector3 firstDir = dir;
        RaycastHit hit = default(RaycastHit);
//        Vector3 pos = transform.position;
        Vector3 prevPos = pos;
        int loopCount = isSlide?5:1;
        for(int i=0;i<loopCount;++i)
        {
            if (Physics.SphereCast(pos, _radius - _skinWidth, dir, out hit, length))
            {
                pos = hit.point + (hit.normal * (_radius));
                dir = (dir - hit.normal * Vector3.Dot(dir,hit.normal)).normalized;
                length -= (pos - prevPos).magnitude;
                prevPos = pos;
                if (length<=0f) break;
                if (Vector3.Dot(dir,firstDir) < 0f) break;
            }
            else 
            {
                pos = pos + dir * length;
                return (i>0);
            }
        }
        return true;
    }

    List<Vector3> _traceLine = new List<Vector3>();
    void trace(Vector3 p)
    {
        _traceLine.Add(p);
        if (_traceLine.Count>200) _traceLine.RemoveAt(0);

        for(int i=0;i<_traceLine.Count-1;++i)
        {
            Debug.DrawLine(_traceLine[i],_traceLine[i+1]);
        }
    }

    // Update is called once per frame
    void Update () {
        GVStar star = GVUtil.GetNearStar(transform.position);
        Vector3 dirG = star.GetGravityDirection(transform.position);
        Vector3 dirUp = Vector3.Lerp(_upDir, -dirG, Time.deltaTime * 10f);
        _upDir = dirUp;
        _direction = (_direction - dirG * Vector3.Dot(_direction, dirG)).normalized;
        transform.LookAt(transform.position + _direction, _upDir);

        float jumpVel = Vector3.Dot(dirUp, _velocity);
        jumpVel = Mathf.Clamp(jumpVel - Time.deltaTime * 30f, -10f, 20f);
        if (_isGround && Input.GetButtonDown("Jump"))
        {
            jumpVel = 20f;
        }
        _velocity = dirUp * jumpVel;

        // move
        Vector3 moveDir = Vector3.zero;
        moveDir = _cam.transform.up * Input.GetAxis("Vertical")
            + _cam.transform.right * Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        float inverseMove = 0f;
        if (moveDir.magnitude > 0f)
        {
            moveDir = (moveDir - dirG * Vector3.Dot(moveDir, dirG)).normalized;
            _direction = Vector3.Lerp(_direction,moveDir,Time.deltaTime * 10f);
            Vector3 prevPos = pos;
            float moveLen = Time.deltaTime * _speed;
            move(ref pos,moveDir, Time.deltaTime * _speed,true);
//            inverseMove = -Mathf.Min(0f,Vector3.Dot(dirG,pos - prevPos));
            inverseMove = _isGround?moveLen:0f;
        }

        // move jump
        if (move(ref pos,dirG, (-jumpVel * Time.deltaTime) + inverseMove,false))
        {
            _isGround = true;
        }
        else
        {
            _isGround = false;
        }

        trace(pos);
        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        GVStar star = GVUtil.GetNearStar(transform.position);
        if (star != null)
        {
            Gizmos.DrawLine(transform.position, transform.position+_direction);
            Gizmos.DrawLine(transform.position, transform.position+_upDir);
            Vector3 dirG = star.GetGravityDirection(transform.position);
//            Gizmos.DrawLine(transform.position, transform.position + dirG*5f);
        }
    }
}
