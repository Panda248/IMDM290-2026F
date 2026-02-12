// UMD IMDM290 
// Instructor: Myungin Lee
// Student: Andrew Hu


using UnityEngine;

public class Lerp2 : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 100; 
    float time = 0.01f;
    Vector3[] startPosition, startDisp, midPosition, endPosition;
    float heartR; // radius of heart
    float flowerR, tScale; // t scale for creating flower
    float randR, minDisplacement, maxDisplacement, minOffset, maxOffset; // fields for slowly moving the random positions
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        startPosition = new Vector3[numSphere]; 
        startDisp = new Vector3[numSphere]; // x = max x disp, y = max y disp, z = t offset
        midPosition = new Vector3[numSphere];
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            randR = 10f;
            minDisplacement = 0.01f;
            maxDisplacement = 0.02f;
            minOffset = 0;
            maxOffset = Mathf.PI;
            startPosition[i] = new Vector3(randR * Random.Range(-1f, 1f), randR * Random.Range(-1f, 1f), randR * Random.Range(-1f, 1f));
            startDisp[i] = new Vector3(Random.Range(minDisplacement, maxDisplacement),
                                       Random.Range(minDisplacement, maxDisplacement),
                                       Random.Range(minOffset, maxOffset));
            // creating a flower shape
            flowerR = 5f;
            tScale = 3.5f;
            float t = i * 2 * Mathf.PI / numSphere;
            float r = Mathf.Cos(tScale * t);
            float cos = r * Mathf.Cos(t);
            float sin = r * Mathf.Sin(t);
            midPosition[i] = new Vector3(flowerR * cos, flowerR * sin);

            heartR = 3f; // radius of the heart
            t = i * 2 * Mathf.PI / numSphere;
            cos = Mathf.Cos(t);
            sin = Mathf.Sin(t);
            endPosition[i] = new Vector3(heartR * Mathf.Sqrt(2) * Mathf.Pow(sin, 3), heartR * (-Mathf.Pow(cos, 3) - Mathf.Pow(cos, 2) + 2 * cos + 1));
        }

        // Let there be spheres..
        for (int i = 0; i < numSphere; i++){
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            spheres[i].transform.position = startPosition[i];

            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere;
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            sphereRenderer.material.color = color;
        }
    }

    void Update()
    {
        time += Time.deltaTime;


        for (int i = 0; i < numSphere; i++){
            // move randomPositions
            float t = time + startDisp[i].z;
            float x = Mathf.Sin(t) * startDisp[i].x;
            float y = Mathf.Sin(t) * startDisp[i].y;
            startPosition[i] += new Vector3(x, y);

            // rotate flower 
            t = i * 2 * Mathf.PI / numSphere; // reusing t var (not reusing value)
            float r = Mathf.Cos(5 * t + time);
            float cos = r * Mathf.Cos(t);
            float sin = r * Mathf.Sin(t);
            midPosition[i] = new Vector3(flowerR * cos, flowerR * sin);

            // move heartSpheres (so it looks like its cycling)
            t = time + (i * 2 * Mathf.PI / numSphere);
            cos = Mathf.Cos(t);
            sin = Mathf.Sin(t);
            endPosition[i].Set(heartR * Mathf.Sqrt(2) * Mathf.Pow(sin, 3), heartR * (-Mathf.Pow(cos, 3) - Mathf.Pow(cos, 2) + 2 * cos + 1), 0);

            // Update Spheres
            Transform sphere = spheres[i].transform;
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();

            // Position
            float lerpFraction = (Mathf.Pow(Mathf.Cos(time), 5) + Mathf.Pow(Mathf.Sin(time), 3) + Mathf.Cos(time)) / 2f;
            lerpFraction = Mathf.Clamp(lerpFraction, -1, 1);
            if(lerpFraction > 0)
            {
                sphere.position = Vector3.Lerp(midPosition[i], endPosition[i], lerpFraction);
            }
            else
            {
                sphere.position = Vector3.Lerp(startPosition[i], midPosition[i], lerpFraction + 1);
            }


            // Color
            float hue = (float)i / numSphere;
            Color color = Color.HSVToRGB((Mathf.Abs(sphere.position.x) % 5)/5, (Mathf.Abs(sphere.position.y) % 5)/5, 0.5f); 
            sphereRenderer.material.color = color;
        }
    }
}
