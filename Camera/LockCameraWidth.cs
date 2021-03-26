using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace BookIt
{
    public class LockCameraWidth : MonoBehaviour
    {
        #region Before ProCamera2D
        //float defaultWidth;
        //float defaultHeight;

        //Vector3 CameraPos;

        //void Start()
        //{
        //    CameraPos = Camera.main.transform.position;

        //    defaultHeight = Camera.main.orthographicSize;

        //    // Camera size is set as height which is then used to calculate width based on aspect ratio
        //    // Since we always want to display the same width regardless of aspect ratio
        //    // We have to get the width of our reference ratio at the current size we want
        //    // and calculate a new height (Camera size) for the other ratios
        //    defaultWidth = Camera.main.orthographicSize * (16f / 9f);
        //    ScaleCamera();
        //}

        //void ScaleCamera()
        //{
        //    // Since we got our width based on our reference aspect ratio 
        //    // we simply divide it by our current aspect ratio and set our new camera size
        //    Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;

        //    // This sets the camera position to old position but with the bottom of the camera on y's 0 line
        //    Camera.main.transform.position = new Vector3(CameraPos.x, CameraPos.y + -1 * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        //}
        #endregion

        #region With ProCamera2D

        float defaultWidth;
        //float defaultHeight;

        private void Awake()
        {
#if UNITY_IOS
            Application.targetFrameRate = 60;
#endif

            SetInitialReferences();
            ScaleCamera();
        }

        void SetInitialReferences()
        {
            //defaultHeight = Camera.main.orthographicSize;
            defaultWidth = Camera.main.orthographicSize * (16f / 9f);
        }

        void ScaleCamera()
        {
            if (ProCamera2D.Instance != null)
            {
                ProCamera2D.Instance.UpdateScreenSize(defaultWidth / Camera.main.aspect, 0);
                if (ProCamera2D.Instance.GetComponent<ProCamera2DParallax>() != null)
                {
                    ProCamera2D.Instance.GetComponent<ProCamera2DParallax>().RootPosition = new Vector3(
                                ProCamera2D.Instance.GetComponent<ProCamera2DParallax>().RootPosition.x,
                                Camera.main.orthographicSize,
                                ProCamera2D.Instance.GetComponent<ProCamera2DParallax>().RootPosition.z);
                }    
            }
            else Debug.Log("No ProCamera2D instance");
        }
        #endregion
    }
}
