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
            double[] tabValeurPrincipal = new double[nbValPrincipal + 2]; // +2 pour avoir de la marge car il va y avoir des rajouts plus tard
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

            ////////////////////// On calcul la variable entrante //////////////////////
            // Début du while (a faire)
            // Condition d'arrêt : avoir tout les coefs négatifs

            // On cherche la valeur entrante
            double variableEntrante = 0;
            // La variable "numeroVariableEntrante" permet d'obtenir le numéro associé à la variable, ex : x1
            int numeroVariableEntrante = 0;
            for (int i = 0; i < nbValPrincipal; i++) // on ne peut pas prendre le length du tableau, il n'est pas toujours valable
            {
                if(tabValeurPrincipal[i] > variableEntrante)
                {
                    variableEntrante = tabValeurPrincipal[i];
                    
                    numeroVariableEntrante = i;
                }
            }
            // Comme i commence par 0 et non 1, on doit lui ajouter 1
            Console.WriteLine("Valeur de variable entrante : " + variableEntrante + ", la variable entrante est : x" + (numeroVariableEntrante + 1));
            // ToDo : virer de la VHB ?

            ////////////////////// On calcul la variable sortante //////////////////////
            double variableSortante = tabSousContraintes[0, nbValPrincipal + 1] / tabSousContraintes[0, numeroVariableEntrante];
            int numeroEquationSelectionne = 0;
            int counterNombreCoefs = 0;
            int counterNombreCoefsNegatif = 0;
            bool stopIteration = false;
            int counterIteration = 0;
            for (int i = 0; i < tabSousContraintes.GetUpperBound(0)+1; i++)
            {
                // Il s'agit coefficient qui permet de savoir si on continue ou non
                double coef = tabSousContraintes[i,nbValPrincipal + 1] / tabSousContraintes[i, numeroVariableEntrante];
                counterNombreCoefs++;
                if(coef < variableSortante && coef > 0)
                {
                    variableSortante = coef;
                    numeroEquationSelectionne = i;
                }
                if (coef < 0)
                {
                    counterNombreCoefsNegatif++;
                }
                if(counterNombreCoefs == counterNombreCoefsNegatif)
                {
                    stopIteration = true;
                }
                // ToDo : MAJ de la vs avec la VDB ?
            }
            // Comme l'index du numéro de l'équation selectionné commence à 0, on doit lui ajouter 1 pour qu'il s'affiche correctement
            Console.WriteLine("Valeur de variable sortante : " + variableSortante + ", numéro de l'équation selectionnée : " + (numeroEquationSelectionne + 1));

            
            /*while(!stopIteration)
            {*/
                ////////////////////// On calcul l'équation d'échange //////////////////////
                // Création de l'équation d'échange
                double[] equationEchange = new double[nbValPrincipal + 2];
                counterIteration++;
                // La variable sortante est au 1er rang du tableau
                equationEchange[0] = numeroVariableEntrante;
                for (int i = 1; i < nbValPrincipal + 2; i++)
                {
                    // On traite le cas où on pourrais avoir des bugs avec des valeurs négatives
                    if (i == 1 || i < nbValPrincipal + 1)
                    {
                        equationEchange[i] = -(tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, 0]);
                    }
                    else
                    {
                        // On ne soustrait pas la constante
                        equationEchange[i] = tabSousContraintes[numeroEquationSelectionne, i] / tabSousContraintes[numeroEquationSelectionne, 0];
                    }

                }
                afficheSimple(equationEchange, "Tableau sous contraintes d'équation d'échange");


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
                                for (int index = 1; index < nbValEcart + 1; index++)
                                {
                                    resultat = sousContraintesTempo[numeroVariableEntrante] * equationEchange[index];
                                    // Cas de la constante
                                    if (index == nbValEcart)
                                    {
                                        resultat = sousContraintesTempo[index] - resultat;
                                    }
                                    else
                                    {
                                        // On ne veut pas faire de calcul avec la nouvelle variable
                                        if (index != nbValEcart - 1)
                                        {
                                            resultat = resultat + sousContraintesTempo[index];
                                        }
                                    }
                                    // On met à jour une partie d'une sous contrainte
                                    tabSousContraintes[ligne, index] = resultat;
                                }                                
                                // Comme la variable "ligne" est déclaré à 0 et non à 1 pour parcourir le tableau, on doit ici lui ajouter 1
                                Console.WriteLine("Resultat equation n°" + (ligne + 1) + ", " + resultat);
                                // On remet à zéro le compteur pour les contraintes
                                compteurDecalageContraintes = 0;
                                // Remettre a zéro sousContraintesTempo ?

                            }
                        }
                    }
                }

            //mis a jour des sous équations avec l'équation d'échange
            tabSousContraintes[numeroEquationSelectionne, 0] = 1; //dans ce cas, la variable a toujours la valeur 1
            for (int colonne = 1; colonne < nbValEcart + 1; colonne++){
                tabSousContraintes[numeroEquationSelectionne, colonne] = equationEchange[colonne];
            }


            // TODO : Recalculer le nouveau Z --> c'est avec lui que tu vas avoir une nouvelle condition d'arrêt
            // calcul de Z (le maximum) de la fonction
            double multiplicateurZ = tabValeurPrincipal[numeroVariableEntrante];
            for (int index = 1; index < equationEchange.Length; index++)
            {
                /*
                 * ok, la ca devient foireux. 
                 * Ce tableau passe généralement de X variable à X +1 au premier passage (et peut encore augmenter plus tard)
                 * Donc, au tout début il est initalisé avec deux "case" de marge de plus
                 * C'est pour cela que l'on utilise pas sa taille 
                 */
                double resultat = multiplicateurZ * equationEchange[index ];// ok
                if (index == equationEchange.Length -1)
                {
                    tabValeurPrincipal[nbValPrincipal] = resultat;
                }
                else
                {
                    if(numeroVariableEntrante == 0)
                    { //faire la condition pour l'addition 
                        tabValeurPrincipal[index - 1] = resultat + tabValeurPrincipal[index - 1] ;
                    }
                    else
                    {
                        tabValeurPrincipal[index - 1] = resultat;
                    }
                }
            }

            //on calcule le nombre de variable dans le tableau
            int counterTabValeurPrincipal = 0;
            for (int index = 0; index < tabValeurPrincipal.Length; index++)
            {
                if(tabValeurPrincipal[index] != 0)
                {
                    counterTabValeurPrincipal++;
                }
            }

            //on met à jour le nombre de nbValPrincipal
            nbValPrincipal = counterTabValeurPrincipal;

                //} //fin du while


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
                Console.WriteLine("debug "+info + ", élément : " + i + ", " + tab[i]);
            }
            Console.WriteLine("\n");
        }
    }
}
