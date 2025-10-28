using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Models;
public class PageData<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
