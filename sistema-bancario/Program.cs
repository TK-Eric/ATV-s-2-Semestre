// See https://aka.ms/new-console-template for more information
using sistema_bancario.Models;



decimal Saldo = 1000.00m; 
Console.WriteLine("Digite seu nome:");
string Conta = Console.ReadLine()!;
int numero = -1;
ContaCorrente cont = new ContaCorrente(Conta, Saldo); 

while (numero != 0)

{

//nao consegui utilizar o limite para fazer o limete de saldo negatio de 200 :(


Console.WriteLine("\n=======Menu banco========");
Console.WriteLine("2. Saldo Disponivel");
Console.WriteLine("3. Saque");
Console.WriteLine("4. Depositar");
Console.WriteLine("5. Encerrar");
Console.Write("Escolha uma opção: ");

    decimal opcao = decimal.Parse(Console.ReadLine()!);


switch (opcao)

{

    
    case 2: 
        Console.WriteLine($"Saldo: {Saldo}"); 
        if (Saldo == 0)
        {
             Console.WriteLine("Deposite primeiro!");
        }
        break;

    case 3: 
        Console.WriteLine("Quanto quer sacar?: "); 
        decimal Saque = decimal.Parse(Console.ReadLine()!);
        
        if (Saque <= Saldo)
        {
            Saldo -= Saque; 
            Console.WriteLine($"Saque realizado! Novo Saldo: {Saldo}");
        }
        else
        {
            Console.WriteLine("Saldo insuficiente para este saque.");
        }
        break;

    case 4: 
        Console.WriteLine("Quanto quer Depositar?: "); 
        decimal deposito = decimal.Parse(Console.ReadLine()!);
        Saldo += deposito; 
        Console.WriteLine($"Depósito realizado! Novo Saldo: {Saldo}");
        break;
    case 5:
        Console.WriteLine("Encerrando..."); 
        numero = 0;
    break;    
}
}
    
    
    // case valor1:
    //     // O que acontece se a variável for igual a valor1
    //     break; // O break é OBRIGATÓRIO no C#! Ele avisa que o bloco acabou.

    // case valor2:
    //     // O que acontece se a variável for igual a valor2
    //     break;

    // default:
    //     // O equivalente ao "else". O que acontece se nenhum dos casos acima for atendido.
    //     break;

