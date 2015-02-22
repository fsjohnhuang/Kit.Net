using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lpp.TplHelper;
using lpp.TplHelper.Attr;
using System.Diagnostics;

namespace TplHelperUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            Person person = new Person();
            person.Name = "John Huang";
            person.ShortName = "John";
            person.Age = 18;
            person.Now = DateTime.Now;
            person.Sex = true;
            string code = "@name(@shortname):age->@age(@shortname)@now";
            Compiler.Recompile(typeof(Person));
            watch.Start();
            string result = Compiler.Compile<Person>(code, person);
            watch.Stop();
            Console.WriteLine("code: @name(@shortname):age->@age(@shortname)@now");
            Console.WriteLine("result: " + result);
            Console.WriteLine("Complete:" + watch.ElapsedMilliseconds);
            Console.Read();
        }
    }

    class Person
    {
        [TplVar("name")]
        public string Name { get; set; }
        [TplVar("shortname")]
        public string ShortName { get; set; }
        [TplVar("age")]
        public int Age { get; set; }
        [TplVar("now", Format="yyyyMMdd-HHmm-ss")]
        public DateTime Now { get; set; }
        public bool Sex { get; set; }
        public bool ex { get; set; }
        public bool e { get; set; }
        public bool Sx { get; set; }
        public bool Ssex { get; set; }
        public bool Sedx { get; set; }
        public bool Sefx { get; set; }
        public bool Sexc { get; set; }
        public bool Sexs { get; set; }
        public bool Seax { get; set; }
        public bool Sezx { get; set; }
        public bool Seaax { get; set; }
        public bool Secx { get; set; }
        public bool Sexcc { get; set; }
        public bool Sexaes { get; set; }
        public bool Sexgg { get; set; }
        public bool Sexg { get; set; }
        public bool Sexaseq { get; set; }
        public bool Sexge { get; set; }
        public bool Sexzse { get; set; }
    }
}
