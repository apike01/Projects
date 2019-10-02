using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class HouseController : MonoBehaviour {
    
    /* HouseController Class is used to:
     * 1. Determine is a surface has been found
     * 2. Add a House at the indicated point given on the surface found
     * 3. Control the house movements
     * 4. Change materials on the created house
     */

    public GameObject House;
    public GameObject HouseA;
    
    // Cream House Materials
    public Material creamSiding;
    public Material creamShingles;
    public Material creamPaint;
    public Material BrownRock;
    public Material creamBrick;

    // Blue House Materials
    public Material blueTrim;
    public Material blueSiding;
    public Material blueShingles;
    public Material bluePaint;
    public Material basicBrick;
    public Material basicRock;

    // Green House Materials
    public Material whiteTrim;
    public Material sageSiding;
    public Material sageShingles;
    public Material sagePaint;
   
    // Bools for detecting when occurances happen
    private bool surfacesFound = false;
    public static bool addHouse = false;
    public static bool house1Added = false;
    
    // AR object placements
    private Touch touch;
    private TrackableHit hit;
    private TrackableHitFlags raycastFilter;
    private Anchor anchor;
    
    //Empty GameObjects to set transformations
    public GameObject EmptyGameObject;
    public GameObject EmptyGameObject2;
    public float turnSpeed = 50f;
    public Camera Camera;
    
    //Directions and Vectors
    public bool forward = false;
    private Vector3 forwardNormal = new Vector3();
    private Vector3 rightNormal = new Vector3();
    private Vector3 upNormal = new Vector3();
    private Vector3 StayOnY = new Vector3();

    //Set bool values for movements turned off
    public bool isForwardPressed = false;
    public bool isBackwardPressed = false;
    public bool isLeftPressed = false;
    public bool isRightPressed = false;
    public bool isRotateLeftPressed = false;
    public bool isRotateRightPressed = false;
    public bool isUpPressed = false;
    public bool isDownPressed = false;
       

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Set directions based on the camera position
        EmptyGameObject.transform.position = new Vector3(Camera.transform.position.x, 0, Camera.transform.position.z);
        forwardNormal = Camera.transform.forward;
        rightNormal = Camera.transform.right;
        upNormal = Camera.transform.up;

        // IF statement to see what button has been pressed and execute translate
        if (isForwardPressed)
        {
            EmptyGameObject2.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y, HouseA.transform.position.z);
            HouseA.transform.Translate(forwardNormal * Time.deltaTime * -0.25f, Space.World);
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, EmptyGameObject2.transform.position.y, HouseA.transform.position.z);
        }

        else if (isBackwardPressed)
        {
            EmptyGameObject2.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y, HouseA.transform.position.z);
            HouseA.transform.Translate(forwardNormal * Time.deltaTime * 0.25f, Space.World);
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, EmptyGameObject2.transform.position.y, HouseA.transform.position.z);
        }

        else if (isUpPressed)
        {            
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y + .025f, HouseA.transform.position.z);
        }

        else if (isDownPressed)
        {
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y - .025f, HouseA.transform.position.z);
        }

        else if (isLeftPressed)
        {
            EmptyGameObject2.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y, HouseA.transform.position.z);
            HouseA.transform.Translate(rightNormal * Time.deltaTime * -0.25f, Space.World);
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, EmptyGameObject2.transform.position.y, HouseA.transform.position.z);
        }

        else if (isRightPressed)
        {
            EmptyGameObject2.transform.position = new Vector3(HouseA.transform.position.x, HouseA.transform.position.y, HouseA.transform.position.z);
            HouseA.transform.Translate(rightNormal * Time.deltaTime * 0.25f, Space.World);
            HouseA.transform.position = new Vector3(HouseA.transform.position.x, EmptyGameObject2.transform.position.y, HouseA.transform.position.z);
        }

        else if (isRotateLeftPressed)
        {
            GameObject[] RotatedHouses = GameObject.FindGameObjectsWithTag("HouseCenter");
            foreach (GameObject RotatedHouse in RotatedHouses)
            {
                RotatedHouse.transform.Rotate(RotatedHouse.transform.up, -turnSpeed * Time.deltaTime);
            }
        }

        else if (isRotateRightPressed)
        {
            GameObject[] RotatedHouses = GameObject.FindGameObjectsWithTag("HouseCenter");
            foreach (GameObject RotatedHouse in RotatedHouses)
            {
                RotatedHouse.transform.Rotate(RotatedHouse.transform.up, turnSpeed * Time.deltaTime);
            }
        }

        else
        {
            HouseA.transform.Translate(forwardNormal * Time.deltaTime * 0, Space.World);
        }
       
        // Determine if a surface has been found and add house to position indicated
        if (surfacesFound && addHouse)
        {

            touch = Input.GetTouch(0);
            raycastFilter = TrackableHitFlags.PlaneWithinBounds;
            
            if (Input.touchCount < 1 || touch.phase != TouchPhase.Began)
            {
                return;
            }
            
            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                anchor = hit.Trackable.CreateAnchor(hit.Pose);
                HouseA = Instantiate(House, hit.Pose.position, Quaternion.identity, anchor.transform);
                addHouse = false;
                house1Added = true;
            }                        
        }
        else
        {
            List<DetectedPlane> trackedPlanes = new List<DetectedPlane>();
            Session.GetTrackables<DetectedPlane>(trackedPlanes);
            if (trackedPlanes.Count > 0)
            {
                surfacesFound = true;
            }
        }
                                  
    } // End Update()

    // Methods for changing materials on house object
    public void ChangeMaterialCream()
    {
        //Change Walls
        GameObject[] walls = GameObject.FindGameObjectsWithTag("ExtWalls");
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Renderer>().material = creamSiding;
        }

        //Change Trim
        GameObject[] trims = GameObject.FindGameObjectsWithTag("ExtTrim");
        foreach (GameObject trim in trims)
        {
            trim.GetComponent<Renderer>().material = whiteTrim;
        }

        //Change Shingles
        GameObject[] shingles = GameObject.FindGameObjectsWithTag("GarageShingles");
        foreach (GameObject shingle in shingles)
        {
            shingle.GetComponent<Renderer>().material = creamShingles;
        }

        //Change Paint
        GameObject[] paints = GameObject.FindGameObjectsWithTag("GaragePaint");
        foreach (GameObject paint in paints)
        {
            paint.GetComponent<Renderer>().material = creamPaint;
        }

        //Change Brick
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Bricks");
        foreach (GameObject brick in bricks)
        {
            brick.GetComponent<Renderer>().material = creamBrick;
        }

        //Change Rock
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rocks");
        foreach (GameObject rock in rocks)
        {
            rock.GetComponent<Renderer>().material = BrownRock;
        }
    } // End ChangeMaterialCream()


    public void ChangeMaterialBlue()
    {
        //Change Walls
        GameObject[] walls = GameObject.FindGameObjectsWithTag("ExtWalls");
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Renderer>().material = blueSiding;
        }

        //Change Trim
        GameObject[] trims = GameObject.FindGameObjectsWithTag("ExtTrim");
        foreach (GameObject trim in trims)
        {
            trim.GetComponent<Renderer>().material = blueTrim;
        }

        //Change Shingles
        GameObject[] shingles = GameObject.FindGameObjectsWithTag("GarageShingles");
        foreach (GameObject shingle in shingles)
        {
            shingle.GetComponent<Renderer>().material = blueShingles;
        }

        //Change Paint
        GameObject[] paints = GameObject.FindGameObjectsWithTag("GaragePaint");
        foreach (GameObject paint in paints)
        {
            paint.GetComponent<Renderer>().material = bluePaint;
        }

        //Change Brick
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Bricks");
        foreach (GameObject brick in bricks)
        {
            brick.GetComponent<Renderer>().material = basicBrick;
        }

        //Change Rock
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rocks");
        foreach (GameObject rock in rocks)
        {
            rock.GetComponent<Renderer>().material = basicRock;
        }
    } // End ChangeMaterialBlue()


    public void ChangeMaterialSage()
    {
        //Change Walls
        GameObject[] walls = GameObject.FindGameObjectsWithTag("ExtWalls");
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Renderer>().material = sageSiding;
        }

        //Change Trim
        GameObject[] trims = GameObject.FindGameObjectsWithTag("ExtTrim");
        foreach (GameObject trim in trims)
        {
            trim.GetComponent<Renderer>().material = whiteTrim;
        }

        //Change Shingles
        GameObject[] shingles = GameObject.FindGameObjectsWithTag("GarageShingles");
        foreach (GameObject shingle in shingles)
        {
            shingle.GetComponent<Renderer>().material = sageShingles;
        }

        //Change Paint
        GameObject[] paints = GameObject.FindGameObjectsWithTag("GaragePaint");
        foreach (GameObject paint in paints)
        {
            paint.GetComponent<Renderer>().material = sagePaint;
        }

        //Change Brick
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Bricks");
        foreach (GameObject brick in bricks)
        {
            brick.GetComponent<Renderer>().material = basicBrick;
        }

        //Change Rock
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rocks");
        foreach (GameObject rock in rocks)
        {
            rock.GetComponent<Renderer>().material = basicRock;
        }
    } // End ChangeMaterialSage()

    
    // Method to change bool when house button pressed
    public void AddHouse()
    {
        if (house1Added == false)
        {
            addHouse = true;
        }
    }
       
    // Methods for determining when buttons are pushed or not
    public void onPointerUpForward()
    {
        isForwardPressed = false;
    }

    public void onPointerDownForward()
    {
        isForwardPressed = true;
    }

    public void onPointerUpBackward()
    {
        isBackwardPressed = false;
    }

    public void onPointerDownBackward()
    {
        isBackwardPressed = true;
    }

    public void onPointerUpLeft()
    {
        isLeftPressed = false;
    }

    public void onPointerDownLeft()
    {
        isLeftPressed = true;
    }

    public void onPointerUpRight()
    {
        isRightPressed = false;
    }

    public void onPointerDownRight()
    {
        isRightPressed = true;
    }

    public void onPointerUpRotateLeft()
    {
        isRotateLeftPressed = false;
    }

    public void onPointerDownRotateLeft()
    {
        isRotateLeftPressed = true;
    }

    public void onPointerUpRotateRight()
    {
        isRotateRightPressed = false;
    }

    public void onPointerDownRotateRight()
    {
        isRotateRightPressed = true;
    }

    public void onPointerUpUp()
    {
        isUpPressed = false;
    }

    public void onPointerDownUp()
    {
        isUpPressed = true;
    }

    public void onPointerUpDownDir()
    {
        isDownPressed = false;
    }

    public void onPointerDownDownDir()
    {
        isDownPressed = true;
    }
} // End HouseController class
