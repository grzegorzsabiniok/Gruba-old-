using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class POICenter : MonoBehaviour {

    public static POICenter main;
    Dictionary<Vector3, List<POI>> poi = new Dictionary<Vector3, List<POI>>();
	void Start () {
        main = this;
	}
    public POI CheckPOI(Vector3 _position, Unit _owner, Type _type, Dictionary<string,object> _parameters)
    {
        _position = Main.Normalize(_position);
        if (poi.ContainsKey(_position))
        {
            for (int i = 0; i < poi[_position].Count; i++)
            {
                if (poi[_position][i].position == _position && poi[_position][i].Match(_owner, _type, _parameters))
                {
                    POI temp = poi[_position][i];
                    if (temp.oneTime)
                        RemovePOI(temp);
                    return temp;
                }
            }
        }
        return null;
    }
    public POI CheckPOI(Vector3 _position, Unit _owner, Type _type)
    {
        return CheckPOI(_position, _owner, _type, null);
    }
    public void AddPOI(POI _poi, Vector3 _position,POICaler _caler)
    {
        _position = Main.Normalize(_position);
        if (!poi.ContainsKey(_position))
        {
            poi.Add(_position, new List<POI>());
        }
        _poi.position = _position;
        _poi.caler = _caler;
        poi[_position].Add(_poi);
    }
    public void AddPOI(POI _poi, Vector3 _position)
    {
        AddPOI(_poi, _position, null);
    }
    public void RemovePOI(POI _poi,Vector3 _position)
    {


        if (_poi.subpois != null)
        {
            for (int i = 0; i < _poi.subpois.Length; i++)
            {
                poi[_poi.subpois[i].position].Remove(_poi.subpois[i]);
                if (poi[_poi.subpois[i].position].Count == 0)
                {
                    poi.Remove(_poi.subpois[i].position);
                }
            }
        }
        else
        {
            poi[_position].Remove(_poi);
            if (poi[_position].Count == 0)
            {
                poi.Remove(_position);
            }
        }
    }
    public void RemovePOI(POI _poi)
    {
        RemovePOI(_poi, _poi.position);
    }
}
