using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace Visitare
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}