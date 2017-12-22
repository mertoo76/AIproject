using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public int roadNumber;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Road"))
        {
            RoadPiece rp = collision.collider.GetComponent<RoadPiece>();
            //  print(collision.collider.tag + " " + rp.PieceNumber);

            roadNumber = rp.PieceNumber;
        }
    }
}
