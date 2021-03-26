using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IEncodingService
    {
        public byte[] CalculateSHA256(string password);
    }
}
