
using System;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public GameObject dirtBlock;
    public Camera fpsCam;
    public LayerMask whatIsGround;
    public GameObject blackSquare;
    public Material hoverOverMat;
    public Material dirt;
    

    private GameObject tempBlock;
    private bool check = false;
    private GameObject currentBlackSquare;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        RaycastHit hitInfo;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, 5,  whatIsGround))
        {

            Vector3 place = hitInfo.point - transform.forward * .01f;
            float placex = Mathf.Round(place.x);
            float placey = Mathf.Round(place.y);
            float placez = Mathf.Round(place.z);
            Vector3 changedPlace = new Vector3(placex, placey, placez);

            if (hitInfo.transform.tag == "Block" && check == false)
            {
                tempBlock = hitInfo.transform.gameObject;
                check = true;
            }

            // Right clicking places a block and left clicking destroys a block
            if (rightClick)
            {
                if(hitInfo.transform.tag == "Block")
                {
                    Instantiate(dirtBlock, hitInfo.transform.position + hitInfo.normal, Quaternion.identity);
                }
                else
                {
                    Instantiate(dirtBlock, changedPlace, Quaternion.identity);
                }
            }
            else if(leftClick)
            {
                if(hitInfo.transform.tag=="Block")
                {
                    Destroy(hitInfo.transform.gameObject);
                    check = false;
                }
            }

            // Black square that shows where the block will be placed on the plane
            changedPlace.y -= 0.49f;
            if (hitInfo.transform.tag == "Plane" && currentBlackSquare == null)
            {
                currentBlackSquare = Instantiate(blackSquare, changedPlace, Quaternion.identity);
            }
            else if(hitInfo.transform.tag == "Plane" && currentBlackSquare != null)
            {
                currentBlackSquare.transform.position = changedPlace;
            }
            else if(currentBlackSquare!=null)
            {
                Destroy(currentBlackSquare);
            }

            // Changes the material of the dirt to show the player hovering over the block
            if (tempBlock != null && GameObject.ReferenceEquals(tempBlock, hitInfo.transform.gameObject))
            {     
                tempBlock.GetComponent<MeshRenderer>().material = hoverOverMat;
            }
            else if(tempBlock != null)
            {
                tempBlock.GetComponent<MeshRenderer>().material = dirt;
                check = false;
            }
        }
        else if(currentBlackSquare!=null)
        {
            Destroy(currentBlackSquare);
        }
    }
}
