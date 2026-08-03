using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_bancario.Models
{
 
    public class ContaCorrente : Conta
    {

        private const decimal Limite = 200;

        public ContaCorrente(string titular, decimal saldo)
            
            : base(titular, saldo)
        {
           
        }

        public override void Sacar(decimal valor)
{
    if (valor <= 0) 
        throw new ArgumentException("Valor Invalido");

    if (valor > Saldo + Limite)
        throw new InvalidOperationException("Valor alem do limite");

    Saldo -= valor;

    Console.WriteLine($"Foi realizado o saque de {valor}. Saldo atual de {Saldo}");
}

    }
}