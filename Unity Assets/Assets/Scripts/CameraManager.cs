using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera selectedCamera;
    public Camera maincam;
    public Camera shopcam;

    void Start()
    {
        maincam.gameObject.SetActive(true);
        shopcam.gameObject.SetActive(false);
    }

    public void ToggleCamera()
    {
        if(maincam.gameObject.activeInHierarchy)
        {
            maincam.gameObject.SetActive(false);
            shopcam.gameObject.SetActive(true);
        }
        else
        {
            maincam.gameObject.SetActive(true);
            shopcam.gameObject.SetActive(false);
        }
    }
}
