using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.domain
{
    public interface Entity<T>
    {
        T Id { get; set; }
    }
}
