using System;

class Program
{
    // Matriz 3x3 de strings (exigência do enunciado)
    static string[,] tab = new string[3,3];

    static void Main()
    {
        IniciarTabuleiro();

        string jogadorAtual = "X";
        string vencedor = null;

        while (true)
        {
            Console.Clear();
            DesenharTabuleiro();

            // Entrada de jogada (linhas e colunas 1..3, mais amigável)
            Console.WriteLine($"\nVez de {jogadorAtual}. Informe LINHA e COLUNA (1-3). Ex: 2 3");
            Console.Write("> ");
            var entrada = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entrada)) continue;

            var partes = entrada.Split(new char[] {' ', ',', ';', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length != 2 || !int.TryParse(partes[0], out int lin) || !int.TryParse(partes[1], out int col))
            {
                Console.WriteLine("Formato inválido. Tecle qualquer tecla para tentar de novo...");
                Console.ReadKey();
                continue;
            }

            // Convertendo para índice 0..2
            lin--; col--;
            if (!PosicaoValida(lin, col))
            {
                Console.WriteLine("Posição inválida. Tecle qualquer tecla para tentar de novo...");
                Console.ReadKey();
                continue;
            }
            if (!CasaVazia(lin, col))
            {
                Console.WriteLine("Essa casa já foi usada. Tecle qualquer tecla para tentar de novo...");
                Console.ReadKey();
                continue;
            }

            tab[lin, col] = jogadorAtual;

            // --- Verificação de vitória (EXIGIDO) ---
            vencedor = VerificarVencedor(tab);
            if (vencedor != null)
            {
                Console.Clear();
                DesenharTabuleiro();
                Console.WriteLine($"\n🎉 Jogador {vencedor} venceu!");
                break;
            }

            if (TabuleiroCheio())
            {
                Console.Clear();
                DesenharTabuleiro();
                Console.WriteLine("\nDeu velha! Empate. 😅");
                break;
            }

            // alterna jogador
            jogadorAtual = (jogadorAtual == "X") ? "O" : "X";
        }

        Console.WriteLine("\nFim de jogo.");
    }

    // ------------------- Funções auxiliares -------------------

    static void IniciarTabuleiro()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                tab[i, j] = " "; // mantém string (não char) como pede o enunciado
    }

    static void DesenharTabuleiro()
    {
        Console.WriteLine("   1   2   3");
        for (int i = 0; i < 3; i++)
        {
            Console.Write($" {i+1} ");
            for (int j = 0; j < 3; j++)
            {
                Console.Write(tab[i, j]);
                if (j < 2) Console.Write(" | ");
            }
            if (i < 2) Console.Write("\n  ---+---+---\n");
        }
        Console.WriteLine();
    }

    static bool PosicaoValida(int i, int j) => i >= 0 && i < 3 && j >= 0 && j < 3;

    static bool CasaVazia(int i, int j) => tab[i, j] == " ";

    static bool TabuleiroCheio()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (tab[i, j] == " ") return false;
        return true;
    }

    /// <summary>
    /// Verifica as 4 possibilidades de vitória:
    /// 1) Linhas (horizontal)  -> todos tab[i, j] iguais em j=0..2
    /// 2) Colunas (vertical)   -> todos tab[i, j] iguais em i=0..2
    /// 3) Diagonal principal   -> i == j
    /// 4) Diagonal secundária  -> i + j == 2
    /// Retorna "X" ou "O" se houver vencedor; senão, null.
    /// </summary>
    static string VerificarVencedor(string[,] b)
    {
        // 1) Horizontais
        for (int i = 0; i < 3; i++)
            if (IguaisENaoVazios(b[i,0], b[i,1], b[i,2]))
                return b[i,0];

        // 2) Verticais
        for (int j = 0; j < 3; j++)
            if (IguaisENaoVazios(b[0,j], b[1,j], b[2,j]))
                return b[0,j];

        // 3) Diagonal principal (i == j)
        if (IguaisENaoVazios(b[0,0], b[1,1], b[2,2]))
            return b[0,0];

        // 4) Diagonal secundária (i + j == 2)
        if (IguaisENaoVazios(b[0,2], b[1,1], b[2,0]))
            return b[0,2];

        return null;
    }

    static bool IguaisENaoVazios(string a, string b, string c)
        => a != " " && a == b && b == c;
}
