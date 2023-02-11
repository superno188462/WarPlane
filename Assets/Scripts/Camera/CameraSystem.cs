using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : Singleton<CameraSystem>
{
    //速度    

    private float[] sizes = { 7,10,13};
    private int index = 1;

    private float maximum = 13;

    private float minmum = 7;


    
    private Vector3 target;
    private void Start()
    {
        //限制size大小
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minmum, maximum);
        Camera.main.orthographicSize = sizes[index];
        target = new Vector3(0,0,-10);
    }
    public void ChangeCameraSize(bool x)
    {
        if(x==true && index<2)
        {
            index++;
        }
        else if(x==false &&index > 0)
        {
            index--;
        }
        //Debug.Log(index);
        Camera.main.orthographicSize = sizes[index];
    }
    public void FollowTarget(Transform player)
    {
        //Debug.Log("FollowTarget");
        target.x = player.position.x;
        target.y = player.position.y;
        //offset = target.position - this.transform.position;
        Camera.main.transform.position = target;
    }
    //void Update()

    //{

    //    if (Input.GetAxis("Mouse ScrollWheel") != 0)

    //    {

    //            

    //        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minmum, maximum);

    //        //滚轮改变    

    //        Camera.main.orthographicSize =

    //        Camera.main.orthographicSize - Input.GetAxis

    //        ("Mouse ScrollWheel") * ChangeSpeed;

    //    }

    //}

}    

