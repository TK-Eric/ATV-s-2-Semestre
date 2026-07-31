// See https://aka.ms/new-console-template for more information
using sistema_bancario.Models;

Console.WriteLine("\n=======Menu banco========");
Console.Write("1. Conta");
Console.Write("2. Saldo Disponivel ");
Console.Write("3. Saque ");
Console.Write("4. Depositar");
decimal opcao = decimal.Parse(Console.ReadLine()!);

ContaCorrente cont = ContaCorrente ();

switch (opcao){

case 1: 
    Console.WriteLine("Digite seu nome:"); 
    string Conta = Console.ReadLine()!;
    break;
    case 2: 
    Console.WriteLine($"Saldo:{Saldo}"); 
    if (Saldo -= 0)
        {
             Console.WriteLine("Deposite primeiro!");
        }
    break;
    case 3: 
    Console.WriteLine("Quanto quer sacar?: "); 
    string Saque = Console.ReadLine()!;
    Saldo - Saque
    break;
    case 4: 
    Console.WriteLine("Quarta"); 
    break;
    case 5: 
    Console.WriteLine("Quinta"); 
    break;
    case 6: 
    Console.WriteLine("Sexta"); 
    break;
    case 7: 
    Console.WriteLine("Sabado"); 
    break;
    
    // case valor1:
    //     // O que acontece se a variável for igual a valor1
    //     break; // O break é OBRIGATÓRIO no C#! Ele avisa que o bloco acabou.

    // case valor2:
    //     // O que acontece se a variável for igual a valor2
    //     break;

    // default:
    //     // O equivalente ao "else". O que acontece se nenhum dos casos acima for atendido.
    //     break;
}
