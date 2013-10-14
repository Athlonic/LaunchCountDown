using System.Collections.Generic;
using UnityEngine;

namespace LaunchCountDown.Extensions
{
    public static class PartExtensions
    {
        public static bool IsPrimary(this Part thisPart, List<Part> partslist, int moduleClassID)
        {
            foreach (Part part in partslist)
            {
                if (part.Modules.Contains(moduleClassID))
                {
                    if (part == thisPart)
                        return true;
                    else
                        break;
                }
            }
            
            
            
            return false;
        }
    }
}
