﻿using Imob.Models;
using System.Collections.Generic;
using System.Linq;

namespace Imob.DAL
{
    public class LocatarioDAO
    {
        private readonly Context _context;

        public LocatarioDAO(Context context) => _context = context;
        public Locatario BuscarPorNome(string nome) =>
            _context.Locatarios.FirstOrDefault(x => x.Nome == nome);

        public Locatario BuscarPorId(int id) =>
            _context.Locatarios.Find(id);
        public  bool BuscarPorCpf(string cpf)
        {
            var x = _context.Locatarios.FirstOrDefault(x => x.Cpf == cpf);
            if (x != null)
            {
                return false;
            }
            return true;
        }
        public bool Cadastrar(Locatario locatario)
        {
            if (BuscarPorNome(locatario.Nome) == null)
            {
                _context.Locatarios.Add(locatario);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Atualizar(Locatario locatario)
        {            
            _context.Locatarios.Update(locatario);
            _context.SaveChanges();
        }
        public void Remover(int id)
        {
            _context.Locatarios.Remove(BuscarPorId(id));
            _context.SaveChanges();
        }

        public List<Locatario> FiltrarPorParteNome(string parteNome) =>
            _context.Locatarios.Where(x => x.Nome.Contains(parteNome)).ToList();
        public List<Locatario> Listar() => _context.Locatarios.ToList();
    }
}
