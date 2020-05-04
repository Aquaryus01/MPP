using Domain.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.repository
{
    public interface IRegistrationRepository : ICrudRepository<string, Registration>
    {
    }
}
