using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour {

    public bool show;
    int stage = 0;
    Vector3 position;
    public Color ok, nook;
    public Vector3 start, end;
    public void Create()
    {
        Transform temp = Transform.Instantiate(transform);
        temp.GetComponent<MeshRenderer>().material.color = ok;
    }
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        switch (stage)
        {
            case 0:
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Destroy(gameObject);
                    }
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction, out hit))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            stage = 1;
                            position = transform.position;
                            break;
                        }

                        if (hit.transform.tag == "terrain")
                        {
                            transform.position = Main.Normalize(new Vector3(0,-1,0)+hit.point+(hit.point-ray.origin)/1000)+new Vector3(0,0.5f,0);
                        }
                    }
                }
                break;
            case 1:
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray.origin, ray.direction, out hit))
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            Destroy(gameObject);
                        }
                        if (hit.transform.tag == "terrain")
                        {

                            Vector3 temp = Main.Normalize(new Vector3(0, -1, 0) + hit.point + (hit.point - ray.origin) / 1000) + new Vector3(0, 0.5f, 0);
                            Vector3 min = new Vector3(Mathf.Min(temp.x, position.x), Mathf.Min(temp.y, position.y), Mathf.Min(temp.z, position.z));
                            Vector3 max = new Vector3(Mathf.Max(temp.x, position.x)+1, Mathf.Max(temp.y, position.y)+1, Mathf.Max(temp.z, position.z)+1);
                            transform.localScale = (max - min);
                            transform.localScale += new Vector3(transform.localScale.x==0?1:0, 0.2f, transform.localScale.z == 0 ? 1 : 0);
                            transform.position = (max - min) / 2 + min - new Vector3(0.5f,0.5f,0.5f);
                            
                            if (Input.GetMouseButtonUp(0))
                            {
                                if (!CanPlace(position, temp))
                                {
                                    Destroy(gameObject);
                                    return;
                                }
                                else
                                {
                                    stage = 2;
                                    start = min- new Vector3(0, 0.5f, 0);
                                    end = max-new Vector3(1,0,1);
                                    DoThings();
                                }
                            }
                            
                                if (CanPlace(position, temp))
                            {
                                transform.GetComponent<MeshRenderer>().material.color = ok;
                            }
                            else
                            {
                                transform.GetComponent<MeshRenderer>().material.color = nook;
                            }
                        }
                    }
                }
                break;
                
        }
    }
    public virtual void DoThings()
    {
        
    }
    public virtual bool CanPlace(Vector3 start, Vector3 end)
    {
        if (start.y != end.y)
        {
            return false;
        }
        Vector3 min = new Vector3(Mathf.Min(start.x, end.x), 0, Mathf.Min(start.z, end.z));
        Vector3 max = new Vector3(Mathf.Max(start.x, end.x), 0, Mathf.Max(start.z, end.z));
        for (int x = (int)min.x; x <= (int)max.x; x++)
        {
            for (int y = (int)min.z; y <= (int)max.z; y++)
            {
                if(Main.main.GetBlock(new Vector3(x,start.y+1,y)) != 0 || Main.main.GetBlock(new Vector3(x, start.y, y)) <2)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
