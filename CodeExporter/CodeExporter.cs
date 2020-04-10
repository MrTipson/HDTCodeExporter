using System;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using HearthDb.Deckstrings;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


namespace CodeExporter
{
    public class CodeExporter : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
        private Common.Config _cfg;
        private static string _configName = @"\config.json";

        private static int _elapsed = 0;
        private const int _delay = 100;
        private const int _interval = 1000;

        private static string _file;
        public string File
        {
            set
            {
                if (_file == null || !_file.Equals(value))
                {
                    _file = value;
                }
            }
            get
            {
                return _file;
            }
        }
        private string _deckString;
        public string DeckString
        {
            set
            {
                if (_deckString == null || !_deckString.Equals(value))
                {
                    _deckString = value;
                }
            }
            get
            {
                return _deckString;
            }
        }

        public void OnLoad()
        {
            try
            {
                _cfg = JsonConvert.DeserializeObject<Common.Config>(System.IO.File.ReadAllText(Config.Instance.ConfigDir + _configName));
                if (System.IO.File.Exists(_cfg.FilePath))
                {
                    File = _cfg.FilePath;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void OnUnload()
        {
            WriteConfig();
        }

        public void OnButtonPress()
        {
            SaveFileDialog openDialog = new SaveFileDialog();
            openDialog.Filter = "Text Files (*.txt) | *.txt";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                File = openDialog.FileName;
                WriteDeckCode(DeckString);
                WriteConfig();
            }
        }

        public void OnUpdate()
        {
            if (_elapsed > _interval)
            {
                _elapsed = 0;
                string deckString = DeckSerializer.Serialize(HearthDbConverter.ToHearthDbDeck(DeckList.Instance.ActiveDeck), false);

                if (deckString != null && !deckString.Equals(DeckString))
                {
                    WriteDeckCode(deckString);
                    DeckString = deckString;
                }
            }

            _elapsed += _delay;
        }

        private void WriteDeckCode(string deckString)
        {
            if (File != null && deckString != null)
            {
                try
                {
                    System.IO.File.WriteAllText(File, deckString);
                }
                catch (Exception ex) { }
            }
        }
        private void WriteConfig()
        {
            try
            {
                _cfg.FilePath = File;
                System.IO.File.WriteAllText(Config.Instance.ConfigDir + _configName, JsonConvert.SerializeObject(_cfg));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public string Name => "CodeExporter";

        public string Description => "Writes and updates current decklist into the selected file";

        public string ButtonText => "Choose file";

        public string Author => "Kanon & MrTipson";

        public Version Version => new Version(1, 0);

        public System.Windows.Controls.MenuItem MenuItem => null;
    }
}
