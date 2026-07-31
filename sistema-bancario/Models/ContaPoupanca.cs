using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_bancario.Models
{
    public class ContaPoupanca : Conta
    {
    public ContaPoupanca(string titular, decimal saldoInicial): base (titular, saldoInicial)
    {
    }

    public override void Sacar(decimal valor)
    {
        if(valor > Saldo)
            System.Console.WriteLine("Saldo Insuficiente");

            Saldo -= valor;
    }
    }
}