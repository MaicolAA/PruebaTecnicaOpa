using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public int MinCalorias {get; set; }

    [BindProperty]
    public int PesoMaximo {get; set; }

    [BindProperty]
    public string Elementos {get; set;}

    public List<string> Resultados {get; set; }

    public void OnPost()
    {
        List<element> elementosList = parseElementos(Elementos);
        Resultados = findOptimusCombination(elementosList, MinCalorias, PesoMaximo);
    }

    private List<element> parseElementos(string elementosString)
    {
        var elementosList = new List<element>();
        var lineas = elementosString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var linea in lineas)
        {
            var partes = linea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 3 && int.TryParse(partes[1], out int peso) && int.TryParse(partes[2], out int calorias))
            {
                elementosList.Add(new element { name = partes[0], height = peso, calories = calorias });
            }
        }

        return elementosList;
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
