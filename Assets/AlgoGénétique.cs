using System;
using System.Collections;
using System.Collections.Generic;
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
    private string[] children = new string[2];
    [SerializeField]private GameObject BonHommePrefab;
    
    string outputString = "";

    string ConvertirEnBits(Vector3 inputVector)
    {
        outputString += BitConverter.ToString(BitConverter.GetBytes(inputVector.x));
        outputString += BitConverter.ToString(BitConverter.GetBytes(inputVector.y));
        outputString += BitConverter.ToString(BitConverter.GetBytes(inputVector.z));
      return outputString;  
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

/*     string[] CrossOver(string parent1, string parent2)
    {
        int randomPlacement = Random.Range(0, parent1.Length);
        string child1 = parent1.Substring(0, randomPlacement) + parent2.Substring(randomPlacement, parent2.Length-randomPlacement);
        string child2 = parent2.Substring(0, randomPlacement) + parent1.Substring(randomPlacement, parent1.Length-randomPlacement);
        
        children[0] = child1;
        children[1] = child2;

        return children;
    } */

    string Mutation(string ADN)
    {
        int randomIndex = Random.Range(0, ADN.Length);
        string newADN;
        if (ADN[randomIndex]==0)
        {
            newADN = ADN.Substring(0,randomIndex-1)+'1' + ADN.Substring(randomIndex-1, ADN.Length-randomIndex-1);
        } else 
        {
            newADN = ADN.Substring(0,randomIndex-1)+'0' + ADN.Substring(randomIndex-1, ADN.Length-randomIndex-1);
        }

        return newADN;
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
        
        print(ConvertirEnBits(new Vector3(9,10,3)));
        return parents;
    }
}
