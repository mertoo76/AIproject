using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Global
{
    public const int CHROMOSOME_SIZE = 90;
    public const int POPULATION_SIZE = 30;
    public const double MUTATION_RATE = 0.3;
    public static readonly System.Random random = new System.Random();
    public static readonly GeneticAlgorithm generic = new GeneticAlgorithm();
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
    public void incGenes()
    {
        for (int i = 0; i < this.getChromozom().Count; i++)
        {
            int j = 0;
            while (j < Global.CHROMOSOME_SIZE)
            {
                double sayi = Global.random.NextDouble();
                //sola dön
                if (sayi >= 0.66)
                {
                    this.getChromozom()[i].getGenes().Add(0);
                }
                else if (sayi >= 0.33)
                {
                    //saga dön
                    this.getChromozom()[i].getGenes().Add(1);
                }
                else
                {   //düz git
                    this.getChromozom()[i].getGenes().Add(2);
                }

                j++;
            }

        }
    }


} //population




public class Chromosome
{

    List<int> genes = new List<int>();
    float fitness;
    int lastPos = 0;
    //int cSize;
    public Chromosome()
    {
        fitness = 0;
        int i = 0;

        while (i < Global.CHROMOSOME_SIZE)
        {
            double sayi = Global.random.NextDouble();
            //sola dön
            if (sayi >= 0.66)
            {
                genes.Add(0);
            }
            else if (sayi >= 0.33)
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
    public Chromosome(int size)
    {
        fitness = 0;
        int i = 0;

        while (i < size)
        {
            double sayi = Global.random.NextDouble();
            //sola dön
            if (sayi >= 0.66)
            {
                genes.Add(0);
            }
            else if (sayi >= 0.33)
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

    public int getLastPos()
    {
        return lastPos;
    }

    public void setLastPos(int lastPos)
    {
        this.lastPos = lastPos;
    }

}



public class GeneticAlgorithm
{
    public int NUMB_OF_ELITE_CHROMOSOMES = 5;
    public int TOURNAMENT_SELECTION_SIZE = 12;
    public Population evolve(Population pop)
    {
        pop = mutate_population(crossover_population(pop));
        //pop.getChromozom()[3].getGenes().Add(1);
        return pop;
    }

    public Population selectTournamentPop(Population pop)
    {
        Population tournament_pop = new Population(0);
        int i = 0;
        while (i < TOURNAMENT_SELECTION_SIZE)
        {
            int sayi = Global.random.Next(0, Global.POPULATION_SIZE);
            tournament_pop.getChromozom().Add(pop.getChromozom()[sayi]);
            i++;
        }
        tournament_pop.getChromozom().Sort((a, b) => a.getFitness().CompareTo(b.getFitness()));//fitnesa göre sort etmeyi ayarla
        tournament_pop.getChromozom().Reverse();
        return tournament_pop;

    }

    public Chromosome crossover_chromosomes(Chromosome chromosome1, Chromosome chromosome2)
    {
        Chromosome crossover_chrom = new Chromosome(chromosome1.getGenes().Count);
        int i = 0;
        //for (i = 0; i < chromosome1.getGenes().Count; i++)
        //{
        int sayi = Global.random.Next(0, chromosome1.getGenes().Count);
        //        if (sayi < 0.5)
        //        {
        //            crossover_chrom.getGenes()[i] = chromosome1.getGenes()[i];
        //        }
        //        else
        //        {
        //            crossover_chrom.getGenes()[i] = chromosome2.getGenes()[i];
        //        }
        for (int j = 0; j < sayi; j++)
            crossover_chrom.getGenes()[j] = chromosome1.getGenes()[j];
        for (int j = sayi; j < chromosome1.getGenes().Count; j++)
            crossover_chrom.getGenes()[j] = chromosome2.getGenes()[j];


        //}

        return crossover_chrom;
    }

    public void mutate_chromosome(Chromosome chromosome)
    {
        int i;
        for (i = 0; i < chromosome.getGenes().Count; i++)
        {
            double sayi = Global.random.NextDouble();
            if (sayi < Global.MUTATION_RATE)
            {
                chromosome.getGenes()[i] = (chromosome.getGenes()[i] + 1) % 3;
            }
        }

    }

    public void mutate_elite_chromosome(Chromosome chromosome)
    {
        int i;
        for (i = chromosome.getGenes().Count - 5 * Global.CHROMOSOME_SIZE; i < chromosome.getGenes().Count; i++)
        {
            double sayi = Global.random.NextDouble();
            if (sayi < Global.MUTATION_RATE)
            {
                chromosome.getGenes()[i] = (chromosome.getGenes()[i] + 1) % 3;
            }
        }

    }

    public Population crossover_population(Population pop)
    {
        Population crossover_pop = new Population(0);
        int i;
        for (i = 0; i < this.NUMB_OF_ELITE_CHROMOSOMES; i++)
        {
            Debug.Log("Elite fitness: " + pop.getChromozom()[i].getFitness() + " i: " + i);
            crossover_pop.getChromozom().Add(pop.getChromozom()[i]);
        }
        while (i < Global.POPULATION_SIZE)
        {
            Debug.Log("Elite fitness: " + pop.getChromozom()[i].getFitness() + " i: " + i);
            Chromosome chromosome1 = selectTournamentPop(pop).getChromozom()[0];
            Chromosome chromosome2 = selectTournamentPop(pop).getChromozom()[0];
            crossover_pop.getChromozom().Add(crossover_chromosomes(chromosome1, chromosome2));
            //Debug.Log("Cross "+crossover_pop.getChromozom()[i].Equals(pop.getChromozom()[i]));
            i++;
        }
        return crossover_pop;
    }

    public Population mutate_population(Population pop)
    {
        int i;
        for (i = 0; i < NUMB_OF_ELITE_CHROMOSOMES; i++)
        {
            mutate_elite_chromosome(pop.getChromozom()[i]);
        }
        for (i = NUMB_OF_ELITE_CHROMOSOMES; i < Global.POPULATION_SIZE; i++)
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
    public CarData(GameObject car, Chromosome chromosome)
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

public class generic : MonoBehaviour
{
    public float Speed = 1f;
    public float TurnSpeed = 50f;
    public GameObject Car;
    List<GameObject> carList = new List<GameObject>();
    public int count = 0;
    Population pop;
    int generation_number = 0;
    // Use this for initialization

    public bool isEqual(List<int> a, List<int> b)
    {
        // Debug.Log("isEqual Counts: "+a.Count + " " + b.Count);
        if (a.Count != b.Count)
            return false;
        for (int i = 0; i < b.Count; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }

    void Start()
    {
        int sil = 1;
        Debug.Log("Hello" + sil);
        pop = new Population(Global.POPULATION_SIZE);
        pop.getChromozom().Sort((a, b) => a.getFitness().CompareTo(b.getFitness()));//fitnesa göre sort etmeyi ayarla
        pop.getChromozom().Reverse();

        //List<int> gene3 = pop.getChromozom()[3].getGenes().ToList<int>();
        //Debug.Log("a"+gene3.Count);
        pop = Global.generic.evolve(pop);
        //List<int> gene3new = pop.getChromozom()[3].getGenes();
        //Debug.Log("a" + gene3new.Count);
        //Debug.Log(isEqual(gene3, gene3new));
        //  generic.

        int i;

        for (i = 0; i < Global.POPULATION_SIZE; i++)
        {

            GameObject obj = Instantiate(Car, Car.transform.position, Car.transform.rotation) as GameObject;

            carList.Add(obj);

        }
        //StartCoroutine(MyMethod());

    }

    IEnumerator MyMethod()
    {
        Debug.Log("Before Waiting 2 seconds");
        yield return new WaitForSeconds(5);
        Debug.Log("After Waiting 2 Seconds");
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        var gas = 1;
        var steer = 1;
        var moveDist = gas * Speed * Time.deltaTime;
        var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

        int i;

        count++;

        //StartCoroutine(MyMethod());
        i = 0;
        int j = 0;
        //pop.incGenes();
        foreach (GameObject x in carList)
        {
            if (x != null)
            {
                Car carCont = x.GetComponent<Car>();
                if (carCont.roadNumber != 0)
                {

                    j = pop.getChromozom()[i].getLastPos();
                    //Hareketleri yaptır.
                    //hareketleri yaptırırken
                    //Debug.Log("Chromosome:"+ i);
                    int temp = j;
                    while (j < pop.getChromozom()[i].getGenes().Count && j - temp < Global.CHROMOSOME_SIZE)
                    {

                        //Debug.Log("gene:"+ pop.getChromozom()[i].getGenes()[j]);


                        if (pop.getChromozom()[i].getGenes()[j] == 0)
                        {
                            //float step = Speed * Time.deltaTime;
                            //x.transform.position += Vector3.MoveTowards(transform.position, x.transform.position, step);
                            x.transform.Translate(Vector3.forward * moveDist);
                            //Rigidbody rb = x.GetComponent<Rigidbody>();
                            //rb.MovePosition(Vector3.forward * moveDist);
                            // x.transform.position = x.transform.position + Vector3.forward * 1;
                            // x.transform.position += x.transform.forward * Time.deltaTime * 1;
                        }
                        else if (pop.getChromozom()[i].getGenes()[j] == 1) { x.transform.Rotate(new Vector3(0, 2, 0)); }
                        else if (pop.getChromozom()[i].getGenes()[j] == 2) { x.transform.Rotate(new Vector3(0, -2, 0)); }
                        j++;
                    }

                    if (j == pop.getChromozom()[i].getGenes().Count)
                    {
                        Debug.Log("Increasing...");
                        pop.incGenes();

                    }
                    pop.getChromozom()[i].setLastPos(j);
                    //  if (pop.getChromozom()[i].getFitness() < carCont.roadNumber)
                    pop.getChromozom()[i].setFitness(carCont.roadNumber);
                    //Debug.Log("roadNum:" + pop.getChromozom()[i].getFitness());
                }
            }
            i++;
        }

        int null_count = 0;
        foreach (GameObject x in carList)
        {
            if (x == null)
            {
                null_count++;
            }
        }
        if (null_count == carList.Count)
        {
            carList.Clear();

            for (i = 0; i < Global.POPULATION_SIZE; i++)
            {

                GameObject obj = Instantiate(Car, Car.transform.position, Car.transform.rotation) as GameObject;

                carList.Add(obj);
                pop.getChromozom()[i].setLastPos(0);
            }
            Debug.Log("Evolving...");
            Debug.Log("Genes: " + pop.getChromozom()[0].getGenes().Count);
            pop.getChromozom().Sort((a, b) => a.getFitness().CompareTo(b.getFitness()));//fitnesa göre sort etmeyi ayarla
            pop.getChromozom().Reverse();
            //List<int> once = pop.getChromozom()[3].getGenes().ToList<int>();
            pop = Global.generic.evolve(pop);
            //List<int> sonra = pop.getChromozom()[3].getGenes().ToList<int>();
            //Debug.Log("0 Degisti. : " + isEqual(once,sonra));
            generation_number++;
            Debug.Log("Generation: " + generation_number);
        }


        //carList.Clear();




        //Debug.Log("---- Fitness ----");
        //Debug.Log(pop.getChromozom()[0].getFitness());
        //Debug.Log(pop.getChromozom()[0].getGenes()[0]);
        //Debug.Log(pop.getChromozom()[1].getFitness());
        //Debug.Log(pop.getChromozom()[2].getFitness());
        //Debug.Log(pop.getChromozom()[3].getFitness());
        //Debug.Log(pop.getChromozom()[4].getFitness());
        //Debug.Log(pop.getChromozom()[5].getFitness());
        //Debug.Log(pop.getChromozom()[6].getFitness());
        //Debug.Log(pop.getChromozom()[7].getFitness());
        //Debug.Log(pop.getChromozom()[8].getFitness());
        //Debug.Log(pop.getChromozom()[9].getFitness());
        //Debug.Log(pop.getChromozom()[0].getLastPos());
        //Debug.Log(pop.getChromozom()[1].getLastPos());
        //Debug.Log(pop.getChromozom()[2].getLastPos());
        //Debug.Log(pop.getChromozom()[3].getLastPos());
        //Debug.Log(pop.getChromozom()[4].getLastPos());



    }




}

