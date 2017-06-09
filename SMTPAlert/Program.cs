
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
namespace Examples.SmptExamples.Async
{
    public class SimpleAsynchronousExample
    {
        // we tail the IIS/APACHE logfile with NxLog and if any event we call exec_async to fire this alert to it admins
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        public static void Main(string[] args)
        {
            //Console.WriteLine("args length " + args.Length);
            string myArgs;
            if (args.Length == 0) { myArgs = ""; }
            else { myArgs = args[0]; }
            //Console.WriteLine("args value " + myArgs);

            SmtpClient client = new SmtpClient("MAILSERVER.SOME.WHERE.LOCAL");
            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("Alert@your.org",
               "Alert " + " DNS Sink " + (char)0xD8,
            System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress("notify@your.org");
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = "Please check the log file at C:\\inetpub\\logs\\LogFiles\\W3SVC3\\*.log on MONITORED SERVER for Event Details ";
            message.Body += Environment.NewLine + "\r\n" + myArgs; 
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Suspicious Activity!!!!";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.Send(message);
            // Clean up.
            message.Dispose();
            //Console.WriteLine("Goodbye.");
            //Console.ReadLine();
        }
    }
}