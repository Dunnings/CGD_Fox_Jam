using UnityEngine;
using System.Collections;

public class FoxTunnel : MonoBehaviour {

    public int m_tunnelDiameter = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (!Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        for (int x = (int)pixelUV.x - (int)(m_tunnelDiameter / 2f); x < (int)pixelUV.x + (int)(m_tunnelDiameter / 2f); x++)
        {
            for (int y = (int)pixelUV.y - (int)(m_tunnelDiameter / 2f); y < (int)pixelUV.y + (int)(m_tunnelDiameter / 2f); y++)
            {
                if (Vector2.Distance(new Vector2(x, y), new Vector2(pixelUV.x, pixelUV.y)) < m_tunnelDiameter / 2f)
                {
                    tex.SetPixel(x, y, Color.red);
                }
            }
        }

        tex.Apply();
    }
}
