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
            Console.WriteLine("Nb valeurs principales :");
            int nbValPrincipal = int.Parse(Console.ReadLine());
            int[] tabValeurPrincipal = new int[nbValPrincipal];
            // Pour affichage Z
            string valZ = "Z = ";
            ////////////////////// On récup valeur principales //////////////////////
            for (int i = 0; i < nbValPrincipal; i++)
            {
                Console.WriteLine("Numéro valeur principale " + (i + 1) + " :");
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

            Console.WriteLine("Nb valeurs d'écarts :");
            int nbValEcart = int.Parse(Console.ReadLine());
            int[] tabValeurEcart = new int[nbValEcart];
            ////////////////////// On récup valeur d'écarts //////////////////////
            for (int i = 0; i < nbValEcart; i++)
            {
                Console.WriteLine("Numéro valeur d'écart " + (i + 1) + " :");
                tabValeurEcart[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("");

            ////////////////////// On récup les sous-contraintes //////////////////////
            for (int i = 0; i < nbValEcart; i++)
            {
                // Voir pour comment gérer l'ajout des sous-contraintes
            }


            // "Pause écran"
            Console.ReadLine();
        }
    }
}
