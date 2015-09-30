using System;
using System.IO;
using System.Net;
using System.Text;


namespace Get
{
    public class DenisClient
    {



        public static void Main(string[] args)
        {


            Console.WriteLine("Пожалуйста введите URL адресс");
            string http = "http://www.";
            string URL = Convert.ToString(http + Console.ReadLine());

            WebRequest GETURL = WebRequest.Create(URL);

            Stream objStream = GETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);
            
            string Line = "";

            while (Line != null)
            {

                Line = objReader.ReadLine();
                if (Line != null)
                {
                    Console.WriteLine("{0}", Line);
                }

            }

            var File = new FileStream(@"D:\GetTest.html", FileMode.OpenOrCreate);

            StreamWriter s = new StreamWriter(File);

            s.Write(objReader.ReadToEnd());

            s.Write("111!");
            s.Close();


            Console.ReadKey();
        }
    }
}






