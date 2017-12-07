﻿using System;
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
            double[] tabValeurPrincipal = new double[nbValPrincipal];
            // Pour affichage Z
            string valZ = "Z = ";
            ////////////////////// On récup valeur principales //////////////////////
            for (int i = 0; i < nbValPrincipal; i++)
            {
                Console.WriteLine("Multiplicateur de la variable principale " + (i + 1) + " :");
                tabValeurPrincipal[i] = double.Parse(Console.ReadLine());

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

            double[] VHB = new double[nbValPrincipal]; //la VHB
            double[] VDB = new double[nbValEcart] ; // la VDB
            //on remplis la VDB avec les variables d'écarts
            int countVDB = 0;
            for (int o = nbValPrincipal; o < nbValEcart; o++)
            {
                VHB[countVDB] = o;
                countVDB++;
            }

            //on remplis la VHB avec les variables principales
            int countVHB = 0;
            for (int o = 0; o < nbValPrincipal; o++)
            {
                VHB[countVHB] = o;
                countVHB++;
            }

            // Ici on fait "nbValPrincipal+2" car on stocke dans le tableau les variables principales + la variable d'écart + la constante
            double[,] tabSousContraintes = new double[nbValEcart, nbValPrincipal+2];
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
                        tabSousContraintes[ligne, colonne] = double.Parse(Console.ReadLine());
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
                        tabSousContraintes[ligne, colonne] = double.Parse(Console.ReadLine());
                    }
                }
            }

            //début du while (a faire)
            //condition d'arrét : avoir tout les coefs négatifs

            //on cherche la valeur entrante
            double vEntrante = 0;
            int numeroVEntrante = 0;
            for (int i = 0; i < tabValeurPrincipal.Length; i++)
            {
                if(tabValeurPrincipal[i] > vEntrante)
                {
                    vEntrante = tabValeurPrincipal[i];
                    numeroVEntrante = i;
                }
            }
            Console.WriteLine("valeur de variable entrante :" + vEntrante + ", numéro v entrante : "+ numeroVEntrante);//virer de la VHB

            double variableSortante = tabSousContraintes[0, nbValPrincipal + 1] / tabSousContraintes[0, numeroVEntrante];
            int numeroEquationSelectionne = 0;
            for (int i = 0; i < tabSousContraintes.GetUpperBound(0)+1; i++)
            {
                //on calcule ratio
                double ratio = tabSousContraintes[i,nbValPrincipal + 1] / tabSousContraintes[i, numeroVEntrante];
                if(ratio < variableSortante && ratio >0)
                {
                    variableSortante = ratio;
                    numeroEquationSelectionne = i;
                }
                numeroEquationSelectionne++;
                // MAJ de la vs avec la VDB
            }
            Console.WriteLine("valeur de variable sortante : "+ variableSortante);



                // "Pause écran"
                Console.ReadLine();
        }
    }
}
