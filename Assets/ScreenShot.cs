 using UnityEngine;
 using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ScreenShot : MonoBehaviour {
     public int resWidth = 2550; 
     public int resHeight = 3300;
 
     private bool takeHiResShot = false;

    Camera camera2; 

    public static string ScreenShotName(int width, int height) {

        string newPath = Application.dataPath;
        newPath = newPath.Remove(newPath.Length - "\\Assets\\".Length + 1);

         return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                              newPath, 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffff"));
     }
 
     void LateUpdate() {
        camera2 = this.GetComponent<Camera>();

        takeHiResShot |= Input.GetKeyDown("k");
         if (takeHiResShot) {

            string filename = ScreenShotName(resWidth, resHeight);

            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera2.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera2.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            //----- Get corners -----//

            List<Vector3> corners = new List<Vector3>();

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
                if (i >= 2 && i <= 5)
                {
                    float scale = i / (float)verts.Length;

                    Vector3 pointPos = new Vector3(card.transform.position.x + vert.x, card.transform.position.y + vert.y, card.transform.position.z + vert.z);
                    pointPos = Vector3.Scale(card.transform.localScale, pointPos);
                    pointPos.y = pointPos.y - (card.transform.localScale.y / 2);

                    Vector3 screenPoint = camera2.WorldToScreenPoint(pointPos);
                    screenPoint.y = 1080 - screenPoint.y;

                    corners.Add(screenPoint);
                }

                i = i + 1;
            }

            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> top = new List<int>();
            List<int> bottom = new List<int>();

            foreach (Vector3 point in corners)
            {
                if (point.x > (1920 / 2))
                {
                    right.Add(corners.IndexOf(point));
                }
                else
                {
                    left.Add(corners.IndexOf(point));
                }

                if (point.y < (1080 / 2))
                {
                    top.Add(corners.IndexOf(point));
                }
                else
                {
                    bottom.Add(corners.IndexOf(point));
                }
            }

            
            String[] outputList = new String[4];
                
            for(int ii = 0; ii < corners.Count; ii++)
            {
                if (left.Contains(ii) && top.Contains(ii))
                {
                    outputList[0] = corners[ii].x + "," + corners[ii].y;
                }
                else if (right.Contains(ii) && top.Contains(ii))
                {
                    outputList[1] = corners[ii].x + "," + corners[ii].y;
                }
                else if (left.Contains(ii) && bottom.Contains(ii))
                {
                    outputList[2] = corners[ii].x + "," + corners[ii].y;
                }
                else if (right.Contains(ii) && bottom.Contains(ii))
                {
                    outputList[3] = corners[ii].x + "," + corners[ii].y;
                }
            }

            if (!outputList.Contains(null))
            {
                string txtFilename = filename.Replace(".png", ".txt");
                Debug.Log(string.Format("Logged points to: {0}", txtFilename));

                System.IO.File.WriteAllLines(txtFilename, outputList);


                //----- End get corners -----//

                
                byte[] bytes = screenShot.EncodeToPNG();
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
            }

            camera2.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);

            takeHiResShot = false;
         }
     }
 }