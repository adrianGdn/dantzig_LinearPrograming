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
            //Bug();

            Console.WriteLine("Nombre de variables principales :");
            int nbValPrincipal = int.Parse(Console.ReadLine());
            double[] tabValeurPrincipal = new double[nbValPrincipal + 1]; // +2 pour avoir de la marge car il va y avoir des rajouts plus tard
            // Pour affichage Z
            string valZ = "Z = ";
            ////////////////////// On récup valeur principales //////////////////////
            for (int i = 0; i < nbValPrincipal; i++)
            {
                Console.WriteLine("Multiplicateur de la variable principale " + (i + 1) + " :");
                tabValeurPrincipal[i] = double.Parse(Console.ReadLine());

                if (i + 1 == nbValPrincipal)
                {
                    // Permet d'afficher correctement Z
                    valZ += "(" + tabValeurPrincipal[i] + "x" + (i + 1) + ") ";
                }
                else valZ += "(" + tabValeurPrincipal[i] + "x" + (i + 1) + ") + ";
            }
            // Affichage de Z et sauts de lignes
            Console.WriteLine("\n\n" + valZ + "\n\n");

            Console.WriteLine("Nombre de variables d'écarts :");
            int nbValEcart = int.Parse(Console.ReadLine());
            Console.WriteLine("\n");

            bool stopIteration = false;
            int counterIteration = 0;
            double[] VHB = new double[nbValPrincipal]; //la VHB
            double[] VDB = new double[nbValEcart]; // la VDB

            //on remplis la VHB avec les variables principales
            int countVHB = 0;
            for (int i = 0; i < nbValPrincipal; i++)
            {
                VHB[countVHB] = i;
                countVHB++;
            }

            //on remplis la VDB avec les variables d'écarts
            int countVDB = countVHB;
            for (int i = 0; i <nbValEcart; i++)
            {
                VDB[i] = i + nbValPrincipal;
            } 

            // Ici on fait "nbValPrincipal+2" car on stocke dans le tableau les variables principales + la variable d'écart + la constante
            double[,] tabSousContraintes = new double[nbValEcart, nbValPrincipal + 2];
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


            // Début du while 
            // Condition d'arrêt : avoir tout les coefs de Z négatifs

            while (!stopIteration)
            {
                ////////////////////// On calcul la variable entrante //////////////////////
                // On cherche la valeur entrante
                double variableEntrante = 0;
                // La variable "numeroVariableEntrante" permet d'obtenir le numéro associé à la variable, ex : x1
                int numeroVariableEntrante = 0;
                if (counterIteration == 0) // cas 1e itération
                {
                    for (int i = 0; i < nbValPrincipal; i++) // on ne peut pas prendre le length du tableau, il n'est pas toujours valable
                    {
                        if (tabValeurPrincipal[i] > variableEntrante)
                        {
                            variableEntrante = tabValeurPrincipal[i];

                            numeroVariableEntrante = i;
                        }
                    }
                }
                else // cas itération > 0
                {
                    for (int i = 0; i < nbValPrincipal - 1; i++) // on ne peut pas prendre le length du tableau, il n'est pas toujours valable
                    {
                        if (tabValeurPrincipal[i] > variableEntrante)
                        {
                            variableEntrante = tabValeurPrincipal[i];

                            numeroVariableEntrante = i;
                        }
                    }
                }
                // Comme i commence par 0 et non 1, on doit lui ajouter 1
                if(counterIteration == 0)
                {
                    Console.WriteLine("Valeur de variable entrante : " + variableEntrante + ", la variable entrante est : x" + (numeroVariableEntrante + 1));
                }

                ////////////////////// On calcul la variable sortante //////////////////////
                double variableSortante = 0;
                int numeroEquationSelectionne = 0;
                int counterNombreCoefs = 0;
                for (int i = 0; i < tabSousContraintes.GetUpperBound(0) + 1; i++)
                {
                    double coef = 0;
                    if (counterIteration == 0)
                    {
                        coef = tabSousContraintes[i, numeroVariableEntrante] / tabSousContraintes[i, nbValPrincipal + 1];
                    }
                    else
                    {
                        coef = tabSousContraintes[i, numeroVariableEntrante] / tabSousContraintes[i, nbValPrincipal];
                    }

                    counterNombreCoefs++;
                    if (coef > variableSortante && coef > 0)
                    {
                        variableSortante = coef;
                        numeroEquationSelectionne = i;
                    }
                }
                // Comme l'index du numéro de l'équation selectionné commence à 0, on doit lui ajouter 1 pour qu'il s'affiche correctement
                Console.WriteLine("Valeur de variable sortante : " + variableSortante + ", numéro de l'équation selectionnée : " + (numeroEquationSelectionne + 1));
                //retourChariot();

                ////////////////////// On calcul l'équation d'échange //////////////////////
                // Création de l'équation d'échange
                double[] equationEchange = new double[nbValPrincipal + 2];

                
                // cas 1e itération
                if (counterIteration == 0)
                {
                    for (int i = 1; i < nbValPrincipal + 2; i++)
                    {
                        // On traite le cas où on pourrais avoir des bugs avec des valeurs négatives
                        if (i == 1 || i < nbValPrincipal + 1)
                        {
                            equationEchange[i] = -(tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante]);
                        }
                        else
                        {
                            // On ne soustrait pas la constante
                            equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante];
                        }

                    }
                }
                else
                { // cas on itération > 0
                    for (int i = 1; i < nbValPrincipal + 1; i++)
                    {
                        equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante + 1];
                    }
                }
                // La variable sortante est au 1er rang du tableau
                equationEchange[0] = 1;

                afficheEquationEchange(equationEchange, numeroVariableEntrante);


                // Calcul des nouvelles sous-contraintes
                // Les sous contraintes sont stockés pour simplifications
                double[] sousContraintesTempo = new double[nbValPrincipal + 2];
                for (int ligne = 0; ligne < tabSousContraintes.GetUpperBound(0); ligne++)
                {
                    // On ne traite pas cette équation car elle est déja résolu
                    if (ligne != numeroEquationSelectionne)
                    {
                        int compteurDecalageContraintes = 0;
                        for (int colonne = 0; colonne < nbValEcart + 1; colonne++)
                        {
                            // On récupère les variable d'une équation dans un tableau
                            sousContraintesTempo[colonne] = tabSousContraintes[ligne, colonne];
                            compteurDecalageContraintes++;
                            // Si on remplit la conditon, on a l'ensemble de l'une des sous-contraintes, donc le traitement peut commencer
                            if (compteurDecalageContraintes == nbValEcart + 1)
                            {
                                double resultat = 0;
                                for (int index = 0; index < nbValEcart; index++)
                                {
                                    resultat = sousContraintesTempo[numeroVariableEntrante] * equationEchange[index + 1];
                                    // Cas de la constante
                                    if (index == nbValEcart)
                                    {
                                        resultat = sousContraintesTempo[index] - resultat;
                                        // On met à jour une partie d'une sous contrainte
                                        tabSousContraintes[ligne, index] = resultat;
                                    }
                                    else
                                    {
                                        if (index == nbValEcart - 2)
                                        {
                                            resultat = resultat + sousContraintesTempo[index];
                                            tabSousContraintes[ligne, index] = resultat;
                                        }
                                        if (index == nbValEcart - 1)
                                        {
                                            resultat = sousContraintesTempo[index + 1] - resultat;
                                            tabSousContraintes[ligne, index + 1] = resultat;
                                        }
                                        else
                                        {
                                            //resultat = resultat + sousContraintesTempo[index];
                                            // On met à jour une partie d'une sous contrainte
                                            tabSousContraintes[ligne, index] = resultat;
                                        }
                                    }

                                }

                                // On remet à zéro le compteur pour les contraintes
                                compteurDecalageContraintes = 0;
                                // Remettre a zéro sousContraintesTempo ?

                            }
                        }
                    }
                }

                //mis a jour des sous équations avec l'équation d'échange
                tabSousContraintes[numeroEquationSelectionne, 0] = 1; //dans ce cas, la variable a toujours la valeur 1
                for (int colonne = 1; colonne < nbValEcart + 1; colonne++)
                {
                    tabSousContraintes[numeroEquationSelectionne, colonne] = equationEchange[colonne];
                }

                afficheSousContraintes(tabSousContraintes, nbValEcart);

                // calcul de Z (le maximum) de la fonction
                double multiplicateurZ = tabValeurPrincipal[numeroVariableEntrante];
                double valeurZOpti = 0;
                int counterCoefNeg = 0;
                for (int index = 1; index < equationEchange.Length; index++)
                {
                   
                    double resultat = multiplicateurZ * equationEchange[index];// ok
                    if (index == equationEchange.Length - 1)
                    {
                        //tabValeurPrincipal[nbValPrincipal] = resultat;
                        tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index - 1];
                    }
                    else
                    {
                        if (numeroVariableEntrante == 0 && counterIteration == 0)
                        {
                            tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index ];
                        }
                        else
                        {
                            tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index - 1];
                        }
                    }
                    if(tabValeurPrincipal[index -1] != 0)
                    {
                        valeurZOpti = tabValeurPrincipal[index -1];
                    }
                    if (tabValeurPrincipal[index - 1] < 0)
                    {
                        counterCoefNeg++;
                    }
                }

                //on calcule et affiche le nombre de variable dans le tableau
                string afficheVarPrincipal = " valeurs du tableau principal : ";
                int counterTabValeurPrincipal = 0;
                for (int index = 0; index < tabValeurPrincipal.Length; index++)
                {
                    if (tabValeurPrincipal[index] != 0)
                    {
                        counterTabValeurPrincipal++;
                        if(index == tabValeurPrincipal.Length -1 )
                        {
                            afficheVarPrincipal += " = " + tabValeurPrincipal[index];
                        }
                        else
                        {
                            afficheVarPrincipal += " , " + tabValeurPrincipal[index];
                        }
                    }
                }

                Console.WriteLine(afficheVarPrincipal);

                //on met à jour le nombre de nbValPrincipal
                nbValPrincipal = counterTabValeurPrincipal;
                counterIteration++;
                Console.WriteLine("Z optimisée : " + valeurZOpti + " pour l\'itération N° " + counterIteration);
                retourChariot();

                if (counterCoefNeg >= nbValPrincipal -1 || counterIteration > 10)
                {
                    stopIteration = true;
                }
            } //fin du while


            // "Pause écran"
            Console.ReadLine();
        }

        // Est-ce vraiment une fonction qui sert à quelque chose à part rendre l'IHM moins lisible ?
        // sert pour le debug
        static void afficheSimple(double[] tab, string info)
        {
            Console.WriteLine("\n");
            for (int i = 0; i < tab.Length; i++)
            {
                Console.WriteLine("debug " + info + ", élément : " + i + ", " + tab[i]);
            }
            Console.WriteLine("\n");
        }

        static void afficheEquationEchange(double[] tab, int numVarEntrante)
        {
            Console.WriteLine("\n");
            string affiche = "équation d'échange : ";
            for (int i = 0; i < tab.Length; i++)
            {
                if(i == 0)
                {
                    affiche += " x"+ (numVarEntrante+1) + " = ";
                }
                else
                {
                    if(i == tab.Length -1)
                    {
                        affiche += " + " + tab[i] ;
                    }
                    else
                    {
                        affiche += " + " + tab[i]+ "x";
                    }
                }
            }
            Console.WriteLine(affiche);
            Console.WriteLine("\n");
        }

        static void afficheSousContraintes(double[,] tab, int nbValEcart)
        {
            Console.WriteLine("\n");
            string affiche = "";
            for (int ligne = 0; ligne < tab.GetUpperBound(0); ligne++)
            {
                affiche = "sous équation : ";
                for (int i = 1; i < nbValEcart + 1; i++)
                {
                    if (i == 0)
                    {
                        affiche += " x = ";
                    }
                    else
                    {
                        if (i == tab.Length - 1)
                        {
                            affiche += " + " + tab[ligne,i];
                        }
                        else
                        {
                            affiche += " + " + tab[ligne, i] + "x";
                        }
                    }
                }
            }
            Console.WriteLine(affiche);
            Console.WriteLine("\n");
        }

        static void retourChariot()
        {
            Console.WriteLine("\n");
        }

        static void Bug()
        {
            Console.WriteLine("Nombre de variables principales :");
            int nbValPrincipal = int.Parse(Console.ReadLine());
            double[] tabValeurPrincipal = new double[nbValPrincipal + 2]; // +2 pour avoir de la marge car il va y avoir des rajouts plus tard
            // Pour affichage Z
            string valZ = "Z = ";
            ////////////////////// On récup valeur principales //////////////////////
            for (int i = 0; i < nbValPrincipal; i++)
            {
                Console.WriteLine("Multiplicateur de la variable principale " + (i + 1) + " :");
                tabValeurPrincipal[i] = double.Parse(Console.ReadLine());

                if (i + 1 == nbValPrincipal)
                {
                    // Permet d'afficher correctement Z
                    valZ += "(" + tabValeurPrincipal[i] + "x" + (i + 1) + ") ";
                }
                else valZ += "(" + tabValeurPrincipal[i] + "x" + (i + 1) + ") + ";
            }
            // Affichage de Z et sauts de lignes
            Console.WriteLine("\n\n" + valZ + "\n\n");

            Console.WriteLine("Nombre de variables d'écarts :");
            int nbValEcart = int.Parse(Console.ReadLine());
            Console.WriteLine("\n");

            bool stopIteration = false;
            int counterIteration = 0;
            double[] VHB = new double[nbValPrincipal]; //la VHB
            double[] VDB = new double[nbValEcart]; // la VDB
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
            double[,] tabSousContraintes = new double[nbValEcart, nbValPrincipal + 2];
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


            // Début du while 
            // Condition d'arrêt : avoir tout les coefs de Z négatifs

            while (!stopIteration)
            {
                ////////////////////// On calcul la variable entrante //////////////////////
                // On cherche la valeur entrante
                double variableEntrante = 0;
                // La variable "numeroVariableEntrante" permet d'obtenir le numéro associé à la variable, ex : x1
                int numeroVariableEntrante = 0;
                if (counterIteration == 0) // cas 1e itération
                {
                    for (int i = 0; i < nbValPrincipal; i++) // on ne peut pas prendre le length du tableau, il n'est pas toujours valable
                    {
                        if (tabValeurPrincipal[i] > variableEntrante)
                        {
                            variableEntrante = tabValeurPrincipal[i];

                            numeroVariableEntrante = i;
                        }
                    }
                }
                else // cas itération > 0
                {
                    for (int i = 0; i < nbValPrincipal - 1; i++) // on ne peut pas prendre le length du tableau, il n'est pas toujours valable
                    {
                        if (tabValeurPrincipal[i] > variableEntrante)
                        {
                            variableEntrante = tabValeurPrincipal[i];

                            numeroVariableEntrante = i;
                        }
                    }
                }
                // Comme i commence par 0 et non 1, on doit lui ajouter 1
                Console.WriteLine("Valeur de variable entrante : " + variableEntrante + ", la variable entrante est : x" + (numeroVariableEntrante + 1));

                ////////////////////// On calcul la variable sortante //////////////////////
                double variableSortante = 0;
                int numeroEquationSelectionne = 0;
                int counterNombreCoefs = 0;
                for (int i = 0; i < tabSousContraintes.GetUpperBound(0) + 1; i++)
                {
                    double coef = 0;
                    if (counterIteration == 0)
                    {
                        coef = tabSousContraintes[i, numeroVariableEntrante] / tabSousContraintes[i, nbValPrincipal + 1];
                    }
                    else
                    {
                        coef = tabSousContraintes[i, numeroVariableEntrante] / tabSousContraintes[i, nbValPrincipal];
                    }

                    counterNombreCoefs++;
                    if (coef > variableSortante && coef > 0)
                    {
                        variableSortante = coef;
                        numeroEquationSelectionne = i;
                    }
                }
                // Comme l'index du numéro de l'équation selectionné commence à 0, on doit lui ajouter 1 pour qu'il s'affiche correctement
                Console.WriteLine("Valeur de variable sortante : " + variableSortante + ", numéro de l'équation selectionnée : " + (numeroEquationSelectionne + 1));
                //retourChariot();

                ////////////////////// On calcul l'équation d'échange //////////////////////
                // Création de l'équation d'échange
                double[] equationEchange = new double[nbValPrincipal + 2];


                // cas 1e itération
                if (counterIteration == 0)
                {
                    for (int i = 1; i < nbValPrincipal + 2; i++)
                    {
                        // On traite le cas où on pourrais avoir des bugs avec des valeurs négatives
                        if (i == 1 || i < nbValPrincipal + 1)
                        {
                            equationEchange[i] = -(tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante]);
                        }
                        else
                        {
                            // On ne soustrait pas la constante
                            equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante];
                        }

                    }
                }
                else
                { // cas on itération > 0
                    for (int i = 1; i < nbValPrincipal + 1; i++)
                    {
                        // On traite le cas où on pourrais avoir des bugs avec des valeurs négatives
                        if (i == 1 || i < nbValPrincipal + 1)
                        {
                            equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante + 1];
                        }
                        else
                        {
                            // On ne soustrait pas la constante
                            equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, numeroVariableEntrante + 1];
                        }
                    }
                }
                // La variable sortante est au 1er rang du tableau
                equationEchange[0] = 1;

                afficheEquationEchange(equationEchange, numeroVariableEntrante);


                // Calcul des nouvelles sous-contraintes
                // Les sous contraintes sont stockés pour simplifications
                double[] sousContraintesTempo = new double[nbValPrincipal + 2];
                for (int ligne = 0; ligne < tabSousContraintes.GetUpperBound(0); ligne++)
                {
                    // On ne traite pas cette équation car elle est déja résolu
                    if (ligne != numeroEquationSelectionne)
                    {
                        int compteurDecalageContraintes = 0;
                        for (int colonne = 0; colonne < nbValEcart + 1; colonne++)
                        {
                            // On récupère les variable d'une équation dans un tableau
                            sousContraintesTempo[colonne] = tabSousContraintes[ligne, colonne];
                            compteurDecalageContraintes++;
                            // Si on remplit la conditon, on a l'ensemble de l'une des sous-contraintes, donc le traitement peut commencer
                            if (compteurDecalageContraintes == nbValEcart + 1)
                            {
                                double resultat = 0;
                                for (int index = 0; index < nbValEcart; index++)
                                {
                                    resultat = sousContraintesTempo[numeroVariableEntrante] * equationEchange[index + 1];
                                    // Cas de la constante
                                    if (index == nbValEcart)
                                    {
                                        resultat = sousContraintesTempo[index] - resultat;
                                        // On met à jour une partie d'une sous contrainte
                                        tabSousContraintes[ligne, index] = resultat;
                                    }
                                    else
                                    {
                                        if (index == nbValEcart - 2)
                                        {
                                            resultat = resultat + sousContraintesTempo[index];
                                            tabSousContraintes[ligne, index] = resultat;
                                        }
                                        if (index == nbValEcart - 1)
                                        {
                                            resultat = sousContraintesTempo[index + 1] - resultat;
                                            tabSousContraintes[ligne, index + 1] = resultat;
                                        }
                                        else
                                        {
                                            //resultat = resultat + sousContraintesTempo[index];
                                            // On met à jour une partie d'une sous contrainte
                                            tabSousContraintes[ligne, index] = resultat;
                                        }
                                    }

                                }

                                // On remet à zéro le compteur pour les contraintes
                                compteurDecalageContraintes = 0;
                                // Remettre a zéro sousContraintesTempo ?

                            }
                        }
                    }
                }

                //mis a jour des sous équations avec l'équation d'échange
                tabSousContraintes[numeroEquationSelectionne, 0] = 1; //dans ce cas, la variable a toujours la valeur 1
                for (int colonne = 1; colonne < nbValEcart + 1; colonne++)
                {
                    tabSousContraintes[numeroEquationSelectionne, colonne] = equationEchange[colonne];
                }
                afficheSousContraintes(tabSousContraintes, nbValEcart);

                // calcul de Z (le maximum) de la fonction
                double multiplicateurZ = tabValeurPrincipal[numeroVariableEntrante];
                double valeurZOpti = 0;
                int counterCoefNeg = 0;
                for (int index = 1; index < equationEchange.Length; index++)
                {
                    double resultat = multiplicateurZ * equationEchange[index];// ok
                    if (index == equationEchange.Length - 1)
                    {
                        //tabValeurPrincipal[nbValPrincipal] = resultat;
                        tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index - 1];
                    }
                    else
                    {
                        if (numeroVariableEntrante == 0)
                        {
                            tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index - 1];
                        }
                        else
                        {
                            tabValeurPrincipal[index - 1] = resultat;
                        }
                    }
                    if (tabValeurPrincipal[index - 1] != 0)
                    {
                        valeurZOpti = tabValeurPrincipal[index - 1];
                    }
                    if (tabValeurPrincipal[index - 1] < 0)
                    {
                        counterCoefNeg++;
                    }
                }

                //on calcule le nombre de variable dans le tableau
                int counterTabValeurPrincipal = 0;
                for (int index = 0; index < tabValeurPrincipal.Length; index++)
                {
                    if (tabValeurPrincipal[index] != 0)
                    {
                        counterTabValeurPrincipal++;
                    }
                }

                //on met à jour le nombre de nbValPrincipal
                nbValPrincipal = counterTabValeurPrincipal;
                counterIteration++;
                Console.WriteLine("Z optimisée : " + valeurZOpti + " pour l\'itération N° " + counterIteration);
                retourChariot();

                if (counterCoefNeg >= nbValPrincipal - 1 || counterIteration > 10)
                {
                    stopIteration = true;
                }
            } //fin du while


            // "Pause écran"
            Console.ReadLine();
        }
    }
}

