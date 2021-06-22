using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Helpers;
using Library.Models.Reader;

namespace Library.Services.Reader
{
    public interface IReaderService
    {
        Task<AOResult<IEnumerable<ReaderModel>>> GetReadersAsync(Func<ReaderModel, bool> func = null);

        Task<AOResult> AddReaderAsync(ReaderModel reader);
    }
}
