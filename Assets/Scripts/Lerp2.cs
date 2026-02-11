// UMD IMDM290 
// Instructor: Myungin Lee
// Student: Andrew Hu


using Unity.VisualScripting;
using UnityEngine;

public class Lerp2 : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float time = 0.01f;
    Vector3[] startPosition, endPosition;

    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            r = 3f; // radius of the heart
            float t = i * 2 * Mathf.PI / numSphere;
            float cos = Mathf.Cos(t);
            float sin = Mathf.Sin(t);
            endPosition[i] = new Vector3(r * Mathf.Sqrt(2) * Mathf.Pow(sin, 3), r * (-Mathf.Pow(cos, 3) - Mathf.Pow(cos, 2) + 2 * cos + 1)  );
        }

        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
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
            Transform sphere = spheres[i].transform;
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();

            // Position
            float lerpFraction = 1 - Mathf.Clamp(Mathf.Abs(Mathf.Cos(time) / (time + 0.5f)), 0, 1);

            sphere.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);

            // Color
            float hue = (float)i / numSphere;
            Color color = Color.HSVToRGB(sphere.position.x/10, sphere.position.y, sphere.position.z); 
            sphereRenderer.material.color = color;
        }
    }
}
