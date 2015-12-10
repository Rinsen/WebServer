using System;
using Microsoft.SPOT;

using System.Collections;


namespace Rinsen.WebServer.FileAndDirectoryServer
{
    public class BoundaryHeaderCollection : Hashtable
    {
        public BoundaryHeaderCollection():base(){}

        public BoundaryHeaderCollection(string HeaderData)
            : base()
        {
            var arrHeaders = HeaderData.Trim().Split('\n');
            foreach (var strLine in arrHeaders)
            {
                var arrHeaderKeyValue = strLine.Split(':');
                Add(arrHeaderKeyValue[0].Trim().ToLower(), arrHeaderKeyValue[1]);
            }
        }

        //public BoundaryHeaderCollection(string HeaderData)
        //    : base()
        //{
        //    var arrHeaders = HeaderData.Trim().Split('\n');
        //    foreach (var strLine in arrHeaders)
        //    {
        //        Debug.Print("Header: " + strLine);
        //        var arrHeaderKeyValue = strLine.Split(':');
        //        var arrSubKeys = arrHeaderKeyValue[1].Split(';');
        //        var arrSubKeyKeyValues = new Hashtable();
        //        foreach (var strValue in arrSubKeys)
        //        {
        //            Debug.Print("Subkey: " + strValue);
        //            if (strValue.IndexOf(';') != -1)
        //            {
        //                var arrTemp = strValue.Split(';');
        //                arrSubKeyKeyValues.Add(arrTemp[0].Trim(), arrTemp[1]);
        //            }
        //            else
        //                arrSubKeyKeyValues.Add(strValue.Trim(), string.Empty);
        //        }
        //        Add(arrHeaderKeyValue[0].Trim(), arrSubKeyKeyValues);
        //    }
        //}
    }
}
