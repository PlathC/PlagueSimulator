using UnityEditor;
using UnityEngine;

namespace Model.Utils
{
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera = null;
        private float mainSpeed = 100.0f;
        
        public void Start()
        {
            m_camera = Camera.main;
        }

        public void Update()
        {
            Vector3 pVelocity = new Vector3();
            if (Input.GetKey (KeyCode.Space))
            {
                pVelocity += new Vector3(0, 0 , 1);
            }
            
            if (Input.GetKey (KeyCode.LeftControl))
            {
                pVelocity += new Vector3(0, 0, -1);
            }
            
            if (Input.GetKey (KeyCode.Z))
            {
                pVelocity += new Vector3(0, 1, 0);
            }
            
            if (Input.GetKey (KeyCode.S))
            {
                pVelocity += new Vector3(0, -1, 0);
            }
            
            if (Input.GetKey (KeyCode.Q))
            {
                pVelocity += new Vector3(-1, 0, 0);
            }
            
            if (Input.GetKey (KeyCode.D))
            {
                pVelocity += new Vector3(1, 0, 0);
            }

            m_camera.transform.Translate(pVelocity);
        }
    }
}