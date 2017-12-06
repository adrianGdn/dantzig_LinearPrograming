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
            // Affichage de Z et sauts de lignes
            Console.WriteLine("\n\n" + valZ+ "\n\n");

            Console.WriteLine("Nombre de variables d'écarts :");
            int nbValEcart = int.Parse(Console.ReadLine());
            Console.WriteLine("\n");


            // Ici on fait "nbValPrincipal+2" car on stocke dans le tableau les variables principales + la variable d'écart + la constante
            int[,] tabSousContraintes = new int[nbValEcart, nbValPrincipal+2];
            ////////////////////// On récup les sous-contraintes //////////////////////
            for (int ligne = 0; ligne < nbValEcart; ligne++)
            {
                for (int colonne = 0; colonne < nbValPrincipal + 2; colonne++)
                {
                    // Permet de changer le texte en fonction de la valeur demandé
                    if (colonne == (nbValPrincipal + 1))
                    {
                        // On stocke la constante en dernière place du tableau
                        Console.WriteLine("Valeur de la constante pour la sous-contrainte " + (ligne + 1) + " :");
                        tabSousContraintes[ligne, colonne] = int.Parse(Console.ReadLine());
                        // On fait un saut de ligne pour améliorer la lecture
                        Console.WriteLine("\n");
                    }
                    else if (colonne == (nbValPrincipal))
                    {
                        // On stocke la variable d'écart en avant-dernière place du tableau qui par défaut pour le premier passage vaut 1
                        tabSousContraintes[ligne, colonne] = 1;
                    }
                    else
                    {
                        // Si ce n'est ni une variable d'écart, ni une constante alors c'est une variable principale et elle est stocké avant les deux autres types
                        Console.WriteLine("Multiplicateur de la variable principale " + (colonne + 1) + " pour la sous-contrainte " + (ligne + 1) + " :");
                        tabSousContraintes[ligne, colonne] = int.Parse(Console.ReadLine());
                    }
                }
            }


            // "Pause écran"
            Console.ReadLine();
        }
    }
}
