using System;
using System.Threading;

namespace AI_Tris
{
    class Program
    {
        static sceltaCasuale rand = new sceltaCasuale();
        static string segnoUtente = "X", segnoCPU = "O";

        static void Main(string[] args)
        {
            #region Tris
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  _______ _____  _____  _____ ");
            Console.WriteLine(" |__   __|  __ \\|_   _|/ ____|");
            Console.WriteLine("    | |  | |__) | | | | (___  ");
            Console.WriteLine("    | |  |  _  /  | |  \\___ \\ ");
            Console.WriteLine("    | |  | | \\ \\ _| |_ ____) |");
            Console.WriteLine("    |_|  |_|  \\_\\_____|_____/ ");
            Console.ForegroundColor = ConsoleColor.White;

            Thread.Sleep(500);
            Console.WriteLine("\n\n\nPremere un tasto qualunque per avviare");
            Console.Readtasto();
            Console.Clear();
            #endregion

            PVE();
        }

        static void PVE()
        {
            #region Variabili
            int[,] conteggioTris =
            {
                    {0, 0, 0, 0 },
                    {1, 0, 0, 0 },
                    {2, 0, 0, 0 },
                    {0, 0, 0, 0 },
                    {0, 1, 0, 0 },
                    {0, 2, 0, 0 },
                    {0, 0, 0, 0 },
                    {0, 0, 0, 0 },
            };

            sbyte[] posizioni = new sbyte[2];
            byte cicli = 5;

            bool val, utenteCheInizia, bloccaUnaLinea, sceltaCasuale;
            bool vittoria = false;

            string[,] griglia = new string[3, 3];
            for (int i = 0; i < griglia.GetLength(0); i++)
            {
                for (int j = 0; j < griglia.GetLength(1); j++)
                {
                    griglia[i, j] = " ";
                }
            }

            string moneta, diff;
            #endregion

            #region Difficoltà
            do
            {
                val = false;
                Console.WriteLine("0 - Facile\n1 - Normale");
                Console.Write("Scegli la difficoltà: ");
                diff = Console.ReadLine();

                if (diff != "0" && diff != "1")
                {
                    val = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("La scelta selezionata non è disponibile!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (val);
            Console.Clear();
            #endregion

            #region Lancio della moneta
            do
            {
                val = false;
                Console.WriteLine("Testa o Croce?");
                moneta = Console.ReadLine().ToLower();

                if (moneta != "testa" && moneta != "croce")
                {
                    val = true;
                    Console.WriteLine("Your choice is not available!\n");
                }
            } while (val);

            val = rand.Next(0, 2) == 1;
            utenteCheInizia = (moneta == "testa" && !val) || (moneta == "croce" && val);

            if (val)
                Console.WriteLine("\nTesta");
            else
                Console.WriteLine("\nCroce");
            #endregion

            #region Partita
            StampaLaGriglia(griglia);

            if (!utenteCheInizia)
            {
                cicli = 4;
                griglia[rand.Next(0, 3), rand.Next(0, 3)] = segnoCPU;
            }

            for (int i = 0; i < cicli; i++)
            {
                #region Reset variabili
                sceltaCasuale = true;
                bloccaUnaLinea = true;

                posizioni[0] = 5;
                posizioni[1] = 2;

                for (int f = 0; f < 8; f++)
                {
                    conteggioTris[f, 2] = 0;
                    conteggioTris[f, 3] = 0;
                }
                #endregion

                MossaUtente(griglia, posizioni);

                StampaLaGriglia(griglia);

                vittoria = Vittoria(griglia, conteggioTris);

                if (vittoria)
                {
                    Console.SetCursorPosition(0, 7);
                    ScrittaVittoria();
                    i = 6;
                }

                if (i < 4)
                {
                    switch (diff)
                    {
                        case "0":
                            PosizioneCasuale(griglia);
                            break;

                        case "1":
                            for (int j = 0; j < 8; j++)
                            {
                                if (conteggioTris[j, 3] == 2 && griglia[conteggioTris[j, 0], conteggioTris[j, 1]] == " ")
                                {
                                    griglia[conteggioTris[j, 0], conteggioTris[j, 1]] = segnoCPU;
                                    sceltaCasuale = false;
                                    bloccaUnaLinea = false;
                                    break;
                                }
                            }

                            if (bloccaUnaLinea)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    if (conteggioTris[j, 2] == 2 && griglia[conteggioTris[j, 0], conteggioTris[j, 1]] == " ")
                                    {
                                        griglia[conteggioTris[j, 0], conteggioTris[j, 1]] = segnoCPU;
                                        sceltaCasuale = false;
                                        break;
                                    }
                                }
                            }

                            if (sceltaCasuale)
                            {
                                PosizioneCasuale(griglia);
                            }
                            break;
                    }

                    StampaLaGriglia(griglia);

                    for (int j = 0; j < 8; j++)
                    {
                        conteggioTris[j, 3] = 0;
                    }

                    vittoria = Vittoria(griglia, conteggioTris);
                    if (vittoria)
                    {
                        Console.SetCursorPosition(0, 7);
                        GameOver();
                        i = 6;
                    }
                }
            }

            #region StampaLaGriglia end of the game message
            if (!vittoria)
            {
                Console.SetCursorPosition(0, 7);
                Tie();
            }
            #endregion

            #endregion
        }

        static void PosizioneCasuale(string[,] griglia)
        {
            int riga, colonna;
            do
            {
                riga = rand.Next(0, 3);
                colonna = rand.Next(0, 3);
            } while (griglia[riga, colonna] != " ");
            griglia[riga, colonna] = segnoCPU;
        }

        static void StampaLaGriglia(string[,] matrice)
        {
            Console.Clear();
            Console.WriteLine($" {matrice[0, 0]} | {matrice[0, 1]} | {matrice[0, 2]}");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" {matrice[1, 0]} | {matrice[1, 1]} | {matrice[1, 2]}");
            Console.WriteLine($"---+---+---");
            Console.WriteLine($" {matrice[2, 0]} | {matrice[2, 1]} | {matrice[2, 2]}");
        }

        #region Vittoria, sconfitta, pareggio
        static void Pareggio()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" _____      _   _        ");
            Console.WriteLine("|  __ \\    | | | |       ");
            Console.WriteLine("| |__) |_ _| |_| |_ __ _ ");
            Console.WriteLine("|  ___/ _` | __| __/ _` |");
            Console.WriteLine("| |  | (_| | |_| || (_| |");
            Console.WriteLine("|_|   \\__,_|\\__|\\__\\__,_|");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void ScrittaVittoria()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" _    _       _         _       _        ");
            Console.WriteLine("| |  | |     (_)       (_)     | |       ");
            Console.WriteLine("| |__| | __ _ _  __   ___ _ __ | |_ ___  ");
            Console.WriteLine("|  __  |/ _` | | \\ \\ / / | '_ \\| __/ _ \\ ");
            Console.WriteLine("| |  | | (_| | |  \\ V /| | | | | || (_) |");
            Console.WriteLine("|_|  |_|\\__,_|_|   \\_/ |_|_| |_|\\__\\___/ ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void GameOver()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   _____                         ____                 ");
            Console.WriteLine("  / ____|                       / __ \\                ");
            Console.WriteLine(" | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ ");
            Console.WriteLine(" | | |_ |/ _` | '_ ` _ \\ / _ \\ | |  | \\ \\ / / _ \\ '__|");
            Console.WriteLine(" | |__| | (_| | | | | | |  __/ | |__| |\\ V /  __/ |   ");
            Console.WriteLine("  \\_____|\\__,_|_| |_| |_|\\___|  \\____/  \\_/ \\___|_|   ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        static void MossaUtente(string[,] griglia, sbyte[] pos)
        {
            ConsoleKey tasto;
            bool invio = false;
            sbyte riga, col;

            StampaLaGriglia(griglia);

            #region Spostamento nella griglia
            while (!invio)
            {
                Console.SetCursorPosition(pos[0], pos[1]);
                tasto = Console.Readtasto().tasto;

                switch (tasto)
                {
                    case Consoletasto.invio:
                        riga = (sbyte)((pos[1]) / 2);
                        col = (sbyte)((pos[0] - 1) / 4);
                        if (griglia[riga, col] == " ")
                        {
                            griglia[riga, col] = segnoUtente;
                            invio = true;
                        }
                        break;

                    // tasto: frecce
                    case Consoletasto.LeftArrow:
                        pos[0] -= 4;
                        if (pos[0] < 1)
                            pos[0] = 1;
                        break;
                    case Consoletasto.UpArrow:
                        pos[1] -= 2;
                        if (pos[1] < 0)
                            pos[1] = 0;
                        break;
                    case Consoletasto.RightArrow:
                        pos[0] += 4;
                        if (pos[0] > 9)
                            pos[0] = 9;
                        break;
                    case Consoletasto.DownArrow:
                        pos[1] += 2;
                        if (pos[1] > 4)
                            pos[1] = 4;
                        break;

                    default:
                        break;
                }
            }
            #endregion
        }

        static bool Vittoria(string[,] griglia, int[,] tris)
        {
            bool vittoria = false;

            #region Conntrollo vittoria
            for (byte j = 0; j < 3; j++)
            {
                #region Righe
                if (griglia[0, j] == segnoUtente)
                    tris[0, 2]++;
                else if (griglia[0, j] == " ")
                    tris[0, 1] = j;
                else
                    tris[0, 3]++;

                if (griglia[1, j] == segnoUtente)
                    tris[1, 2]++;
                else if (griglia[1, j] == " ")
                    tris[1, 1] = j;
                else
                    tris[1, 3]++;

                if (griglia[2, j] == segnoUtente)
                    tris[2, 2]++;
                else if (griglia[2, j] == " ")
                    tris[2, 1] = j;
                else
                    tris[2, 3]++;
                #endregion

                #region Colonne
                if (griglia[j, 0] == segnoUtente)
                    tris[3, 2]++;
                else if (griglia[j, 0] == " ")
                    tris[3, 0] = j;
                else
                    tris[3, 3]++;

                if (griglia[j, 1] == segnoUtente)
                    tris[4, 2]++;
                else if (griglia[j, 1] == " ")
                    tris[4, 0] = j;
                else
                    tris[4, 3]++;

                if (griglia[j, 2] == segnoUtente)
                    tris[5, 2]++;
                else if (griglia[j, 2] == " ")
                    tris[5, 0] = j;
                else
                    tris[5, 3]++;
                #endregion

                #region Diagonali
                if (griglia[j, j] == segnoUtente)
                    tris[6, 2]++;
                else if (griglia[j, j] == " ")
                {
                    tris[6, 0] = j;
                    tris[6, 1] = j;
                }
                else
                    tris[7, 3]++;

                if (griglia[j, 2 - j] == segnoUtente)
                    tris[7, 2]++;
                else if (griglia[j, 2 - j] == " ")
                {
                    tris[7, 0] = j;
                    tris[7, 1] = 2 - j;
                }
                else
                    tris[7, 3]++;
                #endregion
            }
            #endregion

            for (byte i = 0; i < tris.GetLength(0); i++)
            {
                if (tris[i, 2] == 3 || tris[i, 3] == 3)
                {
                    vittoria = true;
                    i = 254;
                }
            }

            return vittoria;
        }
    }
}