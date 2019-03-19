﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
namespace DomainInterface
{
    public interface IUnitsRepo
    {
        List<Units> getAllUnit();
        ReturnType saveUnit(Units units);     
    }
}
