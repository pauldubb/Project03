
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
    public AudioClip placeSound1;
    public AudioClip placeSound2;
    public AudioClip placeSound3;
    public Animator animator;
    public GameObject player;

    private GameObject tempBlock;
    private bool check = false;
    private GameObject currentBlackSquare;

    // Update is called once per frame
    void Update()
    { 
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        RaycastHit hitInfo;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, 5,  whatIsGround))
        {
            Vector3 place = hitInfo.point - transform.forward * .01f;
            Vector3 changedPlace = new Vector3(Mathf.Round(place.x), Mathf.Round(place.y), Mathf.Round(place.z));

            if (hitInfo.transform.tag == "Block" && check == false)
            {
                tempBlock = hitInfo.transform.gameObject;
                check = true;
            }

            // Right clicking places a block left clicking destroys a block
            if (rightClick)
            {
                    if (hitInfo.transform.tag == "Block")
                    {
                        if (!Approximately(hitInfo.transform.position + hitInfo.normal, transform.position))
                        {
                            Instantiate(dirtBlock, hitInfo.transform.position + hitInfo.normal, Quaternion.identity);
                            //plays one of three possible block noises
                            int random = UnityEngine.Random.Range(0, 3);
                            if (random == 0)
                            {
                                AudioHelper.PlayClip2D(placeSound1, 1f);
                            }
                            else if (random == 1)
                            {
                                AudioHelper.PlayClip2D(placeSound2, 1f);
                            }
                            else if (random == 2)
                            {
                                AudioHelper.PlayClip2D(placeSound3, 1f);
                            }
                            animator.Play("Base Layer.HandMove");
                        }
                    }
                    else
                    {
                        if (!Approximately(changedPlace, transform.position))
                        {
                            Instantiate(dirtBlock, changedPlace, Quaternion.identity);
                            //plays one of three possible block noises
                            int random = UnityEngine.Random.Range(0, 3);
                            if (random == 0)
                            {
                                AudioHelper.PlayClip2D(placeSound1, 1f);
                            }
                            else if (random == 1)
                            {
                                AudioHelper.PlayClip2D(placeSound2, 1f);
                            }
                            else if (random == 2)
                            {
                                AudioHelper.PlayClip2D(placeSound3, 1f);
                            }
                            animator.Play("Base Layer.HandMove");
                        }
                    }
            }
            else if(leftClick)
            {
                if(hitInfo.transform.tag=="Block")
                {
                    hitInfo.transform.gameObject.GetComponent<Block>().Die();
                    check = false;

                    //plays one out of three possible block noises
                    int random = UnityEngine.Random.Range(0, 3);
                    if (random == 0)
                    {
                        AudioHelper.PlayClip2D(placeSound1, 1f);
                    }
                    else if (random == 1)
                    {
                        AudioHelper.PlayClip2D(placeSound2, 1f);
                    }
                    else if (random == 2)
                    {
                        AudioHelper.PlayClip2D(placeSound3, 1f);
                    }
                    animator.Play("Base Layer.HandMove");
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

    private bool Approximately(Vector3 me, Vector3 other)
    {
        Vector3 changedMe = new Vector3(Mathf.Round(me.x), Mathf.Round(me.y), Mathf.Round(me.z));
        Vector3 changedOther = new Vector3(Mathf.Round(other.x), Mathf.Round(other.y), Mathf.Round(other.z));
        Vector3 changedOtherLower = new Vector3(Mathf.Round(other.x), Mathf.Round(other.y)-1, Mathf.Round(other.z));

        return changedMe.Equals(changedOther) || changedMe.Equals(changedOtherLower);
    }
}
