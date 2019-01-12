using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class Export : ContentResult
    {
        private string fileName;
        public byte[] fileData;
        public Export(string name, byte[] data)
        {
            fileName = name;
            fileData = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var contentDisposition = string.Format("attachment; filename={0}", this.fileName);
            context.HttpContext.Response.AddHeader("Content-type", "application/force-download");
            context.HttpContext.Response.AddHeader("Content-Disposition",
                    string.Format("attachment; filename = \"{0}\"",
                    System.IO.Path.GetFileName(this.fileName)));
            ContentType = "application/force-download";
            context.HttpContext.Response.BinaryWrite(this.fileData);
        }
    }
}