﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Global
{
    public const int CHROMOSOME_SIZE = 100;
    public const int POPULATION_SIZE = 10;
    public const double MUTATION_RATE = 0.2;
}

public class Population
{
    List<Chromosome> chromosomes = new List<Chromosome>();
    int size;
    //int cSize;
    public Population(int size)
    {
        int i = 0;
        this.size = size;
        //this.cSize = cSize;
        while (i < size)
        {
            Chromosome newChro = new Chromosome();
            chromosomes.Add(newChro);
            i++;
        }
        
    }

   public List<Chromosome> getChromozom()
    {
        return this.chromosomes;

    }


} //population




public class Chromosome
{
    List<int> genes = new List<int>();
    float fitness;
    //int cSize;
    public Chromosome()
    {
        fitness = 0;
        int i = 0;
        //this.cSize = cSize;
        System.Random random = new System.Random();
        while (i < Global.CHROMOSOME_SIZE)
        {
            double sayi = random.NextDouble();
            //sola dön
            if (sayi >= 0.66)
            {
                genes.Add(0);
            }else if(sayi>= 0.33)
            {
                //saga dön
                genes.Add(1);
            }
            else
            {   //düz git
                genes.Add(2);
            }

            i++;

        }

    }

    public List<int> getGenes()
    {

        return this.genes;
    }

    public float getFitness()
    {
        return fitness;
    }

    public void setFitness(float fitness)
    {
        this.fitness = fitness;
    }

}



public class GeneticAlgorithm
{
 public  int NUMB_OF_ELITE_CHROMOSOMES = 2;
 public  int TOURNAMENT_SELECTION_SIZE = 4;
 public  System.Random random = new System.Random();
  public  Population evolve(Population pop)
    {
        mutate_population(crossover_population(pop));
        return pop;
    }

    public Population selectTournamentPop(Population pop)
    {
        Population tournament_pop =new Population(0);
        int i = 0;
        while (i < TOURNAMENT_SELECTION_SIZE)
        {
            int sayi = random.Next(0, Global.POPULATION_SIZE);
            tournament_pop.getChromozom().Add(pop.getChromozom()[sayi]);
            i++;
        }
        //tournament_pop.getChromozom().Sort((a) => (a.getFitness()>b.getFitness()));//fitnesa göre sort etmeyi ayarla
        return tournament_pop;
        
    }

    public Chromosome crossover_chromosomes(Chromosome chromosome1, Chromosome chromosome2)
    {
        Chromosome crossover_chrom = new Chromosome();
        int i = 0;
        for (i = 0; i < Global.CHROMOSOME_SIZE;i++)
        {
            double sayi = random.NextDouble();
            if (sayi < 0.5)
            {
                crossover_chrom.getGenes()[i] = chromosome1.getGenes()[i];
            }
            else
            {
                crossover_chrom.getGenes()[i] = chromosome2.getGenes()[i];
            }


        }

        return crossover_chrom;
    }

    public void mutate_chromosome(Chromosome chromosome)
    {
        int i;
        for (i = 0; i < Global.CHROMOSOME_SIZE; i++)
        {
            double sayi = random.NextDouble();
               if(sayi<Global.MUTATION_RATE )
            {
                chromosome.getGenes()[i] = (chromosome.getGenes()[i] + 1) % 3;
            }
        }

    }

    public Population crossover_population(Population pop)
    {
        Population crossover_pop = new  Population(0);
        int i;
        for (i = 0; i < this.NUMB_OF_ELITE_CHROMOSOMES; i++)
        {
            crossover_pop.getChromozom().Add(pop.getChromozom()[i]);
        }
        i = this.NUMB_OF_ELITE_CHROMOSOMES;
        while (i < Global.POPULATION_SIZE)
        {
            Chromosome chromosome1 = selectTournamentPop(pop).getChromozom()[0];
            Chromosome chromosome2 = selectTournamentPop(pop).getChromozom()[0];
            crossover_pop.getChromozom().Add(crossover_chromosomes(chromosome1, chromosome2));
            i++;
        }
        return crossover_pop;    
    }

    public Population mutate_population(Population pop)
    {
        int i;
        for (i = 0; i < Global.POPULATION_SIZE; i++)
        {
            mutate_chromosome(pop.getChromozom()[i]);
        }
        return pop;
    }

}

public class CarData
{
    GameObject car;
    Chromosome chromosome;
    public CarData(GameObject car, Chromosome chromosome )
    {
        this.car = car;
        this.chromosome = chromosome;

    }

    public GameObject getCar()
    {
        return this.car;
    }
    public void setCar(GameObject car)
    {
        this.car = car;
    }

    public Chromosome getChromosome()
    {
        return this.chromosome;
    }
    public void setChromosome(Chromosome choromosome)
    {
        this.chromosome = choromosome;
    }

}

public class generic : MonoBehaviour {
    public float Speed = 1f;
    public float TurnSpeed = 5;
    public GameObject Car;
    List<GameObject> carList = new List<GameObject>();
    public int count = 0;
    Population pop;
    int generation_number = 0;
    // Use this for initialization

    void Start () {
        int sil = 1;
        Debug.Log("Hello"+sil);
         pop = new Population(Global.POPULATION_SIZE);
        //pop.getChromozom().Sort((a,b) => (a.getFitness()>b.getFitness()));//fitnesa göre sort etmeyi ayarla
        
        int i;

        for (i = 0; i < Global.POPULATION_SIZE; i++)
        {

            GameObject obj = Instantiate(Car, Car.transform.position, Car.transform.rotation) as GameObject;

            carList.Add(obj);

        }
        StartCoroutine(MyMethod());

    }

    IEnumerator MyMethod()
    {
        Debug.Log("Before Waiting 2 seconds");
        yield return new WaitForSeconds(5);
        Debug.Log("After Waiting 2 Seconds");
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        

        var gas = 1;
        var steer = 1;
        var moveDist = gas * Speed * Time.deltaTime;
        var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

        int i;
      


        count++;
        GeneticAlgorithm generic = new GeneticAlgorithm();
        pop = generic.evolve(pop);
        StartCoroutine(MyMethod());
        i = 0;
        int j = 0;
        foreach (GameObject x in carList)
        {
            if (x != null) { 
                j = 0;
            //Hareketleri yaptır.
            //hareketleri yaptırırken
            while (j < pop.getChromozom()[i].getGenes().Count)
            {

                // Debug.Log("gene:"+ pop.getChromozom()[i].getGenes()[j]);


                if (pop.getChromozom()[i].getGenes()[j] == 0)
                {
                    x.transform.Translate(Vector3.forward * moveDist);
                    // x.transform.position = x.transform.position + Vector3.forward * 1;
                   // x.transform.position += x.transform.forward * Time.deltaTime * 1;
                }
                  else if(pop.getChromozom()[i].getGenes()[j] == 1) { x.transform.Rotate(new Vector3(0, turnAngle, 0)); }
                 else if(pop.getChromozom()[i].getGenes()[j] == 2) { x.transform.Rotate(new Vector3(0, -turnAngle, 0)); }
                j++;
            }


            Car carCont = x.GetComponent<Car>();
            pop.getChromozom()[i].setFitness(carCont.roadNumber);
            Debug.Log("roadNum:" + pop.getChromozom()[i].getFitness());
            i++;
        }
        }


        /*
        foreach (GameObject x in carList)
        {
          
            Destroy(x);
        }
        
        carList.Clear();
        */
    
       // pop.getChromozom().Sort((a,b) => (a.getFitness()>b.getFitness()));//fitnesa göre sort etmeyi ayarla
        generation_number++;

    }


   

}
