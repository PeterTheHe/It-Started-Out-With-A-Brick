using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

    private Rigidbody2D pointerRB;
    private bool dragging;
    private RigidbodyConstraints2D oldConstraints;
    private Rigidbody2D currentRB;

	// Use this for initialization
	void Start () {

        GameObject pointerGO = new GameObject("PointerRB");
        pointerRB = pointerGO.AddComponent<Rigidbody2D>();
        pointerRB.isKinematic = true;

    }

    // Update is called once per frame
    void Update () {

        //Set the pointer pos to mouse pos
        pointerRB.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButton(0))
        {

            if (!dragging)
            {

                RaycastHit2D hit = Physics2D.Raycast(pointerRB.transform.position, Vector2.zero);
                if (hit.collider)
                {

                    //Can we drag it?
                    if (hit.collider.GetComponent<Draggable>())
                    {

                        currentRB = hit.collider.GetComponent<Rigidbody2D>();
                        oldConstraints = currentRB.constraints;
                        currentRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        dragging = true;

                    }

                }

            }

            else

            {

                currentRB.transform.position = new Vector3 (pointerRB.transform.position.x, pointerRB.transform.position.y, currentRB.transform.position.z);
            
            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            if (dragging)
            {

                currentRB.constraints = oldConstraints;
                currentRB = null;
                dragging = false;

            }

        }



	}



}
