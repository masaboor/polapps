using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;
using System.Data;

/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
    public Email()
    { }

    public static void SendEmail(string TO, string CC, string Subject, string Body)
    {
        MailMessage mm = new MailMessage();
        mm.From = new MailAddress(WebConfigurationManager.AppSettings["SMTPEmailDisplay"]);
        mm.IsBodyHtml = true;

        string[] to = TO.Split(';');
        for (int i = 0; i < to.Length; i++)
            for (int j = 0; j < to.Length; j++)
                if (to[i] == to[j] && i != j)
                    to[j] = "";

        for (int i = 0; i < to.Length; i++)
            if (to[i] != "")
                mm.To.Add(to[i]);

        if (CC != "")
        {
            string[] cc = CC.Split(';');
            for (int i = 0; i < cc.Length; i++)
                for (int j = 0; j < cc.Length; j++)
                    if (cc[i] == cc[j] && i != j)
                        cc[j] = "";
            for (int i = 0; i < cc.Length; i++)
                if (cc[i] != "")
                    mm.CC.Add(cc[i]);
        }

        mm.Subject = Subject;
        mm.Body = Body;

        NetworkCredential NetworkCred = new NetworkCredential(WebConfigurationManager.AppSettings["SMTPEmailLogin"]
            , WebConfigurationManager.AppSettings["SMTPPassword"]);

        SmtpClient smtp = new SmtpClient();
        smtp.Host = WebConfigurationManager.AppSettings["SMTPHost"]; // "10.85.1.237";
        smtp.EnableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["SMTPSSL"]);   //Server does not support secure connections.
        smtp.UseDefaultCredentials = true;
        smtp.Credentials = NetworkCred;
        smtp.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["SMTPPort"]);
        smtp.Send(mm);
    }

    public static void ExportToSpreadsheet(DataTable table, string name)
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();
        foreach (DataColumn column in table.Columns)
        {
            context.Response.Write((column.ColumnName + '\t'));
        }

        context.Response.Write(Environment.NewLine);
        foreach (DataRow row in table.Rows)
        {
            for (int i = 0; (i <= (table.Columns.Count - 1)); i++)
            {
                context.Response.Write((row[i].ToString().Replace(";", String.Empty) + '\t'));
            }

            context.Response.Write(Environment.NewLine);
        }

        // context.Response.ContentType = "text/csv";
        context.Response.ContentType = "application/vnd.ms-excel";
        context.Response.AppendHeader("Content-Disposition", ("attachment; filename=" + (name + ".xls")));
        context.Response.End();
    }
}