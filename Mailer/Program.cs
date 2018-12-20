using System;
using System.Timers;
using System.Net;
using System.Net.Mail;

namespace sendmessagetimer
{
   public class Program
   {
       private static Timer _clock;
       private static Object _lock = new Object();
       private static int _count = 0;

       public object ThisLock { get => _lock; set => _lock = value; }

       public static void Main(string[] args)
       {
           System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               (s, cert, chain, sslPolicyErrors) => true;

           _clock = new System.Timers.Timer();
           _clock.Interval = 1000;
           _clock.Elapsed += TimeEvent;
           _clock.AutoReset = true;
           _clock.Enabled = true;

           Console.WriteLine("Zaczynamy wysylanie!");
           Console.ReadLine();
       }

       private static void TimeEvent(Object source, ElapsedEventArgs e)
       {
           lock (_lock)
           {
               if (_count > 5) return;

               var message = new MailMessage
               {
                   From = new MailAddress("tomotest8@gmail.com"),
                   Subject = "Temat maila",
                   Body = "Siema ziomek, jak leci? Masz plik. Elo!"
               };
               message.To.Add(new MailAddress("tomaszanuszczyk@gmail.com"));

               var smtp = new SmtpClient("smtp.gmail.com", 587)
               {
                   UseDefaultCredentials = false,
                   EnableSsl = true,
                   
                   Credentials = new NetworkCredential(userName: "tomotest8@gmail.com", password: "Qwertyu123#")
               };

               //Attachment file = new Attachment(@"C:\pliki\z8.pdf");
               //message.Attachments.Add(file);

               _count++;
               if (_count <= 5)
               {
                   smtp.Send(message);
                   Console.WriteLine("Wyslano maila nr {0}!", _count);
               }

               if (_count != 5) return;
               _clock.Stop();
               Console.WriteLine("Koniec wysylania!");
           }
       }
   }
}