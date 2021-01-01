using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Utilities
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(tblCertificates certificate)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body class='text-center'>
                                <br>
                               <img src='http://rpa.co.zw/assets/img/logo.png' class='img-responsive img-fluid' height = 150>
                                <div class='header'><h1>Certificate</h1></div>
                                <br>
                               <h4>This is to Certify that </h4>
                                <h2>{0} </h2>
                                <h4>has successfully completed</h4> 
                                <h2>{1}</h2>
                               <h4>and has been awarded <b>{2} </b>points.</h4>"
                , certificate.tblPharmacists.ApplicationUser.FullName, certificate.Event.EventName, certificate.Event.EventPoints);
            sb.Append(@"                          
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
