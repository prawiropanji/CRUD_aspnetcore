﻿using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsAdderService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

       

    }
}
