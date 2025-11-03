using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Lab_4_TI.Models;

namespace Lab_4_TI.ViewModels
{
    public class NumberTabViewModel : INotifyPropertyChanged
    {
        private int _n = 3;
        private string _tekstNomera = "11";
        private string _resultat = string.Empty;

        public int N
        {
            get => _n;
            set { _n = value; NaSvoystvoIzmenilos(); }
        }

        public string TekstNomera
        {
            get => _tekstNomera;
            set { _tekstNomera = value; NaSvoystvoIzmenilos(); }
        }

        public string TekstResultata
        {
            get => _resultat;
            set { _resultat = value; NaSvoystvoIzmenilos(); }
        }

        public ICommand KomandaPostroitTablitsu => new RelayCommand(_ => PostroitTablitsu());
        public ICommand KomandaPostroitSDNF => new RelayCommand(_ => PostroitSDNF());
        public ICommand KomandaPostroitSKNF => new RelayCommand(_ => PostroitSKNF());
        public ICommand KomandaSkopirovat => new RelayCommand(_ => SkopirovatVBufer());
        public ICommand KomandaPreset1 => new RelayCommand(_ => ZagruzitPreset1());

        private void PredupreditOBolshomRazmere()
        {
            if (N >= 12)
            {
                MessageBox.Show($"Внимание: O(2^n) операций. n={N} -> 2^n={1 << N}", "Предупреждение");
            }
        }

        private void PostroitTablitsu()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!ulong.TryParse(TekstNomera, out ulong nomer))
                {
                    TekstResultata = "Неверный формат номера";
                    return;
                }

                var tablitsa = TruthTable.IzNomera(N, nomer);
                var peremennie = Enumerable.Range(1, N).Select(i => $"x{i}").ToList();

                TekstResultata = $"Таблица истинности (n={N}, номер={nomer}):\n\n";
                TekstResultata += TruthTable.TablitsaVStroku(tablitsa, peremennie);

                TekstResultata += $"\nПояснение: номер={nomer} в двоичной системе: {Convert.ToString((long)nomer, 2).PadLeft(1 << N, '0')}\n";
                TekstResultata += "Биты соответствуют значениям функции на кортежах в порядке возрастания\n";
            }
            catch (Exception ex)
            {
                TekstResultata = $"Ошибка: {ex.Message}";
            }
        }

        private void PostroitSDNF()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!ulong.TryParse(TekstNomera, out ulong nomer))
                {
                    TekstResultata = "Неверный формат номера";
                    return;
                }

                var tablitsa = TruthTable.IzNomera(N, nomer);
                var peremennie = Enumerable.Range(1, N).Select(i => $"x{i}").ToList();
                var sdnf = DnfKnfGenerator.VSDNF(tablitsa, peremennie);
                var podschet = DnfKnfGenerator.PodschitatSDNF(sdnf);

                TekstResultata = $"СДНФ (n={N}, номер={nomer}):\n\n{sdnf}\n\n";
                TekstResultata += $"Литералы: {podschet.literali}, Конъюнкты: {podschet.konYunktsii}, Дизъюнкты: {podschet.disYunktsii}";
            }
            catch (Exception ex)
            {
                TekstResultata = $"Ошибка: {ex.Message}";
            }
        }

        private void PostroitSKNF()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!ulong.TryParse(TekstNomera, out ulong nomer))
                {
                    TekstResultata = "Неверный формат номера";
                    return;
                }

                var tablitsa = TruthTable.IzNomera(N, nomer);
                var peremennie = Enumerable.Range(1, N).Select(i => $"x{i}").ToList();
                var sknf = DnfKnfGenerator.VSKNF(tablitsa, peremennie);
                var podschet = DnfKnfGenerator.PodschitatSKNF(sknf);

                TekstResultata = $"СКНФ (n={N}, номер={nomer}):\n\n{sknf}\n\n";
                TekstResultata += $"Литералы: {podschet.literali}, Конъюнкты: {podschet.konYunktsii}, Дизъюнкты: {podschet.disYunktsii}";
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
            N = 3;
            TekstNomera = "11";
            PostroitTablitsu();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NaSvoystvoIzmenilos([CallerMemberName] string? imyaSvoystva = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(imyaSvoystva));
        }
    }
}