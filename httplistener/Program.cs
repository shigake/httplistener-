using System;
using System.Net;

namespace httplistener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string[] str = new string[2];
            str[0] = "http://127.0.0.1/";
            str[1] = "http://127.0.0.1/test/";

            SimpleListenerExample(str);
        }

        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            int i = 0;

            while (true) {
                HttpListener listener = new HttpListener();
                foreach (string s in prefixes)
                {
                    listener.Prefixes.Add(s);
                }
                listener.Start();
                Console.WriteLine("Listening... + "+ i);
                i++;
                HttpListenerContext context = listener.GetContext();
                var url = context.Request.Url.ToString();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = "";
                switch (url) {
                    case "http://127.0.0.1/": responseString ="<HTML><BODY> Hello world!</BODY></HTML>";
                        break;
                    case "http://127.0.0.1/test/": responseString = "<HTML><BODY> Deu Teste</BODY></HTML>";
                        break;
                    default: responseString = "<HTML><BODY>404</BODY></HTML>";
                        break;
                }
                
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
                listener.Stop();
            }
         
        }
    }


}
