using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Lab_4_TI.Models;

namespace Lab_4_TI.ViewModels
{
    /// <summary>
    /// Модель представления для вкладки "По номеру"
    /// </summary>
    public class NumberTabViewModel : INotifyPropertyChanged
    {
        // Поля для хранения даных
        private int _n = 3;
        private string _tekstNomera = "11";
        private string _resultat = string.Empty;

        /// <summary>
        /// Количество переменых в функции
        /// </summary>
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

        /// <summary>
        /// Предупреждение при большом количестве переменых
        /// </summary>
        private void PredupreditOBolshomRazmere()
        {
            if (N >= 12)
            {
                MessageBox.Show($"Внимание: O(2^n) операций. n={N} -> 2^n={1 << N}", "Предупреждение");
            }
        }

        // Построение таблицы истинности
        private void PostroitTablitsu()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!BigInteger.TryParse(TekstNomera, out BigInteger nomer))
                {
                    TekstResultata = "Неверный формат номера";
                    return;
                }

                var tablitsa = TruthTable.IzNomera(N, nomer);
                var peremennie = Enumerable.Range(1, N).Select(i => $"x{i}").ToList();

                TekstResultata = $"Таблица истинности (n={N}, номер={nomer}):\n\n";
                TekstResultata += TruthTable.TablitsaVStroku(tablitsa, peremennie);

                TekstResultata += $"\nПояснение: номер={nomer} в двоичной системе: {KVDvoichnoy(nomer, 1 << N)}\n";
                TekstResultata += "Биты соответствуют значениям функции на кортежах в порядке возрастания\n";
            }
            catch (Exception ex)
            {
                TekstResultata = $"Ошибка: {ex.Message}";
            }
        }

        // Построение СДНФ
        private void PostroitSDNF()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!BigInteger.TryParse(TekstNomera, out BigInteger nomer))
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

        // Построение СКНФ
        private void PostroitSKNF()
        {
            try
            {
                PredupreditOBolshomRazmere();
                if (!BigInteger.TryParse(TekstNomera, out BigInteger nomer))
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

        private static string KVDvoichnoy(BigInteger value, int width)
        {
            if (value < 0)
            {
                return "-" + KVDvoichnoy(BigInteger.Negate(value), width);
            }

            var chars = new char[width];
            for (int i = 0; i < width; i++)
            {
                bool bit = ((value >> i) & BigInteger.One) == BigInteger.One;
                chars[width - 1 - i] = bit ? '1' : '0';
            }
            return new string(chars);
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
