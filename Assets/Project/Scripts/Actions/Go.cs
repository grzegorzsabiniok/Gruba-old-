using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Go : Task {
    /*
    public Vector3[] path = new Vector3[0];
    int pathPosition = 0;
    Vector3 target;
    Search search;
    public Go() { }
    public Go(Vector3[] _path)
    {
        name = "Go";
        //owner.Go(_target);
        path = _path;
        pathPosition = path.Length - 1;
    }
    public Go( Vector3 _target)
    {
        target = _target;
        name = "Go";
    }
    public Go(Search _search)
    {
        name = "Go";
        search = _search;
    }
    public override void Start()
    {
        if(search != null)
        {
            path = search.path;
        }
        if (path.Length == 0) { 
        SetPath(target);
        
    }
        MonoBehaviour.print(path.Length);
        pathPosition = path.Length - 1;
        task.owner.SetAnimation("walk");
        
    }
    public override bool Success()
    {
        if (task.owner.transform.position == target)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool Update()
    {
        if (pathPosition > -1)
        {
            if (CanGo(path[pathPosition]))
            {
                task.owner.transform.LookAt(path[pathPosition]);
                task.owner.transform.localEulerAngles = new Vector3(0, task.owner.transform.localEulerAngles.y, 0);
                task.owner.transform.position = Vector3.MoveTowards(task.owner.transform.position, path[pathPosition], task.owner.speed * Time.deltaTime * 2 * Main.main.unitSpeed);
                if (Vector3.Distance(task.owner.transform.position, path[pathPosition]) < 0.01f)
                {
                    task.owner.transform.position = path[pathPosition];
                    pathPosition--;
                    
                }
            }
            else
            {
                SetPath(path[0]);
                pathPosition = path.Length - 1;
            }
        }
        else
        {
            task.owner.SetAnimation("idle");
            return false;
        }
        return true;
    }
    struct Waypoint
    {
        public Waypoint(float _value, Vector3 _parent)
        {
            value = _value;
            parent = _parent;
            used = false;
        }
        public float value;
        public Vector3 parent;
        public bool used;
    };
    void SetPath(Vector3 _target)
    {
        _target = new Vector3(Mathf.Floor(_target.x), Mathf.Floor(_target.y), Mathf.Floor(_target.z));
        Dictionary<Vector3, Waypoint> waypoint = new Dictionary<Vector3, Waypoint>();
        Vector3 curent;
        curent = new Vector3( Mathf.Floor(task.owner.transform.position.x), Mathf.Floor(task.owner.transform.position.y), Mathf.Floor(task.owner.transform.position.z));
        waypoint.Add(curent, new Waypoint(10000, Vector3.zero));
        for (int i = 0; i < 2000; i++)
        {
            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        
                        if (curent == _target)
                        {
                            Vector3 temp3 = curent;
                            List<Vector3> temp2 = new List<Vector3>();
                            temp2.Add(curent);
                            while (waypoint[temp3].parent != Vector3.zero)
                            {

                                temp2.Add(temp3);
                                temp3 = waypoint[temp3].parent;

                            }
                            path = temp2.ToArray();
                            MonoBehaviour.print(waypoint.Count+"|"+i);
                            return;
                        }
                        Vector3 temp = curent + new Vector3(x - 1, y - 1, z - 1);
                        if (!waypoint.ContainsKey(temp) && CanGo(temp))
                        {

                            waypoint.Add(temp, new Waypoint(Vector3.Distance(temp, _target)+ Vector3.Distance(temp, task.owner.transform.position), curent));
                            //waypoint.Add(temp, new Waypoint(Vector3.Distance(temp, _target) + Vector3.Distance(temp, curent)+waypoint[curent].value, curent));

                        }
                    }
                }
            }
            Waypoint t = waypoint[curent];
            t.used = true;
            waypoint[curent] = t;
            curent = waypoint.Aggregate((l, r) => ((l.Value.value > r.Value.value && !r.Value.used) || l.Value.used) ? r : l).Key;
            
        }


    }
    public static bool CanGo(Vector3 _position)
    {
        if ((Main.main.GetBlock(_position) <2 && Main.main.GetBlock(_position) != -1)
            && (Main.main.GetBlock(_position - new Vector3(0, 1, 0)) != 0 || Main.main.GetBlock(_position - new Vector3(0, 1, 0)) == -2)
            && Main.main.GetBlock(_position - new Vector3(0, 1, 0)) != 1//zakomentuj zeby wlaczyc jezus_mode
            && (Main.main.GetBlock(_position + new Vector3(0, 1, 0)) == 0 || Main.main.GetBlock(_position + new Vector3(0, 1, 0)) == -2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
    public Vector3[] path = new Vector3[0];
    int pathPosition = 0;
    Vector3 target;
    Search search;
    public Go() { }
    public Go(Search _search)
    {
        search = _search;
        name = "Go";
    }
    public Go(Vector3 _target)
    {
        target = _target;
        name = "Go";
    }
    public override Task Clone()
    {
        Go temp = new Go();
        temp.target = target;
        temp.path = path;
        temp.search = search;

        return temp;
    }
    public override void Start()
    {
        if (search != null)
        {
            path = search.path;
        }
        else
        {
            SetPath(target);
        }
        pathPosition = path.Length - 1;
        owner.SetAnimation("walk");
    }

    public override void Update()
    {
        if (pathPosition > -1)
        {
            if (CanGo(path[pathPosition]))
            {
                var targetRotation = Quaternion.LookRotation(path[pathPosition] - owner.transform.position);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Main.main.unitSpeed * Main.main.gameSpeed* 5 * Time.deltaTime);
                owner.transform.localEulerAngles = new Vector3(0, owner.transform.localEulerAngles.y, 0);
                owner.transform.position = Vector3.MoveTowards(owner.transform.position, path[pathPosition], owner.speed * Time.deltaTime * 2 * Main.main.gameSpeed);
                if (Vector3.Distance(owner.transform.position, path[pathPosition]) < 0.01f)
                {
                    owner.transform.position = path[pathPosition];
                    pathPosition--;

                }
            }
            else
            {
                SetPath(path[0]);
                pathPosition = path.Length - 1;
            }
        }
        else
        {
            owner.SetAnimation("idle");
            Success();
            return;
        }
    }
    struct Waypoint
    {
        public Waypoint(float _value, Vector3 _parent)
        {
            value = _value;
            parent = _parent;
            used = false;
        }
        public float value;
        public Vector3 parent;
        public bool used;
    };
    void SetPath(Vector3 _target)
    {
        _target = Main.Normalize(_target);
        Dictionary<Vector3, Waypoint> waypoint = new Dictionary<Vector3, Waypoint>();
        Vector3 curent;
        curent = Main.Normalize(owner.transform.position);
        waypoint.Add(curent, new Waypoint(10000, Vector3.zero));
        for (int i = 0; i < 2000; i++)
        {
            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {

                        if (curent == _target)
                        {
                            Vector3 temp3 = curent;
                            List<Vector3> temp2 = new List<Vector3>();
                            temp2.Add(curent);
                            while (waypoint[temp3].parent != Vector3.zero)
                            {

                                temp2.Add(temp3);
                                temp3 = waypoint[temp3].parent;

                            }
                            path = temp2.ToArray();
                            return;
                        }
                        Vector3 temp = curent + new Vector3(x - 1, y - 1, z - 1);
                        if (!waypoint.ContainsKey(temp) && CanGo(temp))
                        {

                            waypoint.Add(temp, new Waypoint(Vector3.Distance(temp, _target) + Vector3.Distance(temp, owner.transform.position), curent));
                        }
                    }
                }
            }
            Waypoint t = waypoint[curent];
            t.used = true;
            waypoint[curent] = t;
            curent = waypoint.Aggregate((l, r) => ((l.Value.value > r.Value.value && !r.Value.used) || l.Value.used) ? r : l).Key;

        }
        Failure();

    }
    public static bool CanGo(Vector3 _position)
    {
        if ((Main.main.GetBlock(_position) < 2 && Main.main.GetBlock(_position) != -1)
            && (Main.main.GetBlock(_position - new Vector3(0, 1, 0)) != 0 || Main.main.GetBlock(_position - new Vector3(0, 1, 0)) == -2)
            && Main.main.GetBlock(_position - new Vector3(0, 1, 0)) != 1//zakomentuj zeby wlaczyc jezus_mode
            && (Main.main.GetBlock(_position + new Vector3(0, 1, 0)) == 0 || Main.main.GetBlock(_position + new Vector3(0, 1, 0)) == -2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
