using System.Transactions;
using Clean.Architecture.Application;
using Clean.Architecture.Application.Repositories;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Clean.Architecture.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;
        private bool _disposed;
        //
        private readonly ApplicationDbContext _context;

        // repositories

        public IUserRepository UserRepository { get; }
        public IBookRepository BookRepository { get; }
        //
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            // repositories

            UserRepository = new UserRepository(_context);
            BookRepository = new BookRepository(_context);
        }

        // save changes
        public int SaveChanges() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        // transaction
        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        // commit
        public void Commit()
        {
            if (_transaction == null)
            {
                throw new TransactionException("No transaction to commit");
            }
            try
            {
                _context.SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
            {
                throw new TransactionException("No transaction to commit");
            }

            try
            {
                await _context.SaveChangesAsync();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        // rollback
        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new TransactionException("No transaction to commit");
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
            {
                throw new TransactionException("No transaction to commit");
            }

            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }

        // dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // execute transaction
        public async Task ExecuteTransactionAsync(Action action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new TransactionException("Can't execute transaction: " + ex);
            }
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new TransactionException("Can't execute transaction: " + ex);
            }
        }
    }
}
