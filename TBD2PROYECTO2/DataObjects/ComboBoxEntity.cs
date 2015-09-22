namespace TBD2PROYECTO2.DataObjects
{
    public class ComboBoxEntity
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}