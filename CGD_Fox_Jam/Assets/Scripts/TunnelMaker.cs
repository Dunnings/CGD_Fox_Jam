using UnityEngine;
using System.Collections.Generic;

public class TunnelMaker : MonoBehaviour {

    //public Material m_mat;

    public GameObject m_trailSprite;
    public GameObject m_breachSprite;

    public GameObject m_trailParent;

    public float m_trailScale = 1f;

    public Vector3 m_offset;
    

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
    }

	// Update is called once per frame
	void LateUpdate () {
        if (transform.position.y < WorldGenerator.Instance.m_surfacePos + 2f)
        {
            float dist = (Vector3.Distance(transform.position, lastPos));

            int noToSpawn = (int)(dist * 10f);
            noToSpawn++;

            if (noToSpawn > 3)
            {
                Debug.Log(noToSpawn);
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
                if((lastPos + difference).y < WorldGenerator.Instance.m_surfacePos)
                {
                    GameObject newTrailSprite = Instantiate(m_trailSprite);
                    Color col = WorldGenerator.Instance.m_dirtColor;
                    col *= 0.8f;
                    col.a = 1f;
                    newTrailSprite.GetComponent<SpriteRenderer>().color = col;
                    newTrailSprite.transform.rotation = gameObject.transform.rotation;
                    newTrailSprite.transform.localScale = Vector3.one * m_trailScale;
                    newTrailSprite.transform.position = lastPos + difference;
                    newTrailSprite.gameObject.transform.SetParent(m_trailParent.transform);
                    newTrailSprite.transform.Translate(m_offset);
                    spawnedTrailSprites.Add(newTrailSprite);
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

        if(spawnedTrailSprites.Count > 500)
        {
            Destroy(spawnedTrailSprites[0].gameObject);
            spawnedTrailSprites.RemoveAt(0);
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
}
