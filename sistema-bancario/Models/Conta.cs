using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
// ↑ Esses "usings" importam bibliotecas do .NET. No seu código, só o
// "System" está realmente sendo usado (por causa do Console.WriteLine
// e do ArgumentException). Os outros (Collections.Generic, Linq,
// Runtime.CompilerServices, Threading.Tasks) não são utilizados aqui —
// provavelmente ficaram do template padrão do Visual Studio e
// poderiam ser removidos sem problema.

namespace sistema_bancario.Models
{
    // Define o "namespace" (agrupamento lógico) do projeto.
    // Aqui indica que essa classe faz parte da camada de "Models"
    // do sistema bancário — ou seja, representa uma entidade de dados/negócio.

    public class Conta
    {
        // Classe que representa uma conta bancária genérica.
        // Ela é a "base" (classe mãe) para outras contas mais específicas,
        // como ContaCorrente, ContaPoupanca, etc. (por isso o construtor é "protected").

        // ===================== PROPRIEDADES =====================

        // Titular: nome do dono da conta.
        // "get;" sem "set" → só pode ser definida DENTRO da classe (no construtor)
        // e nunca mais alterada depois. Isso é chamado de propriedade "somente leitura"
        // (imutável após a criação do objeto).
        public string Titular { get; }

        // Saldo: valor monetário disponível na conta.
        // "get" é público → qualquer código pode LER o saldo.
        // "protected set" → só a própria classe OU classes filhas (que herdam de Conta)
        // podem ALTERAR o valor. Ninguém de fora (ex: outra classe qualquer) pode
        // simplesmente fazer "conta.Saldo = 999999".
        //
        // Isso é o conceito de ENCAPSULAMENTO: os dados sensíveis (saldo)
        // são protegidos contra alterações indevidas vindas de fora da classe.
        // A única forma seria por métodos controlados, como Depositar() e Sacar().
        public decimal Saldo { get; protected set; }

        // ===================== CONSTRUTOR =====================

        // Construtor da classe. É executado quando um objeto Conta (ou de uma
        // classe filha) é criado.
        //
        // "protected" significa que esse construtor NÃO pode ser chamado
        // diretamente de fora (ex: "new Conta(...)" não funcionaria em outra classe).
        // Só pode ser usado internamente ou por classes que herdam de Conta
        // (via "base(titular, saldoInicial)" no construtor da filha).
        // Isso reforça que "Conta" é pensada para ser uma classe BASE,
        // não para ser instanciada diretamente.
        public Conta(string titular, decimal saldoInicial)
        {
            Titular = titular;           // define o titular no momento da criação
            Saldo = saldoInicial;        // define o saldo inicial da conta
        }

        // ===================== MÉTODOS =====================

        // Método para depositar dinheiro na conta.
        // Esse comportamento é IGUAL para qualquer tipo de conta,
        // por isso não é "virtual" (não precisa ser sobrescrito pelas filhas).
        public void Depositar(decimal valor)
        {
            // Validação: não faz sentido depositar valor zero ou negativo
            if (valor <= 0)
                throw new ArgumentException("Deposito deve ser positivo");
            // ↑ "throw" interrompe a execução e lança um erro (exceção)
            // do tipo ArgumentException, com uma mensagem explicando o motivo.
            // Quem chamar esse método vai precisar tratar esse erro (try/catch)
            // ou o programa vai quebrar ali.

            Saldo += valor; // soma o valor depositado ao saldo atual
        }

        // Método para sacar dinheiro da conta.
        //
        // "virtual" → permite que classes filhas SOBRESCREVAM (override)
        // esse método e mudem o comportamento. Isso é POLIMORFISMO:
        // a classe base define que "toda conta sabe sacar", mas cada
        // tipo de conta pode implementar a lógica de saque à sua maneira
        // (ex: ContaCorrente pode permitir saldo negativo até um limite,
        // ContaPoupanca pode não permitir saldo negativo, etc).
        public virtual void Sacar(decimal valor)
        {
            // Validação: valor de saque precisa ser positivo
            if (valor <= 0) throw new ArgumentException("Valor Invalido");

            Saldo -= valor; // subtrai o valor sacado do saldo
            // ⚠️ Observação: aqui não há validação de saldo insuficiente!
            // Isso significa que, na classe base, é possível deixar o saldo
            // negativo sem restrição. Dependendo da regra de negócio,
            // isso pode ser proposital (para as filhas decidirem o limite)
            // ou pode ser um bug a corrigir.

            Console.WriteLine($"Foi realizado o saque de {valor}. Saldo atual de {Saldo}");
            // ↑ Exibe uma mensagem no console confirmando o saque.
        }
    }
}