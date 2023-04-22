
using HW2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Features
{
    internal abstract class BaseFeature
    { 
        protected string Name = "Basic feature. Need to be renamed!";
        protected string Description = "";
        protected string InputRequirements = "Nothing";
        protected string DefaultInputRequirments = "Nothing";
        public bool HasDefaultInput { get; init; } = false;

        public virtual string GetName()
        {
            return Name;
        }
        public virtual string ShowFullFormat(bool isDefault = false)
        {
            var result  = new StringBuilder();
            var contentSeparator = new string(ConsoleHelper.ContentSeparator, ConsoleHelper.LineLenght);
            result.AppendLine(contentSeparator);
            foreach ( var line in ConsoleHelper.SplitLongLine(Name))
            {
                result.AppendLine(line);
            }
            result.AppendLine(contentSeparator);
            if(Description != null && Description.Length > 0) {
                foreach (var line in ConsoleHelper.SplitLongLine(Description))
                {
                    result.AppendLine(line);
                }
                result.AppendLine(contentSeparator);
            }
            result.AppendLine("Input requirments:");
            if(isDefault)
            {
                foreach (var line in ConsoleHelper.SplitLongLine(DefaultInputRequirments))
                {
                    result.AppendLine(line);
                }
            }
            else
            {
                foreach (var line in ConsoleHelper.SplitLongLine(InputRequirements))
                {
                    result.AppendLine(line);
                }
            }
            
            return result.ToString();
        }

        public abstract void Run(bool isDefaultInput = false);
    }
}
