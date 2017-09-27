using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTrees : Selection
{

    public override void DoThings()
    {
        Tree[] trees = GameObject.FindObjectsOfType<Tree>();
        for (int i = 0; i < trees.Length; i++)
        {
            if (trees[i].transform.position.x >= start.x && trees[i].transform.position.x <= end.x &&
                trees[i].transform.position.y >= start.y && trees[i].transform.position.y <= end.y + 1 &&
                trees[i].transform.position.z >= start.z && trees[i].transform.position.z <= end.z)
            {
                trees[i].Interact();
            }
        }
        Destroy(gameObject);
    }
    public override bool CanPlace(Vector3 start, Vector3 end)
    {
        return true;
    }
}
