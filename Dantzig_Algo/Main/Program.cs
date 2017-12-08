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
            for (int i = nbValPrincipal; i < nbValEcart; i++)
            {
                VHB[countVDB] = i;
                countVDB++;
            }

            //on remplis la VHB avec les variables principales
            int countVHB = 0;
            for (int i = 0; i < nbValPrincipal; i++)
            {
                VHB[countVHB] = i;
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
            Console.WriteLine("valeur de variable entrante :" + vEntrante + ", numéro v entrante : "+ numeroVEntrante);
            // ToDo : virer de la VHB ?

            double variableSortante = tabSousContraintes[0, nbValPrincipal + 1] / tabSousContraintes[0, numeroVEntrante];
            int numeroEquationSelectionne = 0;
            for (int i = 0; i < tabSousContraintes.GetUpperBound(0)+1; i++)
            {
                double coef = tabSousContraintes[i,nbValPrincipal + 1] / tabSousContraintes[i, numeroVEntrante]; //le coef qui permet de savoir si l'on continue
                if(coef < variableSortante && coef > 0)
                {
                    variableSortante = coef;
                    numeroEquationSelectionne = i;
                }
                // ToDo : MAJ de la vs avec la VDB ?
            }
            Console.WriteLine("valeur de variable sortante : "+ variableSortante+ " num equation selectionnée : "+ numeroEquationSelectionne);

            //création de l'équation d'échange
            double[] equationEchange = new double[nbValPrincipal+2];
            equationEchange[0] = numeroVEntrante;//la variable sortante est au 1e rang du tableau

            for (int i = 1; i < nbValPrincipal +2; i++)
            {
                //possibilité d'avoir des bugs avec des valeurs négatives
               if(i == 1 || i < nbValPrincipal + 1)
                {
                    equationEchange[i] = -(tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, 0]);
                }
                else
                { //on ne soustrait pas la constante
                    equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, 0];
                }
                
            }
            afficheSimple(equationEchange, "tableau sous contraintes");
            

            //calcul des nouvelles sous-contraintes
            double[] sousContraintesTempo = new double[nbValPrincipal + 2]; // les sous contraintes sont stockés pour simplifications
            for (int ligne = 0; ligne < tabSousContraintes.GetUpperBound(0) ; ligne++)
            {
                //on ne traite pas cette équation, elle est déja faite
                if(ligne != numeroEquationSelectionne)
                {
                    int compteurDecalageContraintes = 0;
                    for (int colonne = 0; colonne < nbValEcart + 1; colonne++)
                    {
                        //on récupére les variable d'une équation dans un tableau
                        sousContraintesTempo[colonne] = tabSousContraintes[ligne, colonne];
                        compteurDecalageContraintes++;
                        //si on remplis la conditons, on a l'ensemble d'une sous contraintes, donc le traitement peut commencer
                        if(compteurDecalageContraintes == nbValEcart + 1)
                        {
                            for (int index = 1; index < nbValEcart + 1; index++)
                            {
                                double resultat = sousContraintesTempo[numeroVEntrante] * equationEchange[index];//ok
                                //cas de la constante
                                if(index == nbValEcart)
                                {
                                    resultat = sousContraintesTempo[index] - resultat ;
                                }
                                else
                                {
                                    if(index != nbValEcart - 1)
                                    {
                                        resultat = resultat + sousContraintesTempo[index];
                                    }
                                }
                                Console.WriteLine("resultat equation n° "+ ligne + " , "+resultat);
                            }
                            compteurDecalageContraintes = 0; // on remet à zéro le compteur pour les contraintes
                            //remettre a zéro sousContraintesTempo ?
                        }
                    }
                }
            }




                // "Pause écran"
                Console.ReadLine();
        }


        static void afficheSimple(double[] tab, string info)
        {
            Console.WriteLine("\n");
            for (int i = 0; i < tab.Length; i++)
            {
                Console.WriteLine(info+ " , élément : " + i + " , " + tab[i]);
            }
            Console.WriteLine("\n");
        }
    }
}
