using System;
using GeneticAlgorithmLibrary;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneticAlgorithm : MonoBehaviour
{
    public static Random Random = new Random();

    public string target = "To be or not to be.";
    public int minSize = 1;
    public int maxSize = 32;
    public int batchSize = 10;

    public ConsoleUI consoleUI;
    public GeneticAlgorithm<string> geneticAlgorithm;

    private void Start()
    {
        StartNewGenenticAlgorithm();
    }

    public void StartNewGenenticAlgorithm()
    {
        geneticAlgorithm = new GeneticAlgorithm<string>
        (
            populationSize: 50,
            mutationRate: 0.01,
            replicationRate: 0.01,
            initializeFunction: GetRandomString,
            fitnessFunction: CalculateFitness,
            crossoverFunction: Crossover,
            mutateFunction: Mutate
        );   
    }

    private void Update()
    {
        if (geneticAlgorithm.Best == target)
            return;

        if (!consoleUI) consoleUI = FindObjectOfType<ConsoleUI>();
        if (!consoleUI) return;

        for (var i = 0; i < batchSize; i++)
        {
            if (geneticAlgorithm.generation % 100 == 0 || geneticAlgorithm.Best == target)
            consoleUI.WriteLine(
                $"generation: {geneticAlgorithm.generation} fitness: {geneticAlgorithm.BestFitness} best: {geneticAlgorithm.Best}");

            if (geneticAlgorithm.Best == target)
                return;
            
            geneticAlgorithm.Evolve();
        }
    }

    private string GetRandomString()
    {
        return RandomString(RandomInt(minSize, maxSize));
    }

    private double CalculateFitness(string entity)
    {
        var range = maxSize - minSize;
        var lengthDistance = Math.Abs(target.Length - entity.Length);
        var lengthFitness = range - lengthDistance;

        var matchingLetters = 0;
        for (var i = 0; i < target.Length && i < entity.Length; i++)
            if (target[i] == entity[i])
                matchingLetters++;
        var matchingFitness = matchingLetters;

        return lengthFitness + matchingFitness * 2;
    }

    private string Crossover(string parentA, string parentB)
    {
        if (parentA.Length > parentB.Length)
        {
            var temp = parentB;
            parentB = parentA;
            parentA = temp;
        }

        var cutoff = RandomInt(0, parentA.Length);
        var length = RandomInt(parentA.Length, parentB.Length + 1);
        var child = "";
        for (var i = 0; i < length; i++)
        {
            if (i < cutoff)
                child += parentA[i];
            else
                child += parentB[i];
        }

        return child;
    }

    private string Mutate(string str, double mutationRate)
    {
        var mutant = "";
        var selection = RandomFloat01();
        var length = str.Length;
        if (selection < mutationRate)
            length += RandomBool() ? 1 : -1;

        if (length > str.Length)
            str += RandomChar();

        for (var i = 0; i < length; i++)
        {
            selection = RandomFloat01();
            if (selection < mutationRate)
                mutant += RandomChar();
            else
                mutant += str[i];
        }

        return mutant;
    }

    public int RandomInt(int minValue, int maxValue) => Random.Range(minValue, maxValue);

    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789. ";
    public char RandomChar() => chars[Random.Range(0, chars.Length)];

    public string RandomString(int length)
    {
        var str = "";
        for (var i = 0; i < length; i++)
            str += RandomChar();
        return str;
    }

    public bool RandomBool() => Random.Range(0, 2) == 1;

    public float RandomFloat01() => Random.Range(0f, 1f);
}