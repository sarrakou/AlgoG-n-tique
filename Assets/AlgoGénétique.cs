using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

/*
TODO : 
    - Convert float to binary - ELODIE
    - Convery binary to float - ELODIE
    - Recombinaison (crossover) - ADRIANA
    - Mutation - SARRA
    - Evaluer et supprimer les plus faibles - RUTH
*/
public class AlgoGénétique : MonoBehaviour
{
    private int NumGenerations=0;
    public int NumPoints=10;
    private Vector3[] points;
    private BitArray[] ADN;
    private Vector3[] parents = new Vector3[2];
    [SerializeField]private GameObject BonHommePrefab;
    
    string outputString = "";
    
    int FloatToInt(float floatValue)
    {
        return BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
    }

    float IntToFloat(int intValue)
    {
        return BitConverter.ToSingle(BitConverter.GetBytes(intValue), 0);
    }

    
    string ConvertToBit(Vector3 inputVector)
    {
        int xInt = FloatToInt(inputVector.x);
        int yInt = FloatToInt(inputVector.y);
        int zInt = FloatToInt(inputVector.z);

        outputString = Convert.ToString(xInt, 2).PadLeft(32, '0')
                       + Convert.ToString(yInt, 2).PadLeft(32, '0')
                       + Convert.ToString(zInt, 2).PadLeft(32, '0');

        return outputString;
    }

  
    Vector3 ConvertToFloat(string binary)
    {
        string xBin = binary.Substring(0, 32);
        string yBin = binary.Substring(32, 32);
        string zBin = binary.Substring(64, 32);

        float xFloat = IntToFloat(Convert.ToInt32(xBin, 2));
        float yFloat = IntToFloat(Convert.ToInt32(yBin, 2));
        float zFloat = IntToFloat(Convert.ToInt32(zBin, 2));

        return new Vector3(xFloat, yFloat, zFloat);
    }


    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[NumPoints];
        InitialisationPopulation();
        CalculFitness();
        SelectionnerParents();
    }

    // Update is called once per frame
    void Update()
    {
       /*  while()
        {
            NumGenerations++;
            
            Recombinaison();
            Mutation();
        } */
    }

    void InitialisationPopulation()
    {
        for (int i=0; i< NumPoints; i++ )
        {
            points[i] = new Vector3(Random.Range(-282, 47), 90, Random.Range(-281, 50));
        }
    }

    void CalculFitness()
    {
        int GroundlayerMask = 6;

        for (int i=0; i< NumPoints; i++ )
        {
            RaycastHit hit;

            if (Physics.Raycast(points[i], Vector3.down, out hit, Mathf.Infinity))
            {
                points[i] = hit.point;
                Instantiate(BonHommePrefab, points[i], transform.rotation);
            }
             
        }
    }

    Vector3[] SelectionnerParents()
    {
        Vector3 joueur1 = new Vector3(0,0,0);
        Vector3 joueur2 = new Vector3(0,0,0);
        Vector3 joueur3 = new Vector3(0,0,0);

        for (int i=0 ; i<2; i++)
        {
            joueur1 = points[Random.Range(0, NumPoints)];
            joueur2 = points[Random.Range(0, NumPoints)];
            joueur3 = points[Random.Range(0, NumPoints)];
            
            if (joueur1 == joueur2)
            {
                joueur2 = points[Random.Range(0, NumPoints)];
            }
            if (joueur3==joueur1 || joueur3 ==joueur2)
            {
                joueur3 = points[Random.Range(0, NumPoints)];
            }

            float parentY = Mathf.Max(joueur1.y, joueur2.y, joueur3.y );
        }
        /*Vector3 originalVector = new Vector3(9, 10, 3);
        print("Original Vector3: " + originalVector);
        print("Binary: " + ConvertToBit(originalVector));
        Vector3 newVector = ConvertToFloat(outputString);
        print("New Vector3: " + reconstructedVector);*/
        return parents;
    }
}
