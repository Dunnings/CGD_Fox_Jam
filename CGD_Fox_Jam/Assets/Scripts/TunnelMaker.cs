using UnityEngine;
using System.Collections.Generic;
using System;

public class TunnelMaker : MonoBehaviour {

    //public Material m_mat;

    public GameObject m_trailSprite;
    public GameObject m_breachSprite;

    public GameObject m_trailParent;

    public float m_trailScale = 1f;

    public Vector3 m_offset;

    public int trailLength = 50;

    List<GameObject> allTrailSprites = new List<GameObject>();

    List<GameObject> spawnedTrailSprites = new List<GameObject>();

    //// Use this for initialization
    //void ResetMaterial()
    //   {
    //       Texture2D tex = m_mat.mainTexture as Texture2D;
    //       var startingX = 0; //beginning left most point of rect that will show your texture
    //       var startingY = 0; //beginning top most point of rect that will show your texture
    //       var pixelWidth = tex.width; //texture width
    //       var pixelHeight = tex.height; //texture height
    //       Color[] resetColorArray = tex.GetPixels(startingX, startingY, pixelWidth, pixelHeight);
    //       for (int i = 0; i < resetColorArray.Length; i++)
    //       {
    //           resetColorArray[i] = Color.white;
    //       }
    //       tex.SetPixels(startingX, startingY, pixelWidth, pixelHeight, resetColorArray);
    //       tex.Apply();
    //   }

    //   void OnDestroy()
    //   {
    //       ResetMaterial();
    //   }
    
    public Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
        for (int i = 0; i < trailLength; i++)
        {
            GameObject newTrailSprite = Instantiate(m_trailSprite);
            newTrailSprite.SetActive(false);
            newTrailSprite.transform.localScale = Vector3.one * m_trailScale;
            allTrailSprites.Add(newTrailSprite);
        }
    }
    int m_tunnelCount = 0;
	// Update is called once per frame
	void LateUpdate () {
        if (transform.position.y < WorldGenerator.Instance.m_surfacePos + 2f)
        {

            if (spawnedTrailSprites.Count >= trailLength - 5)
            {
                spawnedTrailSprites[0].SetActive(false);
                spawnedTrailSprites.RemoveAt(0);
            }

            float dist = (Vector3.Distance(transform.position, lastPos));

            int noToSpawn = (int)(dist * 10f);
            noToSpawn++;

            if (noToSpawn > 3)
            {
                //Debug.Log(noToSpawn);
            }

            bool hasBreached = false;
            Vector3 breachPos = Vector3.zero;
            for (int i = 0; i < noToSpawn; i++)
            {
                // Scale the distance
                float scaledDist = dist * (float)((float)i / (float)noToSpawn);
                // Calculate the difference vector
                Vector3 difference = transform.position - lastPos;
                // Normalize and scale the difference vector
                difference = difference.normalized * scaledDist;
                float heightCheck = (lastPos + difference).y - 2f;
                if (heightCheck < WorldGenerator.Instance.m_surfacePos)
                {
                    GameObject newTrailSprite = GetNextAvailableTrailSprite();
                    if (newTrailSprite == null)
                    {
                        newTrailSprite = spawnedTrailSprites[0];
                        spawnedTrailSprites.RemoveAt(0);
                    }
                    newTrailSprite.SetActive(true);
                    Color col = WorldGenerator.Instance.m_dirtColor;
                    col *= 0.8f;
                    col.a = 1f;
                    newTrailSprite.GetComponent<SpriteRenderer>().color = col;
                    newTrailSprite.transform.rotation = gameObject.transform.rotation;
                    newTrailSprite.transform.position = lastPos + difference;
                    newTrailSprite.gameObject.transform.SetParent(m_trailParent.transform);
                    if (gameObject.GetComponent<FoxMovement>().GetVel().x > 0)
                    {
                        newTrailSprite.transform.Translate(new Vector3(m_offset.x, m_offset.y, m_offset.z));
                    }
                    else
                    {
                        newTrailSprite.transform.Translate(new Vector3(-m_offset.x, m_offset.y, m_offset.z));
                    }
                    spawnedTrailSprites.Add(newTrailSprite);
                    ReOrderTrailSprites();
                }
                else
                {
                    hasBreached = true;
                    breachPos = lastPos + difference;
                }
            }

            if (hasBreached) 
            {
                //breachPos.y = WorldGenerator.Instance.m_surfacePos;

                //GameObject newTrailSprite = Instantiate(m_breachSprite);
                //newTrailSprite.transform.position = breachPos;
                //spawnedTrailSprites.Add(newTrailSprite);
            }

        }

        lastPos = transform.position;

        
        //RaycastHit hit;
        //if (!Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit))
        //    return;

        //Renderer rend = hit.transform.GetComponent<Renderer>();
        //MeshCollider meshCollider = hit.collider as MeshCollider;
        //if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        //    return;

        //Texture2D tex = rend.material.mainTexture as Texture2D;
        //Vector2 pixelUV = hit.textureCoord;
        //pixelUV.x *= tex.width;
        //pixelUV.y *= tex.height;

        //for (int x = (int)pixelUV.x - (int)(m_diameter/2f); x < (int)pixelUV.x + (int)(m_diameter / 2f); x++)
        //{
        //    for (int y = (int)pixelUV.y - (int)(m_diameter / 2f); y < (int)pixelUV.y + (int)(m_diameter / 2f); y++)
        //    {
        //        if (Vector2.Distance(new Vector2(x, y), new Vector2(pixelUV.x, pixelUV.y)) < m_diameter/2f)
        //        {
        //            if (tex.GetPixel(x, y) != null)
        //            {
        //                tex.SetPixel(x, y, Color.red);
        //            }
        //        }
        //    }
        //}

        //tex.Apply();
    }

    private void ReOrderTrailSprites()
    {
        for (int i = 0; i < allTrailSprites.Count; i++)
        {
            allTrailSprites[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    public GameObject GetNextAvailableTrailSprite() {
        for (int i = 0; i < allTrailSprites.Count; i++)
        {
            if (!allTrailSprites[i].activeSelf) {
                return allTrailSprites[i];
            }
        }
        return null;
    }

}
