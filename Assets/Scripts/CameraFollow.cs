
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    private float _target_poseX,_target_poseY, _posX, _posY;
    public float rightMax, leftMax, highMax, highMin, speed;
    public bool active = true;
    private float _posZ = -8.77f;
   void Awake()
{
    
    if (target)
        {
            _target_poseX = target.transform.position.x;
            _target_poseY = target.transform.position.y;

            // Aplicar límites iniciales
            if (_target_poseX > rightMax && _target_poseX < leftMax)
            {
                _posX = _target_poseX;
            }
            else
            {
                _posX = Mathf.Clamp(_target_poseX, rightMax, leftMax);
            }
            if (_target_poseY < highMax && _target_poseY > highMin)
            {
                _posY = _target_poseY;
            }
            else
            {
                _posY = Mathf.Clamp(_target_poseY, highMin, highMax);
            }

            // Seteamos directamente la posición de la cámara
            transform.position = new Vector3(_posX, _posY, _posZ);
        }
}
    void Move_Cam()
    {
        if (active)
        {
            if (target)
            {
                _target_poseX = target.transform.position.x;
                _target_poseY = target.transform.position.y;

                if (_target_poseX > rightMax  && _target_poseX < leftMax)
                {
                    _posX = _target_poseX;
                }

                if (_target_poseY < highMax && _target_poseY > highMin)
                {
                    _posY = _target_poseY;
                }
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(_posX, _posY, _posZ), speed*Time.deltaTime);
        }
    }
    void Update()
    {
        Move_Cam();
    }
}
