using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjetoConsole
{
    class Armazenando
    {
        static void Main()
        {
            string barco = "";
            string posicao = "";
            string jogador1 = "";
            string jogador2 = "";
            int qtd = 0;
            int linha = 0;
            int coluna = 0;
            int tamanho = 0;
            int inicioLinha = 0;
            int inicioColuna = 0;
            int finalLinha = 0;
            int finalColuna = 0;
            int contador = 0;
            int totalBarcos = 10;
            int quantidade = 0;
            string[,] tabuleiroInvisivel1 = new string[10, 10];
            string[,] tabuleiroInvisivel2 = new string[10, 10];
            string[,] tabuleiro1 = new string[10, 10];
            string[,] tabuleiro2 = new string[10, 10];
            var pattern = @"\b([a-jA-J]{1})([1-9]|10{1})\b";
            Regex regex = new Regex(pattern);
            Random rng = new Random();

            var dicBarcos = new Dictionary<string, string> {
            { "PS", "Porta-Aviões (5 casas). Você tem direito a 1." },
            { "NT", "Navio-Tanque (4 casas). Você tem direito a 2." },
            { "DS", "Destroyer (3 casas). Você tem direito a 3." },
            { "SB", "Submarino (2 casas). Você tem direito a 4." },
            };

            var dicTam = new Dictionary<string, int> {
            { "PS", 5},
            { "NT", 4},
            { "DS", 3},
            { "SB", 2},
            };

            var dicQtd = new Dictionary<string, int> {
            { "PS", 1},
            { "NT", 2},
            { "DS", 3},
            { "SB", 4},
            };

            LerJogadores(ref jogador1, ref jogador2, ref qtd);

            Console.WriteLine($"{jogador1}, é a sua vez de posicionar seus barcos.");
            ImprimirTabuleiro(tabuleiro1);
            PosicionarBarcos(tabuleiro1, dicBarcos, dicQtd, dicTam, barco, posicao, ref quantidade, inicioLinha, finalLinha, inicioColuna, finalColuna, tamanho, ref totalBarcos, regex, linha, coluna, contador);
            Console.WriteLine("Posicionamento Finalizado!");
            ImprimirTabuleiro(tabuleiro1);
            totalBarcos = 10;
            Console.ReadKey();
            Console.Clear();

            dicQtd["PS"] = 1;
            dicQtd["NT"] = 2;
            dicQtd["DS"] = 3;
            dicQtd["SB"] = 4;
            if (qtd == 1)
            {
                Console.WriteLine("Agora o computador posicionará seus barcos.");
                Console.WriteLine("Pressione uma tecla para continuar...");
                PosicionarBarcosAleatorios(tabuleiro2, dicQtd, dicTam);
                Console.ReadKey();
                Console.WriteLine("Posicionamento do computador Finalizado!");
                Console.ReadKey();
                Console.Clear();
            }
            if (qtd == 2)
            {
                Console.WriteLine($"{jogador2}, é a sua vez de posicionar seus barcos.");
                ImprimirTabuleiro(tabuleiro2);
                PosicionarBarcos(tabuleiro2, dicBarcos, dicQtd, dicTam, barco, posicao, ref quantidade, inicioLinha, finalLinha, inicioColuna, finalColuna,
                tamanho, ref totalBarcos, regex, linha, coluna, contador);
                Console.WriteLine("Posicionamento Finalizado!");
                ImprimirTabuleiro(tabuleiro2);
            }

            Batalhar(tabuleiro1, tabuleiro2, tabuleiroInvisivel1, tabuleiroInvisivel2, qtd, jogador1, jogador2, posicao, regex, linha, coluna, rng);
        }


        static void PosicionarBarcos(string[,] tabuleiroArmazenado, Dictionary<string, string> dicBarcos, Dictionary<string, int> dicQtd, Dictionary<string, int> dicTam,
            string barco, string posicao, ref int quantidade, int inicioLinha, int finalLinha, int inicioColuna, int finalColuna, int tamanho, ref int totalBarcos,
            Regex regex, int linha, int coluna, int contador)
        {
            while (totalBarcos != 0)
            {
                Console.WriteLine($"Quantidade de barcos para posicionar: {totalBarcos}");
                if (totalBarcos < 10)
                {
                    Console.WriteLine("Confira seu tabuleiro:");
                    ImprimirTabuleiro(tabuleiroArmazenado);
                }
                while (quantidade == 0)
                {
                    Console.WriteLine($"Qual o tipo de embarcação? Digite uma das siglas abaixo:");
                    foreach (KeyValuePair<string, string> chave in dicBarcos)
                    {
                        Console.WriteLine($"{chave.Key} - {chave.Value}");
                    }

                    do
                    {
                        barco = Console.ReadLine().Trim().ToUpper();
                        if (!dicBarcos.ContainsKey(barco))
                        {
                            Console.WriteLine($"Inválido. Digite uma sigla válida:");
                        }
                    } while (!dicBarcos.ContainsKey(barco));

                    foreach (KeyValuePair<string, int> chave in dicQtd)
                    {
                        if (chave.Key == barco)
                        {
                            if (dicQtd[barco] == 0)
                            {
                                Console.WriteLine("Você já posicionou todos os barcos desse tipo. Escolha outro.");
                            }
                            else
                            {
                                dicQtd[barco] = chave.Value - 1;
                                quantidade = 1;
                                break;
                            }
                        }
                    }
                }

                for (int i = 1; i <= 2; i++)
                {
                    if (i == 1 || i == 2)
                    {
                        do
                        {
                            if (i == 1)
                            {
                                Console.WriteLine($"Qual a primeira posição da embarcação?");
                            }
                            if (i == 2)
                            {
                                Console.WriteLine($"Qual a última posição da embarcação?");
                            }
                            Console.WriteLine("Escolha uma letra (A-J) e um número (1-10). Exemplo: A1");
                            posicao = Console.ReadLine().Trim().ToUpper();

                            if (!regex.Match(posicao).Success)
                            {
                                Console.WriteLine("Inválido. Digite uma letra válida seguida de um número válido.");
                            }
                        } while ((!regex.Match(posicao).Success) || (posicao.Length > 2 && !posicao.Contains("10")));
                        string linhaCorte = posicao.Substring(0, 1);
                        List<string> totalLinhas = new List<string>(10) { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                        linha = totalLinhas.IndexOf(linhaCorte);
                        coluna = int.Parse(posicao.Substring(1)) - 1;
                    }

                    tamanho = 0;
                    foreach (KeyValuePair<string, int> sigla in dicTam)
                    {
                        if (sigla.Key == barco)
                        {
                            contador = 0;
                            if (i == 1)
                            {
                                inicioLinha = linha;
                                inicioColuna = coluna;
                            }
                            if (i == 2)
                            {
                                finalLinha = linha;
                                finalColuna = coluna;
                                if (contador == 0 && (inicioColuna == finalColuna))
                                {
                                    if (inicioLinha < finalLinha)
                                    {
                                        tamanho = finalLinha - inicioLinha + 1;
                                    }
                                    if (inicioLinha > finalLinha)
                                    {
                                        tamanho = inicioLinha - finalLinha + 1;
                                    }
                                }
                                if (contador == 0 && (inicioLinha == finalLinha))
                                {
                                    if (inicioColuna < finalColuna)
                                    {
                                        tamanho = finalColuna - inicioColuna + 1;
                                    }
                                    if (inicioColuna > finalColuna)
                                    {
                                        tamanho = inicioColuna - finalColuna + 1;
                                    }
                                }

                                if (!ValidarPosicao(tabuleiroArmazenado, inicioLinha, finalLinha, inicioColuna, finalColuna, tamanho))
                                {
                                    Console.WriteLine("Inválido. A posição não deve conter nenhum barco.");
                                    i = 0;
                                    break;
                                }

                                if ((inicioLinha != finalLinha && inicioColuna != finalColuna) || tamanho != sigla.Value)
                                {
                                    Console.WriteLine("Inválido. A posição deve ser na horizontal ou vertical e ser equivalente ao tamanho do barco.");
                                    i = 0;
                                    break;
                                }

                                PosicionarBarco(tabuleiroArmazenado, barco, ref quantidade, inicioLinha, finalLinha, inicioColuna, finalColuna, tamanho, ref totalBarcos);
                            }
                        }
                    }
                }
            }
            totalBarcos = 10;
            Console.Clear();
        }

        static void LerJogadores(ref string jogador1, ref string jogador2, ref int qtd)
        {
            Console.WriteLine("Bem-vindo à Batalha Naval!");
            Console.WriteLine("Quantos jogadores participarão? Digite 1 ou 2");
            while ((!int.TryParse(Console.ReadLine(), out qtd)) || qtd < 1 || qtd > 2)
            {
                Console.WriteLine($"Inválido. Digite 1 ou 2.");
            }

            if (qtd == 1)
            {
                Console.WriteLine($"Qual seu nome?");
                jogador1 = Console.ReadLine();
                Console.WriteLine($"Você jogará contra o computador, boa sorte!");
                Console.WriteLine("Pressione uma tecla para continuar...");
                jogador2 = "Computador";
            }

            if (qtd == 2)
            {
                Console.WriteLine("Insira o nome dos jogadores.");

                Console.Write("Jogador 1: ");
                jogador1 = Console.ReadLine();
                Console.Write("Jogador 2: ");
                jogador2 = Console.ReadLine();
                Console.WriteLine($"Boa sorte, {jogador1} e {jogador2}!");
            }
            Console.ReadLine();
            Console.Clear();
        }

        static void PosicionarBarco(string[,] tabuleiro, string barco, ref int quantidade, int inicioLinha, int finalLinha, int inicioColuna, int finalColuna, int tamanho, ref int totalBarcos)
        {
            int contador = 0;
            for (int j = 0; j < tamanho; j++)
            {
                if (inicioLinha == finalLinha && inicioColuna != finalColuna) //horizontal
                {
                    if (inicioColuna < finalColuna)
                    {
                        inicioColuna += contador;
                    }
                    if (inicioColuna > finalColuna)
                    {
                        inicioColuna -= contador;
                    }
                    tabuleiro[inicioLinha, inicioColuna] = barco;
                    contador = 1;
                }
                if (inicioColuna == finalColuna && inicioLinha != finalLinha) //vertical
                {
                    if (inicioLinha < finalLinha)
                    {
                        inicioLinha += contador;
                    }
                    if (inicioLinha > finalLinha)
                    {
                        inicioLinha -= contador;
                    }
                    tabuleiro[inicioLinha, inicioColuna] = barco;
                    contador = 1;
                }
                if (j == tamanho - 1)
                {
                    totalBarcos -= 1;
                    Console.WriteLine("Barco Registrado!");
                    Console.WriteLine("Pressione uma tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                    contador = 0;
                    quantidade = 0;
                }
            }
        }

        static bool ValidarPosicao(string[,] tabuleiro, int inicioLinha, int finalLinha, int inicioColuna, int finalColuna, int tamanho)
        {
            try
            {
                int contador = 0;
                for (int j = 0; j < tamanho; j++)
                {
                    if (inicioLinha == finalLinha && inicioColuna != finalColuna)
                    {
                        if (inicioColuna < finalColuna)
                        {
                            inicioColuna += contador;
                        }
                        if (inicioColuna > finalColuna)
                        {
                            inicioColuna -= contador;
                        }
                        if (tabuleiro[inicioLinha, inicioColuna] != null)
                        {
                            return false;
                        }
                        contador = 1;
                    }
                    if (inicioColuna == finalColuna && inicioLinha != finalLinha)
                    {
                        if (inicioLinha < finalLinha)
                        {
                            inicioLinha += contador;
                        }
                        if (inicioLinha > finalLinha)
                        {
                            inicioLinha -= contador;
                        }
                        if (tabuleiro[inicioLinha, inicioColuna] != null)
                        {
                            return false;
                        }
                        contador = 1;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        static bool FimDeJogo(string[,] tabuleiro)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tabuleiro[i, j] != null && tabuleiro[i, j] != "X " && tabuleiro[i, j] != "A ")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void ImprimirTabuleiro(string[,] tabuleiro)
        {
            List<string> linhasCabecalho = new List<string>(10) { "::A", "::B", "::C", "::D", "::E", "::F", "::G", "::H", "::I", "::J" };
            Console.WriteLine("::::::::::::::::::::::::::::::::::::");
            Console.WriteLine("::  01 02 03 04 05 06 07 08 09 10:::");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(linhasCabecalho[i]);
                for (int j = 0; j < 10; j++)
                {
                    Console.Write($" {(tabuleiro[i, j] == null ? "▒▒" : tabuleiro[i, j])}");
                }
                Console.WriteLine(" ::");
            }
            Console.WriteLine("::::::::::::::::::::::::::::::::::::");
        }

        static void Batalhar(string[,] tabuleiro1, string[,] tabuleiro2, string[,] tabulInv1, string[,] tabulInv2, int qtd, string jogador1, string jogador2, string posicao, Regex regex,
            int linha, int coluna, Random rng)
        {
            int count = 2;
            Console.WriteLine("Vamos começar nossa batalha!");
            Console.WriteLine("Pressione uma tecla para continuar...");
            Console.ReadKey();
            do
            {
                if (count % 2 == 0 && FimDeJogo(tabuleiro1) == false)
                {
                    Console.Clear();
                    if (qtd == 1)
                    {
                        Console.WriteLine($"É a sua vez de jogar.");
                    }
                    else
                    {
                        Console.WriteLine($"É a vez de {jogador1} jogar.");
                    }
                    Atacar(tabuleiro2, tabulInv2, regex, ref linha, ref coluna);
                }
                else if (FimDeJogo(tabuleiro2) == false)
                {
                    Console.Clear();
                    if (qtd == 1)
                    {
                        Console.WriteLine($"É a vez do computador jogar.");
                        Console.WriteLine("Pressione uma tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        AtacarPC(tabuleiro1, tabulInv1, ref linha, ref coluna, rng);
                    }
                    else
                    {
                        Console.WriteLine($"É a vez de {jogador2} jogar.");
                        Atacar(tabuleiro1, tabulInv1, regex, ref linha, ref coluna);
                    }
                }
                count++;

                if (FimDeJogo(tabuleiro1) == true)
                {
                    Console.WriteLine($"Fim de jogo, {jogador2} venceu!");
                }
                if (FimDeJogo(tabuleiro2) == true)
                {
                    Console.WriteLine($"Fim de jogo, {jogador1} venceu!");
                }

            } while (FimDeJogo(tabuleiro2) == false && FimDeJogo(tabuleiro1) == false);
        }

        static void Atacar(string[,] tabuleiro, string[,] tabulInv, Regex regex, ref int linha, ref int coluna)
        {
            string posicao;
            do
            {
                Console.WriteLine("Escolha uma posição para atacar:");
                ImprimirTabuleiro(tabulInv);
                do
                {
                    Console.WriteLine("Escolha uma letra (A-J) e um número (1-10). Exemplo: A1");
                    posicao = Console.ReadLine().Trim().ToUpper();

                    if (!regex.Match(posicao).Success)
                    {
                        Console.WriteLine("Inválido. Digite uma letra válida seguida de um número válido.");
                    }
                } while ((!regex.Match(posicao).Success) || (posicao.Length > 2 && !posicao.Contains("10")));
                string linhaCorte = posicao.Substring(0, 1);
                List<string> totalLinhas = new List<string>(10) { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                linha = totalLinhas.IndexOf(linhaCorte);
                coluna = int.Parse(posicao.Substring(1)) - 1;

                if (tabulInv[linha, coluna] != null)
                {
                    Console.WriteLine("Você já atacou essa posição. Escolha outra.");
                }
            } while (tabulInv[linha, coluna] != null);
            tabulInv[linha, coluna] = (tabuleiro[linha, coluna] == null ? "A " : "X ");
            tabuleiro[linha, coluna] = (tabuleiro[linha, coluna] == null ? "A " : "X ");

            switch (tabulInv[linha, coluna])
            {
                case "A ":
                    Console.WriteLine("Não há nada nessa posição.");
                    Console.ReadKey();
                    break;
                case "X ":
                    Console.WriteLine("Você acertou um navio!");
                    Console.ReadKey();
                    break;
            }
        }

        static void PosicionarBarcosAleatorios(string[,] tabuleiro, Dictionary<string, int> dicQtd, Dictionary<string, int> dicTam)
        {
            List<string> siglas = new List<string>() { "PS", "NT", "DS", "SB" };
            Random rng = new Random();
            int tamanho;
            int quantidade;
            int linhaAleatoria;
            int colunaAleatoria;
            int inicioColuna;
            int finalColuna;
            int inicioLinha;
            int finalLinha;

            foreach (var barco in siglas)
            {
                tamanho = dicTam[barco];
                quantidade = dicQtd[barco];
                for (int j = 0; j < quantidade; j++)
                {
                    linhaAleatoria = rng.Next(0, 10);
                    colunaAleatoria = rng.Next(0, 10);
                    inicioColuna = colunaAleatoria;
                    finalColuna = colunaAleatoria;
                    inicioLinha = linhaAleatoria;
                    finalLinha = linhaAleatoria;

                    if (rng.Next(2) == 1) //vertical
                    {
                        if (rng.Next(2) == 1 && (linhaAleatoria - tamanho) > 0)//subir
                        {
                            inicioLinha -= (tamanho - 1);
                        }
                        else if (linhaAleatoria + tamanho <= 9) //descer
                        {
                            finalLinha += (tamanho - 1);
                        }
                        else
                        {
                            j--;
                            continue;
                        }
                    }
                    else //horizontal
                    {
                        if (rng.Next(2) == 1 && (colunaAleatoria + tamanho) <= 9) //direita
                        {
                            finalColuna += (tamanho - 1);
                        }
                        else if (colunaAleatoria - tamanho >= 0) //esquerda
                        {
                            inicioColuna -= (tamanho - 1);
                        }
                        else
                        {
                            j--;
                            continue;
                        }

                    }

                    if (ValidarPosicao(tabuleiro, inicioLinha, finalLinha, inicioColuna, finalColuna, tamanho))
                    {
                        for (int linha = inicioLinha; linha <= finalLinha; linha++)
                        {
                            for (int coluna = inicioColuna; coluna <= finalColuna; coluna++)
                            {
                                tabuleiro[linha, coluna] = barco;
                            }
                        }
                    }
                    else
                    {
                        j--;
                    }
                }
            }
        }

        static void AtacarPC(string[,] tabuleiro, string[,] tabulInv, ref int linha, ref int coluna, Random rng)
        {
            string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            do
            {
                linha = rng.Next(0, 10);
                coluna = rng.Next(0, 10);

            } while (tabulInv[linha, coluna] != null);
            tabulInv[linha, coluna] = (tabuleiro[linha, coluna] == null ? "A " : "X ");
            tabuleiro[linha, coluna] = (tabuleiro[linha, coluna] == null ? "A " : "X ");

            Console.Write($"O computador atacou a posição {letras[linha]}{coluna + 1} ");
            switch (tabulInv[linha, coluna])
            {
                case "A ":
                    Console.WriteLine("e errou!");
                    break;
                case "X ":
                    Console.WriteLine("e acertou um navio!");
                    break;
            }

            ImprimirTabuleiro(tabulInv);
            Console.WriteLine();
            Console.WriteLine("Pressione uma tecla para continuar...");
            Console.ReadKey();
        }
    }
}