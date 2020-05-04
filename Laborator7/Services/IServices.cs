using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.domain;

namespace Services
{
    public interface IServices
    {
        void login(OfficeDTO officeDTO, Observer client);
        void logout(OfficeDTO officeDTO, Observer client);
        void submitRegistration(InfoSubmitDTO infoSubmit);
        SampleDTO[] getCurrentSamples();
        RegistrationDTO[] searchBySample(StyleDTO styleDTO);
    }
}
