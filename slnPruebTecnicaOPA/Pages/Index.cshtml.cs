using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public int minCalorias {get; set; }

    [BindProperty]
    public int pesoMaximo {get; set; }

    [BindProperty]
    public int cantElementos { get; set; }

    [BindProperty]
    public List<element> Elementos { get; set; }

    public List<string> Resultados {get; set; }

    public void OnPost()
    {
        Resultados = findOptimusCombination(Elementos, minCalorias, pesoMaximo);
    }

    private List<string> findOptimusCombination(List<element> elementos, int caloriasMinimas, int pesoMaximo)
    {
        var resultados = new List<List<element>>();

        void Backtrack(int index, int totalCalorias, int totalPeso, List<element> combination)
        {
            if (totalCalorias >= caloriasMinimas && totalPeso <= pesoMaximo)
            {
                resultados.Add(combination.ToList());
            }

            for (int i = index; i < elementos.Count; i++)
            {
                element elementoActual = elementos[i];
                combination.Add(elementoActual);
                Backtrack(i + 1, totalCalorias + elementoActual.calories, totalPeso + elementoActual.height, combination);
                combination.Remove(elementoActual);
            }
        }

        Backtrack(0, 0, 0, new List<element>());

        return resultados.Select(conjunto => string.Join(" ", conjunto.Select(e => e.name))).ToList();
    }
}
