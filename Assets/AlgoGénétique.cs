using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

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
    private List<Vector3> points = new List<Vector3>();
    private BitArray[] ADN;
    private Vector3[] parents = new Vector3[2];
    private string[] children = new string[2];
    [SerializeField]private GameObject BonHommePrefab;
    [SerializeField]private TMP_Text NumGenerationTxt;
    
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
        StartCoroutine(Generation());
    }

    IEnumerator Generation()
    {
        while(NumGenerations<10)
        {
            NumGenerations++;
            NumGenerationTxt.text = "Génération n° "+NumGenerations;
            ClearOldGeneration();
            InitialisationPopulation();
            CalculFitness();
            
            parents = SelectionnerParents();
            children = CrossOver(ConvertToBit(parents[0]), ConvertToBit(parents[1]));
            Mutation(children[0]);
            points.Add(ConvertToFloat(children[0]));
            Mutation(children[1]);
            points.Add(ConvertToFloat(children[1]));
            supppLesDeuxPlusNuls();
            yield return new WaitForSeconds(2f);
        }
    }


    string[] CrossOver(string parent1, string parent2)
    {
        int randomPlacement = Random.Range(0, parent1.Length);
        string child1 = parent1.Substring(0, randomPlacement) + parent2.Substring(randomPlacement, parent2.Length-randomPlacement);
        string child2 = parent2.Substring(0, randomPlacement) + parent1.Substring(randomPlacement, parent1.Length-randomPlacement);
        
        children[0] = child1;
        children[1] = child2;
        

        return children;
    } 

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
            points.Add(new Vector3(Random.Range(-282, 47), 90, Random.Range(-281, 50)));
        }
    }

    void ClearOldGeneration()
    {
        // Destroy the LevelBlock
        foreach (var gameObj in GameObject.FindGameObjectsWithTag("bonhomme")){
            Destroy(gameObj);
        }
        points.Clear();
        
    }
    void CalculFitness()
    {
        int GroundlayerMask = 6;

        for (int i=0; i< points.Count; i++ )
        {
            RaycastHit hit;

            if (Physics.Raycast(points[i], Vector3.down, out hit, Mathf.Infinity))
            {
                points[i] = hit.point;
                GameObject newBonHomme = Instantiate(BonHommePrefab, points[i], transform.rotation);
                newBonHomme.tag = "bonhomme";
            }
             
        }
    }

    Vector3[] SelectionnerParents()
    {
        Vector3 joueur1 = new Vector3();
        Vector3 joueur2 = new Vector3();
        Vector3 joueur3 = new Vector3();
        Vector3[] p = new Vector3[2];

        for (int i=0; i<2; i++)
        {
            joueur1 = points[Random.Range(0, points.Count)];
            joueur2 = points[Random.Range(0, points.Count)];
            joueur3 = points[Random.Range(0, points.Count)];
            
            if (joueur1 == joueur2)
            {
                joueur2 = points[Random.Range(0, points.Count)];
            }
            if (joueur3==joueur1 || joueur3 ==joueur2)
            {
                joueur3 = points[Random.Range(0, points.Count)];
            }

            float parentY = Mathf.Max(joueur1.y, joueur2.y, joueur3.y);
            if (parentY==joueur1.y)
            {
                p[i] = joueur1;
            } else if (parentY==joueur2.y)
            {
                p[i] = joueur2;
            } else
            {
                p[i] = joueur3;
            }
            
        }
        return p;
    }

    //selection de la plus nul 
    int SelectionnerLePlusNul()
    {
        Vector3 le_plus_bas = points[0];
        int id = 0;

        for (int i = 1; i < points.Count; i++)
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
        points.RemoveAt(SelectionnerLePlusNul());
        points.RemoveAt(SelectionnerLePlusNul());
    }

    //methode pour suprimer un element d'un tableau 
  /*   static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            arr[a] = arr[a + 1];
        }
        Array.Resize(ref arr, arr.Length - 1);
    } */
}
