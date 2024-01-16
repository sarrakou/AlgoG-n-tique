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

/*apres la suppression des 2 plus nuls il faut ^supprimer tous les instances et mes faire respawn a partir du tableau point*/
public class AlgoGénétique : MonoBehaviour
{
    private int NumGenerations=0;
    public int NumPoints=10;
    private Vector3[] points;
    private BitArray[] ADN;
    private Vector3[] parents = new Vector3[2];
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
    
    //test
   public void test()
    {
        print("avant");

        for (int i = 0; i < points.Length; i++)
        {
            print((Vector3)points[i]);
        }
        supppLesDeuxPlusNuls();
        print("apres");
        for(int i = 0; i < points.Length; i++)
        {
            print((Vector3)points[i]);
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

    
    //selection de la plus nul 
    int SelectionnerLePlusNul()
    {
        Vector3 le_plus_bas = points[0];
        int id = 0;

        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].y < le_plus_bas.y)
            {
                le_plus_bas = points[i];
                id = i;
            }
        }
        return id;

    }
     //methode pour supprimer les 2 plus nuls
    void supppLesDeuxPlusNuls()
    {
        RemoveAt(ref points, SelectionnerLePlusNul());
        RemoveAt(ref points, SelectionnerLePlusNul());
        NumPoints -= 2;
    }

    //methode pour suprimer un element d'un tableau 
    static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {

            arr[a] = arr[a + 1];
        }
        Array.Resize(ref arr, arr.Length - 1);
    }







}
