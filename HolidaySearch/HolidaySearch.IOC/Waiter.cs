using HolidaySearch.Search.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidaySearch.IOC
{
    public static class Waiter
    {
        public static T GetInstance<T>()
        {
            return (T)ObjectFactory.GetInstance(typeof(T));
        }
    }
}
