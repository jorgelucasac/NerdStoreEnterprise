﻿using System;
using System.Threading.Tasks;
using Estudos.NSE.Carrinho.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Carrinho.API.Data.Repository
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly CarrinhoDbContext _context;

        public CarrinhoRepository(CarrinhoDbContext context)
        {
            _context = context;
        }

        public async Task<CarrinhoCliente> ObterCarrinhoCliente(Guid clienteId)
        {
            return await _context.CarrinhoCliente
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
        }

        public async Task<CarrinhoItem> ObterCarrinhoItem(Guid carrinhoId, Guid produtoId)
        {
            return await _context.CarrinhoItens
                .FirstOrDefaultAsync(item => item.CarrinhoId == carrinhoId && produtoId == produtoId);
        }

        public async Task Adicionar(CarrinhoCliente carrinhoCliente)
        {
            await _context.CarrinhoCliente.AddAsync(carrinhoCliente);
        }

        public async Task AdicionarItem(CarrinhoItem carrinhoItem)
        {
            await _context.CarrinhoItens.AddAsync(carrinhoItem);
        }

        public void Atualizar(CarrinhoCliente carrinhoCliente)
        {
            _context.CarrinhoCliente.Update(carrinhoCliente);
        }

        public void AtualizarItem(CarrinhoItem carrinhoItem)
        {
            _context.CarrinhoItens.Update(carrinhoItem);
        }

        public void RemoverItem(CarrinhoItem carrinhoItem)
        {
            _context.CarrinhoItens.Remove(carrinhoItem);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}