using System;

namespace F1RacingRomeoRioli
{
    class Program
    {
        static void Main(string[] args)
        {
            int pneu, speed, laps, position, nbrPlayer;
            string returnDirect, place;



            //intro du jeu 
            Console.WriteLine("Bienvenu dans F1 racing");
            Console.WriteLine("Appuyer sur n'importe quelle touche pour continué ");
            Console.ReadLine();
            Console.WriteLine("Choisisez le numero de votre formule 1 ");
            nbrPlayer = Convert.ToInt32((Console.ReadLine()));
            PlaceDepart(out place);
            Console.WriteLine($"Vous avez été assignez {place} sur la piste de départ");

            

        }
        public static void matrice(){


            double[,] matrice = new double[4, 5];
            
            matrice[2, 2] = 5;
            matrice[3, 2] = 12;
            matrice[4, 2] = 56;
            matrice[5, 2] = 89;
            matrice[6, 2] = 25;
            matrice[7, 2] = 72;
            matrice[8, 2] = 45;
        }

        public static void PlaceDepart(out string place)
        {
            int rndPlace;
            Random RanDepart = new Random();
            rndPlace = RanDepart.Next(1, 7);
          
            if (rndPlace == 1)
            {
                place = rndPlace +"ier";
            }else
                place = rndPlace + "ieme";

        }
        public void pneuUsage(ref int pneu, int speed)
        {
            pneu = 1;

            speed = 70 / pneu;
        }
        public static void doublage(ref int pneu, int speed, int nbrRandom, string returnDirect)
        {
            if ((10 + speed)> nbrRandom)
            {

                returnDirect = "Vous avez dépacé la voiture";
                bool doublage = true;

            }
            else
            {
                returnDirect = "Vous n'avez pas réussit le dépasement ";
                bool doublage = false;
            }
        }
        public static void randomDoublage()
        {
            Random rnd = new Random();
            int nbrRandom = rnd.Next(1, 100);


        }
    }
}
