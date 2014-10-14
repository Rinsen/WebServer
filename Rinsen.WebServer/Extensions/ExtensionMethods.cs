using System;
using Microsoft.SPOT;
using System.Collections;

namespace Rinsen.WebServer.Extensions
{
    public static class ExtensionMethods
    {
        public static void Append(this ArrayList targetArrayList, ArrayList arrayList)
        {
            foreach (var item in arrayList)
	        {
                targetArrayList.Add(item);
        	}
        }
    }
}
