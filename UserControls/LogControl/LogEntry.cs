using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WExtraControlLibrary.UserControls.LogControl
{
    public class LogEntry : PropertyChangedBase
    {
        public string Message { get; set; }
    }

    public class ActionLogEntry : LogEntry
    {
        private static Dictionary<string, Color> FcolorMapSYSID = new Dictionary<string, Color>();
        private static Dictionary<string, Color> BcolorMapACTIONS = new Dictionary<string, Color>();

        private static readonly List<Color> colorListDark = new List<Color>{ Colors.Blue, Colors.Red, Colors.DarkGreen, Colors.DarkOrange, Colors.DarkViolet };
        private static readonly Dictionary<string, Color> actionsColorDict = new Dictionary<string, Color> { 
                                                                                                        {"START REC", Colors.Red },
                                                                                                        {"STOP REC", Colors.Red },
                                                                                                        {"READY", Colors.Green },
                                                                                                        {"NOT READY", Colors.OrangeRed },
                                                                                                        {"TRIGGER", Colors.Orange },
                                                                                                        {"CHANGE", Colors.Green },
                                                                                                        {"STATUS", Colors.Blue },

        };

        private enum TYPE
        {
            ACTION,
            SYSID
        }

        public Color FSYSIDColor { get; set; }
        public Color BActionsColor { get; set; }

        public DateTime EventTime { get; set; }

        private string name;
        public string Name {
            get { return name;  }
            set {
                name = value;
            }
        }

        private string eqID;
        public string EqID
        {
            get { return eqID; }
            set
            {
                eqID = value;
                FSYSIDColor = getColor(eqID, TYPE.SYSID);
            }

        }

        private string actionCODE;
        public string ActionCODE {
            get { return actionCODE;  }
            set {
                actionCODE = value;
                BActionsColor = getColor(actionCODE, TYPE.ACTION);
            }
        }

        private static Color getColor(string key, TYPE type)
        {
            Color color = Colors.Black;

            switch (type)
            {
                case TYPE.ACTION:
                    if (!BcolorMapACTIONS.TryGetValue(key, out color))
                    {
                        color = getActionColor(key);
                        BcolorMapACTIONS.Add(key, color);
                    }
                    break;
                case TYPE.SYSID:
                    if (!FcolorMapSYSID.TryGetValue(key, out color))
                    {
                        color = getRandomColor();
                        FcolorMapSYSID.Add(key, color);
                    }
                    break;
            }

            return color;
        }

        private static Color getRandomColor()
        {
            return colorListDark[ new Random().Next(0, colorListDark.Count-1)];
        }

        private static Color getActionColor(string action)
        {
            Color c = Colors.BlueViolet;
            actionsColorDict.TryGetValue(action, out c);
            return c;
        }
    }


    public class CollapsibleActionLogEntry : ActionLogEntry
    {
        public List<LogEntry> Contents { get; set; }
    }


}
