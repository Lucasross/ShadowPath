using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    private float lenght, startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPosX + dist, startPosY, transform.position.z);

        if (temp > startPosX + lenght)
            startPosX += lenght;
        
        else if (temp < startPosX - lenght) 
            startPosX -= lenght;
    }
}
