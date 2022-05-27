using System;

namespace F1RacingRomeoRioli
{

    class Program
    {
        static void Main(string[] args)
        {
            int laps, position, nbrPlayer,colonne;
            string returnDirect, place,nbr;
            double pneu = 1, speed = 0.0;

            nbrPlayer = 0;

            string[,] matrice = new string[11, 13];
            int[] tabNum = new int[9];
            int[] tabPosition = new int[9];


            //intro du jeu 
            Console.WriteLine("Bienvenu dans F1 racing");
            Console.WriteLine(@"
  _    _             /'_'_/.-''/                             _______
  \`../ |o_..__     / /__   / /  -= WORLD CHAMPIONSHIP =-   _\=.o.=/_
`.,(_)______(_).>  / ___/  / /                             |_|_____|_|
~~~~~~~~~~~~~~~~~~/_/~~~~~/_/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
");
            Console.WriteLine("Appuyer sur <enter> pour continuer ");
            Console.ReadLine();

            do
            {
                Console.WriteLine("Choisisez le numero de votre formule 1, choix entre 1 et 10 ");
                nbr = Console.ReadLine();


            } while (!int.TryParse(nbr, out nbrPlayer));
          
            Remplirmatrice(ref nbrPlayer, ref matrice, ref tabNum);

            //Calcul de la position de départ
            PlaceDepart(out place, out int rndPlace);
            Console.WriteLine($"Apres qualification, vous avez été assigné {place} sur la piste de départ");
            Console.ReadLine();

            //Assignation de la position des voitures 
            qualif(nbrPlayer, rndPlace, ref tabNum, ref tabPosition, ref matrice);

            Affichermatrice(matrice);


            //Loop chaque tour
            for (int i = 1; i <= 8; i++)
            {
                //calcul vitesse par rapport à l'usure des pneus
                calculVitesse(ref pneu, ref speed);

                if (i == 1)
                    Console.WriteLine($"Tour {i} c'est parti !!!!!!");
                else
                    Console.WriteLine($"Tour {i}");

                if (rndPlace == 1)
                {
                    Console.WriteLine("Vous êtes actuellement en tête");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Voulez-vous doubler quelqu'un Y/N ?");
                    string retour = Console.ReadLine().ToUpper();
                    if (retour == "Y")
                    {
                        bool doublage = true;
                        returnDirect = "";
                        randomDoublage(out int nbrRandom);
                        Doublage(ref pneu, speed, nbrRandom, out returnDirect, out doublage);
                        Console.WriteLine(returnDirect);
                        if (doublage)
                        {
                            //Le joueur gagne une place
                            tabPosition[rndPlace - 1] = tabPosition[rndPlace - 2];
                            tabPosition[rndPlace - 2] = nbrPlayer;
                            rndPlace--;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Voulez-vous faire un pitstop ? (y/n)");
                        retour = Console.ReadLine().ToUpper();
                        if (retour == "Y")
                        {
                            //Le joueur perd une place
                            if (rndPlace != 9)
                            {
                                tabPosition[rndPlace - 1] = tabPosition[rndPlace];
                                tabPosition[rndPlace] = nbrPlayer;
                                rndPlace++;
                                //Reset la valeur du pneu
                                pneu = 1;
                                Console.WriteLine("Votre pitstop vous fait perdre une place ;p");
                            }
                        }
                        else
                            Console.WriteLine("Vous rester à votre place actuelle");
                    }

                }

                //Doublage des bots
                randomDoublageBot(rndPlace, out int nbrBot1, out int nbrBot2);
                doublageBot(nbrPlayer, nbrBot1, nbrBot2, ref tabPosition, ref rndPlace);

                //Affichage position actuelle du joueur
                Console.WriteLine($"Votre position actuelle : {rndPlace}");

                //Affiche l'usure des pneus               
                Console.WriteLine($"Usure des pneus : {pneu} (Taux de chance de doubler {Math.Round(25 + 70 / pneu)}%)");
                Console.WriteLine();
                //augmentation de l'usure des pneus
                pneu += 0.5;

                //Nouvelle affichage de la matrice
                colonne = i + 2;
                ModifierMatrice(ref tabPosition, ref matrice, ref colonne);
                Affichermatrice(matrice);

                Console.WriteLine("Appuyer sur <enter> pour continuer ");
                Console.ReadLine();
            }

            Console.WriteLine("Fin de la course !!!!");
            Console.WriteLine("");

            //podium
            colonne = 11;
            ModifierMatrice(ref tabPosition, ref matrice, ref colonne);
            Affichermatrice(matrice);

            Console.WriteLine("Résultat : ");
            if (rndPlace == 1)
            {
                Console.WriteLine("Félicitation, vous avez gagné le Grand Prix !");
            }
            else
            {
                Console.WriteLine($"Dommage vous avez terminé en {rndPlace}ème position");
            }
            Console.WriteLine("");
            Console.WriteLine("PODIUM : ");
            Console.WriteLine("[1] " + $"{tabPosition[0]}");
            Console.WriteLine("[2] " + $"{tabPosition[1]}");
            Console.WriteLine("[3] " + $"{tabPosition[2]}");
            Console.ReadLine();

        }


        //Qualification
        public static void qualif(int nbrPlayer, int rndPlace, ref int[] tabNum, ref int[] tabPosition, ref string[,] matrice)
        {
            for (int i = 0; i < tabPosition.Length; i++)
            {
                tabPosition[i] = 0;
            }
            tabPosition[rndPlace - 1] = nbrPlayer;
            Random rnd = new Random();
            int nbr;
            for (int i = 0; i < tabNum.Length - 1; i++)
            {
                do
                {
                    nbr = rnd.Next(0, 9);
                } while (tabPosition[nbr] != 0);
                tabPosition[nbr] = tabNum[i + 1];
            }
            int colonne = 2;
            ModifierMatrice(ref tabPosition, ref matrice, ref colonne);

        }

        //Modification matrice
        public static void ModifierMatrice(ref int[] tabPosition, ref string[,] matrice, ref int colonne)
        {
            for (int i = 0; i < tabPosition.Length; i++)
            {
                matrice[i + 1, colonne] = $"{tabPosition[i]}";
            }
        }

        //Affichage matrice
        public static void Affichermatrice(string[,] matrice)
        {
            for (int i = 0; i < matrice.GetLength(0); i++)
            {

                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    Console.Write(matrice[i, j] + "\t");
                }
                Console.WriteLine("\n");
            }
        }

        //Matrice de départ
        public static void Remplirmatrice(ref int nbrPlayer, ref string[,] matrice, ref int[] tabNum)
        {

            for (int i = 1; i < 10; i++)
            {
                matrice[i, 0] = "[" + $"{i}" + "]";
            }

            matrice[0, 1] = "Num";
            matrice[0, 2] = "Départ";
            matrice[0, 3] = "Tour 1";
            matrice[0, 4] = "Tour 2";
            matrice[0, 5] = "Tour 3";
            matrice[0, 6] = "Tour 4";
            matrice[0, 7] = "Tour 5";
            matrice[0, 8] = "Tour 6";
            matrice[0, 9] = "Tour 7";
            matrice[0, 10] = "Tour 8";
            matrice[0, 11] = "Podium";

            tabNum[0] = nbrPlayer;
            tabNum[1] = 12;
            tabNum[2] = 56;
            tabNum[3] = 89;
            tabNum[4] = 25;
            tabNum[5] = 72;
            tabNum[6] = 34;
            tabNum[7] = 18;
            tabNum[8] = 24;

            matrice[1, 1] = $"{tabNum[0]}";
            matrice[2, 1] = $"{tabNum[1]}";
            matrice[3, 1] = $"{tabNum[2]}";
            matrice[4, 1] = $"{tabNum[3]}";
            matrice[5, 1] = $"{tabNum[4]}";
            matrice[6, 1] = $"{tabNum[5]}";
            matrice[7, 1] = $"{tabNum[6]}";
            matrice[8, 1] = $"{tabNum[7]}";
            matrice[9, 1] = $"{tabNum[8]}";

        }

        //Position de départ du joueur
        public static void PlaceDepart(out string place, out int rndPlace)
        {
            Random RanDepart = new Random();
            rndPlace = RanDepart.Next(1, 9);

            if (rndPlace == 1)
            {
                place = rndPlace + "er";
            }
            else
                place = rndPlace + "ème";
        }

        //Calcul de la vitesse
        public static void calculVitesse(ref double pneu, ref double speed)
        {
            speed = 70 / pneu;
        }

        //Dépassement du joueur
        public static void Doublage(ref double pneu, double speed, int nbrRandom, out string returnDirect, out bool doublage)
        {

            if ((25 + speed) > nbrRandom)
            {

                returnDirect = "Vous avez dépassé une voiture";
                doublage = true;

            }
            else
            {
                returnDirect = "Vous n'avez pas réussi le dépassement";
                doublage = false;
            }
        }

        //Random joueur
        public static void randomDoublage(out int nbrRandom)
        {
            Random rnd = new Random();
            nbrRandom = rnd.Next(1, 100);


        }

        //Random Bots
        public static void randomDoublageBot(int rndPlace, out int nbrBot1, out int nbrBot2)
        {
            Random rnd = new Random();
            do
            {
                nbrBot1 = rnd.Next(2, 5);
            } while (nbrBot1 == rndPlace);
            do
            {
                nbrBot2 = rnd.Next(5, 10);
            } while (nbrBot2 == rndPlace);

        }

        //Doublage des Bots
        public static void doublageBot(int nbrPlayer, int nbrBot1, int nbrBot2, ref int[] tabPosition, ref int rndPlace)
        {
            int dummy;
            dummy = tabPosition[nbrBot1 - 1];
            tabPosition[nbrBot1 - 1] = tabPosition[nbrBot1 - 2];
            tabPosition[nbrBot1 - 2] = dummy;
            Console.WriteLine($"{dummy} a dépassé {tabPosition[nbrBot1 - 1]}");
            //Si la voiture doublé est celle du joueur
            if (tabPosition[nbrBot1 - 1] == nbrPlayer)
            {
                //Le joueur perd une place
                rndPlace++;
                Console.WriteLine("Vous avez été dépassé");
            }

            dummy = tabPosition[nbrBot2 - 1];
            tabPosition[nbrBot2 - 1] = tabPosition[nbrBot2 - 2];
            tabPosition[nbrBot2 - 2] = dummy;
            Console.WriteLine($"{dummy} a dépassé {tabPosition[nbrBot2 - 1]}");
            //Si la voiture doublé est celle du joueur
            if (tabPosition[nbrBot2 - 1] == nbrPlayer)
            {
                //Le joueur perd une place
                rndPlace++;
                Console.WriteLine("Vous avez été dépassé");
            }

        }
    }
}
