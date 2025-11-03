using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4_TI.ViewModels
{
    public class MainViewModel
    {
        public NumberTabViewModel VkladkaPoNomeru { get; } = new NumberTabViewModel();
        public FormulaTabViewModel VkladkaPoFormule { get; } = new FormulaTabViewModel();
        public CompareTabViewModel VkladkaSravnenie { get; } = new CompareTabViewModel();
    }
}