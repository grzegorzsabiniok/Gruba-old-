using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCamera : MonoBehaviour {
    public float cameraSpeed;
    Vector3 mouse;
    float touchpad;
    public float mouseSensivity;
    void Start()
    {
        transform.parent.position = new Vector3(10, Main.main.mapSize.y*Main.main.chunkSize.y + 40, 10);
    }
    void Update () {
        transform.parent.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * cameraSpeed);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, hit.point, Input.GetAxis("Mouse ScrollWheel")*10 - touchpad);
        }
            if (Input.GetMouseButton(1))
        {
            Vector3 temp = mouse - Input.mousePosition;
            transform.Rotate(new Vector3(transform.localEulerAngles.x+temp.y < 60 && transform.localEulerAngles.x + temp.y >5?temp.y*mouseSensivity:0,0,0));
            transform.parent.Rotate(new Vector3(0, -temp.x* mouseSensivity, 0));
        }
        mouse = Input.mousePosition;
        transform.parent.position = new Vector3(
    transform.parent.position.x > Main.main.mapSize.x * Main.main.chunkSize.x ? Main.main.mapSize.x * Main.main.chunkSize.x : transform.parent.position.x < 0 ? 0 : transform.parent.position.x,
    transform.parent.position.y > Main.main.mapSize.y * Main.main.chunkSize.y + 50 ? Main.main.mapSize.y * Main.main.chunkSize.y + 50 : transform.parent.position.y < 0 ? 0 : transform.parent.position.y,
    transform.parent.position.z > Main.main.mapSize.z * Main.main.chunkSize.z ? Main.main.mapSize.z * Main.main.chunkSize.z : transform.parent.position.z < 0 ? 0 : transform.parent.position.z
    );
    }
    public void OnGUI()
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            touchpad = Event.current.delta.y;
        }
        else
        {
            touchpad = 0;
        }
    }
}
