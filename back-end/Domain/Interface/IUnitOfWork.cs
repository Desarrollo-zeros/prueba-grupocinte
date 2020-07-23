using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public int Commit();

    }
}
