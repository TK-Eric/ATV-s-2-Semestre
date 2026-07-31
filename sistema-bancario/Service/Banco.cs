using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_bancario.Models;

namespace sistema_bancario.Service
{
    public class Banco
    {
        private readonly List<Conta> _contas = [];

        public void Adicionar(Conta conta)
        {
            _contas.Add(conta);
        }

        public void ProcessarMovimentações()
        {
            foreach (Conta c in _contas)
            {
                
            }
        }
    }
}