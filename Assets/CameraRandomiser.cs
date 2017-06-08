using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRandomiser : MonoBehaviour {

    public Transform target;

    Camera camera2;

    private bool takeHiResShot = false;

    Vector3 startPos;
    Quaternion startRotation;

    // Use this for initialization
    void Start () {

        startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        startRotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
	}

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    // Update is called once per frame
    void Update()
    {

        //move the camera and rotate it a bit

        camera2 = this.GetComponent<Camera>();

        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            //move the camera
            float offsetX = 0;
            float offsetY = 0;
            float offsetZ = 0;

            offsetX = Random.Range(-4, 4);
            offsetY = Random.Range(0, 8);
            offsetZ = Random.Range(-4, 4);

            float rotOffsetX = 0;
            float rotOffsetY = 0;
            float rotOffsetZ = 0;
            float rotOffsetW = 0;

            rotOffsetX = Random.Range(-45, 45);
            rotOffsetY = Random.Range(-45, 45);
            rotOffsetZ = Random.Range(-360, 360);

            this.transform.position = new Vector3(startPos.x + offsetX, startPos.y + offsetY, startPos.z + offsetZ);
            this.transform.rotation = new Quaternion(startRotation.x + rotOffsetX, startRotation.y + rotOffsetY, startRotation.z + rotOffsetZ, startRotation.w + rotOffsetW);

            transform.LookAt(target);

            GameObject card = GameObject.FindGameObjectWithTag("ID_Card");
            GameObject table = GameObject.FindGameObjectWithTag("Table");

            Renderer cardRenderer = card.GetComponent<Renderer>();
            cardRenderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            Renderer tableRenderer = table.GetComponent<Renderer>();
            tableRenderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            takeHiResShot = false;
        }
    }

    void drawCross(Vector3 point, Color color, float dist)
    {
        //Gizmos.color = color;
        //Gizmos.DrawSphere(point, 1);

        //Debug.DrawLine(new Vector3(point.x + dist, point.y + dist, point.z + dist), new Vector3(point.x - dist, point.y - dist, point.z - dist), color);
        //Debug.DrawLine(new Vector3(point.x - dist, point.y - dist, point.z - dist), new Vector3(point.x + dist, point.y + dist, point.z + dist), color);
        //Debug.DrawLine(new Vector3(point.x - dist, point.y - dist, point.z - dist), new Vector3(point.x - dist, point.y - dist, point.z - dist), color);
        //Debug.DrawLine(new Vector3(point.x + dist, point.y + dist, point.z + dist), new Vector3(point.x + dist, point.y + dist, point.z + dist), color);

        Debug.DrawRay(point, Vector3.up, color);
        Debug.DrawRay(point, Vector3.down, color);
        Debug.DrawRay(point, Vector3.left, color);
        Debug.DrawRay(point, Vector3.right, color);
        Debug.DrawRay(point, Vector3.back, color);
        Debug.DrawRay(point, Vector3.forward, color);
    }

    void OnDrawGizmos()
    {
        GameObject card = GameObject.FindGameObjectWithTag("ID_Card");

        Mesh mesh = card.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] verts = mesh.vertices;

        float i = 0;

        foreach (Vector3 vert in verts)
        {
            //2 - top right
            //3 - top left
            //4 - bottom right
            //5 - bottom left
            if (i >=2 && i <=5)
            {
                float scale = i / (float)verts.Length;

                Vector3 pointPos = new Vector3(card.transform.position.x + vert.x, card.transform.position.y + vert.y, card.transform.position.z + vert.z);
                pointPos = Vector3.Scale(card.transform.localScale, pointPos);
                pointPos.y = pointPos.y - (card.transform.localScale.y / 2);

                drawCross(pointPos, new Color(scale, scale, scale), 0.1f);

                Debug.Log(i + ":" + scale);
            }

            i = i + 1;
        }
    }
}
