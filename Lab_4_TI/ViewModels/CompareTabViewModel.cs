
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Lab_4_TI.Models;

namespace Lab_4_TI.ViewModels
{
    public class CompareTabViewModel : INotifyPropertyChanged
    {
        private string _leviyVvod = string.Empty;
        private string _praviyVvod = string.Empty;
        private string _resultat = string.Empty;

        public string LeviyVvod
        {
            get => _leviyVvod;
            set { _leviyVvod = value; NaSvoystvoIzmenilos(); }
        }

        public string PraviyVvod
        {
            get => _praviyVvod;
            set { _praviyVvod = value; NaSvoystvoIzmenilos(); }
        }

        public string TekstResultata
        {
            get => _resultat;
            set { _resultat = value; NaSvoystvoIzmenilos(); }
        }

        public ICommand KomandaSravnit => new RelayCommand(_ => Sravnit());
        public ICommand KomandaSkopirovat => new RelayCommand(_ => SkopirovatVBufer());

        private void Sravnit()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LeviyVvod) || string.IsNullOrWhiteSpace(PraviyVvod))
                {
                    TekstResultata = "Введите обе функции для сравнения";
                    return;
                }

                var levayaTablitsa = RazobratVvod(LeviyVvod);
                var pravayaTablitsa = RazobratVvod(PraviyVvod);

                var vsePeremennie = levayaTablitsa.Peremennie.Union(pravayaTablitsa.Peremennie).Distinct().OrderBy(v => v).ToList();

                var levieRezultati = new List<bool>();
                var pravieRezultati = new List<bool>();

                int kolichestvoStrok = 1 << vsePeremennie.Count;
                for (int i = 0; i < kolichestvoStrok; i++)
                {
                    var znacheniya = new Dictionary<string, bool>();
                    for (int j = 0; j < vsePeremennie.Count; j++)
                    {
                        znacheniya[vsePeremennie[j]] = ((i >> (vsePeremennie.Count - 1 - j)) & 1) == 1;
                    }

                    bool leviyResultat = VichislitFunktsiyu(LeviyVvod, znacheniya);
                    bool praviyResultat = VichislitFunktsiyu(PraviyVvod, znacheniya);

                    levieRezultati.Add(leviyResultat);
                    pravieRezultati.Add(praviyResultat);

                    if (leviyResultat != praviyResultat)
                    {
                        TekstResultata = $"Функции НЕ эквивалентны!\n\nПервое различающееся значение:\n";
                        TekstResultata += $"Кортеж: {string.Join(", ", vsePeremennie.Select(v => $"{v} = {znacheniya[v]}"))}\n";
                        TekstResultata += $"Левый результат: {leviyResultat}\n";
                        TekstResultata += $"Правый результат: {praviyResultat}";
                        return;
                    }
                }

                TekstResultata = "Функции ЭКВИВАЛЕНТНЫ!";
            }
            catch (Exception ex)
            {
                TekstResultata = $"Ошибка: {ex.Message}";
            }
        }

        private TruthTable RazobratVvod(string vvod)
        {
            if (vvod.StartsWith("n="))
            {
                var chasti = vvod.Split(new[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (chasti.Length >= 3 && int.TryParse(chasti[1], out int n) && ulong.TryParse(chasti[3], out ulong nomer))
                {
                    return TruthTable.IzNomera(n, nomer);
                }
                throw new Exception("Неверный формат номера функции. Используйте: n=3,nomer=11");
            }
            else
            {
                var lekser = new Lexer(vvod);
                var tokeni = lekser.Tokenize().ToList();
                var obramnayaZapis = ParserRpn.VObramnuyuPolskuyuZapis(tokeni);
                var peremennie = Evaluator.PoluchitPeremennie(obramnayaZapis);
                return TruthTable.IzFunktsii(obramnayaZapis, peremennie);
            }
        }

        private bool VichislitFunktsiyu(string vvod, Dictionary<string, bool> znacheniya)
        {
            if (vvod.StartsWith("n="))
            {
                var chasti = vvod.Split(new[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (chasti.Length >= 3 && int.TryParse(chasti[1], out int n) && ulong.TryParse(chasti[3], out ulong nomer))
                {
                    int indeks = 0;
                    var uporyadochenniePeremennie = Enumerable.Range(1, n).Select(i => $"x{i}").ToList();
                    for (int i = 0; i < uporyadochenniePeremennie.Count; i++)
                    {
                        if (znacheniya.ContainsKey(uporyadochenniePeremennie[i]) && znacheniya[uporyadochenniePeremennie[i]])
                        {
                            indeks |= 1 << (uporyadochenniePeremennie.Count - 1 - i);
                        }
                    }
                    return ((nomer >> indeks) & 1) == 1;
                }
                throw new Exception("Неверный формат номера функции");
            }
            else
            {
                var lekser = new Lexer(vvod);
                var tokeni = lekser.Tokenize().ToList();
                var obramnayaZapis = ParserRpn.VObramnuyuPolskuyuZapis(tokeni);
                return Evaluator.Vichislit(obramnayaZapis, znacheniya);
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NaSvoystvoIzmenilos([CallerMemberName] string imyaSvoystva = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(imyaSvoystva));
        }
    }
}