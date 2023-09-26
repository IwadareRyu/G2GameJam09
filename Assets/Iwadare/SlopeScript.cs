using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeScript : MonoBehaviour
{
    RaycastHit2D _groundHit;
    [SerializeField]
    LayerMask _groundLayer;
    [SerializeField]
    Transform _spriteTrans;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -3), Color.red);
        //Debug.DrawRay(transform.position, new Vector2(3, -3), Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        _groundHit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, _groundLayer);
        Vector3 cross;
        if (_groundHit.collider)
        {
            Vector2 vec = _groundHit.normal;
            cross = Vector3.Cross(new Vector3(0,0,1),vec);
            Debug.Log($"{cross.x} {cross.y} {cross.z}");
            Vector3 angle = _spriteTrans.rotation.eulerAngles;
            angle.z = -cross.y * 90f;
            _spriteTrans.rotation = Quaternion.Euler(angle);
        }

    }
}
