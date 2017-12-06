using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nombre de variables principales :");
            int nbValPrincipal = int.Parse(Console.ReadLine());
            int[] tabValeurPrincipal = new int[nbValPrincipal];
            // Pour affichage Z
            string valZ = "Z = ";
            ////////////////////// On récup valeur principales //////////////////////
            for (int i = 0; i < nbValPrincipal; i++)
            {
                Console.WriteLine("Multiplicateur de la variable principale " + (i + 1) + " :");
                tabValeurPrincipal[i] = int.Parse(Console.ReadLine());

                if(i+1 == nbValPrincipal) {
                    // Permet d'afficher correctement Z
                    valZ += "(" + tabValeurPrincipal[i] + "x" + (i + 1) + ") ";
                } else valZ +=  "(" + tabValeurPrincipal[i] + "x" + (i +1) + ") + ";
            }
            // Affichage de Z et saut de ligne
            Console.WriteLine("");
            Console.WriteLine(valZ);
            Console.WriteLine("");

            Console.WriteLine("Nombre de variables d'écarts :");
            int nbValEcart = int.Parse(Console.ReadLine());
            int[] tabValeurEcart = new int[nbValEcart];
            ////////////////////// On récup valeur d'écarts //////////////////////
            for (int i = 0; i < nbValEcart; i++)
            {
                Console.WriteLine("Multiplicateur de la variable d'écart " + (i + 1) + " :");
                tabValeurEcart[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("");

            int[,] tabSousContraintes = new int[nbValEcart, nbValPrincipal+2];
            ////////////////////// On récup les sous-contraintes //////////////////////
            for (int ligne = 0; ligne < nbValEcart; ligne++)
            {
                for (int colonne = 0; colonne < nbValPrincipal + 2; colonne++)
                {
                    Console.WriteLine("Multiplicateur de la variable principale " + (ligne + 1) + " pour la sous-contrainte " + (colonne + 1) + " :");
                    tabSousContraintes[ligne, colonne] = int.Parse(Console.ReadLine());
                }
                // Voir pour comment gérer l'ajout des sous-contraintes
            }


            // "Pause écran"
            Console.ReadLine();
        }
    }
}
