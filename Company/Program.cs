using Company.Data;
using Company.Services;
using Company.UI;
using Microsoft.Extensions.Configuration;

namespace Company
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new CompanyApp();
            app.Run();
        }
    }
}
