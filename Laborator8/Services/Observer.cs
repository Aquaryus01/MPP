using Domain.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    public interface Observer
    {
        void OfficeSubmitted(SampleDTO[] curse);
    }

}
