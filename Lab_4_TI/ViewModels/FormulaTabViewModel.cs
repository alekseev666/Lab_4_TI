using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Lab_4_TI.Models;

namespace Lab_4_TI.ViewModels
{
    public class FormulaTabViewModel : INotifyPropertyChanged
    {
        private string _tekstFormuli = string.Empty;
        private string _resultat = string.Empty;

        public string TekstFormuli
        {
            get => _tekstFormuli;
            set { _tekstFormuli = value; NaSvoystvoIzmenilos(); }
        }

        public string TekstResultata
        {
            get => _resultat;
            set { _resultat = value; NaSvoystvoIzmenilos(); }
        }

        public ICommand KomandaRazobratIVichislit => new RelayCommand(_ => RazobratIVichislit());
        public ICommand KomandaSkopirovat => new RelayCommand(_ => SkopirovatVBufer());
        public ICommand KomandaPreset1 => new RelayCommand(_ => ZagruzitPreset1());
        public ICommand KomandaPreset2 => new RelayCommand(_ => ZagruzitPreset2());

        private void RazobratIVichislit()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TekstFormuli))
                {
                    TekstResultata = "Введите формулу";
                    return;
                }

                var lekser = new Lexer(TekstFormuli);
                var tokeni = lekser.Tokenize().ToList();
                
                // Отладочный вывод
                var tokeniStr = string.Join(" ", tokeni.Select(t => t.Tekst));
                
                var obramnayaZapis = ParserRpn.VObramnuyuPolskuyuZapis(tokeni);
                var peremennie = Evaluator.PoluchitPeremennie(obramnayaZapis);
                
                // Отладочный вывод ОПЗ
                var opzStr = string.Join(" ", obramnayaZapis.Select(t => t.Tekst));

                if (peremennie.Count > 8)
                {
                    TekstResultata = $"Внимание: {peremennie.Count} переменных -> {1 << peremennie.Count} строк таблицы";
                }

                var tablitsa = TruthTable.IzFunktsii(obramnayaZapis, peremennie);

                TekstResultata = $"Таблица истинности для формулы: {TekstFormuli}\n";
                TekstResultata += $"Лексемы: {tokeniStr}\n";
                TekstResultata += $"ОПЗ: {opzStr}\n\n";
                TekstResultata += TruthTable.TablitsaVStroku(tablitsa, peremennie);

                var sdnf = DnfKnfGenerator.VSDNF(tablitsa, peremennie);
                var sknf = DnfKnfGenerator.VSKNF(tablitsa, peremennie);
                var podschetSDNF = DnfKnfGenerator.PodschitatSDNF(sdnf);
                var podschetSKNF = DnfKnfGenerator.PodschitatSKNF(sknf);

                TekstResultata += $"\nСДНФ:\n{sdnf}\n";
                TekstResultata += $"Литералы: {podschetSDNF.literali}, Конъюнкты: {podschetSDNF.konYunktsii}, Дизъюнкты: {podschetSDNF.disYunktsii}\n\n";

                TekstResultata += $"СКНФ:\n{sknf}\n";
                TekstResultata += $"Литералы: {podschetSKNF.literali}, Конъюнкты: {podschetSKNF.konYunktsii}, Дизъюнкты: {podschetSKNF.disYunktsii}";
            }
            catch (Exception ex)
            {
                TekstResultata = $"Ошибка: {ex.Message}";
            }
        }

        private void SkopirovatVBufer()
        {
            if (!string.IsNullOrEmpty(TekstResultata))
            {
                Clipboard.SetText(TekstResultata);
                MessageBox.Show("Результат скопирован в буфер обмена", "Успех");
            }
        }

        private void ZagruzitPreset1()
        {
            TekstFormuli = "(x1 & !x2) | x3";
            RazobratIVichislit();
        }

        private void ZagruzitPreset2()
        {
            TekstFormuli = "(x1 | x2) -> x3";
            RazobratIVichislit();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NaSvoystvoIzmenilos([CallerMemberName] string? imyaSvoystva = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(imyaSvoystva));
        }
    }
}